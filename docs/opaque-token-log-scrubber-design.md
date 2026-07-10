# Design: Client-Side Opaque-Token Log Scrubber for MSAL (.NET / Python / JS / Java)

> Status: Draft design for review. Go is excluded — the MSAL Go SDK emits no logs, so there is
> nothing to hook. This document describes the client half of the eSTS/CCS Highly Identifiable
> Token (HIT) tagging system: an inline scrubber that redacts tagged opaque tokens from log output
> before they reach any sink.

## Problem

The server side (eSTS + CCS) now tags every outgoing **opaque** token with a
Highly Identifiable Token (HIT) tag so that tokens can be detected and redacted
wherever they appear. The tag is a static, base64-detectable prefix:

- **eSTS:** `EvoStsArtifacts` embedded as `{header}{EvoStsArtifacts}{CallerId 4B}{2B padding}{body}`,
  then the whole byte array is base64-encoded (fixed 21-byte tag).
- **MSA:** literal `MsaArtifacts` prefix.

The client half of this system is an **inline log scrubber** in each MSAL SDK that
recognises those patterns in log output and redacts the token **before it is written
to any sink**, so tokens never land in logs. The attached `HitTagForOpaqueTokensPatterns`
class captures the base64 patterns for the 3 possible byte-alignment offsets plus the MSA literal.

Goal of this task: a **cross-SDK design plan** (shared spec + per-language sections) and a
**gap analysis** of the current pattern approach.

## Scope

**In scope**
- Detecting and redacting opaque tokens (auth codes, flow tokens, blob grants, and any
  future eSTS/CCS opaque token) in each SDK's own log output.
- A single, language-agnostic scrubber specification that all 5 SDKs implement identically.
- Shared cross-SDK test vectors to guarantee parity.

**Out of scope**
- JWTs (ID tokens, access tokens) — not tagged, not flowing through the tagged path.
- App-level logging done by the host application outside MSAL.
- Server-side tagging (already designed/rolled out per the attachment).

## Background: what the scrubber keys off

- Tag lives **after the header**, not at the very start of the token, so the base64 offset
  of `EvoStsArtifacts` depends on `headerLength % 3` → three patterns:
  - offset 0: `RXZvU3RzQXJ0aWZhY3Rz` (full 20-char base64 of the 15-byte prefix)
  - offset 1: `V2b1N0c0FydGlmYWN0c` (last char dropped — encodes following CallerId bits)
  - offset 2: `dm9TdHNBcnRpZmFjdH` (last char dropped)
- These prefixes contain only `[A-Za-z0-9]`, so they are identical in base64 and base64url.

## Shared Scrubber Specification (all SDKs implement this identically)

1. **Pattern set (data, not code):** the 3 eSTS offset patterns + MSA form(s). Kept in one
   place per SDK, ideally generated from a shared source so parity is mechanical.
2. **Matching:** case-sensitive / ordinal. Cheap pre-filter (substring test for a rare anchor,
   e.g. `Artifacts` / `RXZ`) before running the full matcher, to keep the hot path fast.
3. **Boundary detection (the redaction extent):** on a hit, expand **both left and right** from
   the match over the base64/base64url charset `[A-Za-z0-9+/_-]` (plus `=` padding), stopping at
   a delimiter (whitespace, quote, `<>`, `,`, `&`, `;`, backslash, control char). Left expansion is
   required because the tag sits after the header. Also handle percent-encoded boundaries in URLs.
4. **Redaction output:** replace the run with a fixed placeholder, e.g.
   `[Redacted opaque token]`. Optional: decode the 4-byte CallerId from the tag to emit the
   token type, e.g. `[Redacted AuthCode token]` (non-sensitive, useful for triage).
5. **Config:** on by default (defense in depth, independent of the PII flag); provide a kill switch.
6. **Telemetry:** count redactions (optionally by token type) **without** logging the value.
7. **Test vectors:** shared corpus (all 3 offsets × base64 + base64url × eSTS + MSA, embedded in
   realistic log lines, URLs and JSON). Same corpus runs in all 5 SDKs.

## How it actually works (mechanism)

### The core scrub algorithm (one function, ported to each SDK)

Input: a finished log line `s`. Output: `s` with any tagged token run replaced by a placeholder.

```
scrub(s):
    # 1. Cheap pre-filter: skip 99.9% of lines with one substring test.
    #    All three eSTS patterns share the ascii-safe anchor "Artifacts" once
    #    base64-decoded is irrelevant — instead anchor on a literal substring that
    #    is present in ALL offset patterns. Simplest: test for each pattern's first
    #    ~6 chars, or use a prebuilt Aho-Corasick automaton over the pattern set.
    if not s contains any of {ANCHORS}: return s        # no allocation, early out

    # 2. Find every match of the pattern set (3 eSTS offsets + MSA form).
    result = s
    for each match at index i, length L in findAll(result, PATTERNS):
        # 3. Expand to the full token run (BOTH directions — tag is mid-token).
        start = expandLeft(result, i)     # walk left while char in TOKEN_CHARSET
        end   = expandRight(result, i+L)  # walk right while char in TOKEN_CHARSET
        # 4. Replace the run with a placeholder (optionally decode CallerId).
        result = result[:start] + PLACEHOLDER + result[end:]
    return result
```

Key definitions:

- `PATTERNS` = `{ "RXZvU3RzQXJ0aWZhY3Rz", "V2b1N0c0FydGlmYWN0c", "dm9TdHNBcnRpZmFjdH", <MSA form> }`
  — case-sensitive/ordinal. Compiled once at startup, never per-call.
- `TOKEN_CHARSET` = base64 + base64url + padding: `[A-Za-z0-9+/_\-=]`. In URL context also treat
  `%` percent-escapes as part of the run so we don't stop early on `%2B`/`%2F`/`%3D`.
- `expandLeft`/`expandRight` walk until a non-token delimiter (whitespace, `"' <>,;&\` , control
  chars, end/start of string). Left expansion is mandatory because the tag sits **after the
  header** (`{header}{tag}{body}`), so bytes precede the matched prefix.
- `PLACEHOLDER` = `[Redacted opaque token]` (or `[Redacted <TokenType> token]` if we decode the
  4-byte CallerId that immediately follows the tag prefix).

### Worked example (why 3 offsets + left-expansion)

A token is `base64( {header}{EvoStsArtifacts}{callerId}{pad}{body} )`. Depending on
`len(header) % 3`, the encoder emits the prefix at a different character alignment, so its base64
rendering is one of the three `PATTERNS`. When we find e.g. `dm9TdHNBcnRpZmFjdH` at index `i`,
the real token started ~`ceil(headerLen*4/3)` chars to the **left** of `i` — we don't know the
exact header length, so we expand left over `TOKEN_CHARSET` until a delimiter. Right-expansion
covers the `{callerId}{body}` remainder. Net: the entire opaque blob is replaced, header included.

### Matching engine choice per language

- One compiled **Aho-Corasick** automaton over the 4 patterns is the fastest, allocation-free
  option and scales if more patterns are added. A compiled multi-alternation regex
  (`RXZvU3RzQXJ0aWZhY3Rz|V2b1N0c0FydGlmYWN0c|dm9TdHNBcnRpZmFjdH|...`) is simpler and fine given
  the cheap pre-filter. Decision can differ per SDK based on available libs.

### Redaction telemetry (safe)

Increment a counter `tokensScrubbed` (optionally keyed by decoded CallerId/token type) inside the
replace step. Never log the matched substring. This lets us measure coverage/leaks in the field.

### Worked example on a real log line (from an S2SAuth error blob)

Given this fragment that MSAL/S2S logged (an `HttpResponseMessage` embedded inside an error string):

```
Set-Cookie: esctx-uYBOwwKr8Sg=AQABCQEAAAAdDD7nC9b5Q7JPd_okEQRFRXZvU3RzQXJ0aWZhY3RzDQAAAAAAwEvgY46V...Q_kqa0hyAA; domain=.login.microsoftonline.com; path=/; secure; HttpOnly; SameSite=None
Set-Cookie: fpc=Ag2n43XSImFAtuJReI4UMyOaZMaiAQAAAF3I2eEOAAAA; expires=...
```

Trace of `scrub()`:
1. Pre-filter hits (the `RXZ...` anchor is present).
2. Match: offset-0 pattern `RXZvU3RzQXJ0aWZhY3Rz` is found inside the `esctx` value
   (`...EQRF` **`RXZvU3RzQXJ0aWZhY3Rz`** `DQAAAA...`). **→ Yes, this token is scrubbed.**
3. Expand right over `TOKEN_CHARSET` until the `;` after `...Q_kqa0hyAA` → stops at the cookie
   delimiter. Expand left over `TOKEN_CHARSET` through `=` and the cookie name back to the space
   after `Set-Cookie:`.
4. Replace → `Set-Cookie: [Redacted opaque token]; domain=.login.microsoftonline.com; ...`

Three things this example proves — all now reflected in the plan/gap-analysis:
- **base64url matters.** The `esctx` value uses `-`/`_` (`Pd_ok`, `PKY--1D1oJ`). Detection still
  works (the pattern is all-alnum) **only because** boundary expansion includes `-_` in
  `TOKEN_CHARSET`. Confirmed necessary (Gap #1).
- **Over-redaction of the cookie name** (`esctx-uYBOwwKr8Sg=`) happens because left-expansion
  stops at whitespace, not `=`. This is acceptable (name isn't the secret) but is a design choice
  to ratify: stop left-expansion at `=`/`:` to preserve the cookie name if desired.
- **Untagged values are NOT scrubbed.** The second cookie `fpc=Ag2n43XS...` and
  `x-ms-gateway-slice=estsfd` carry **no** HIT tag, so the scrubber leaves them. This is the
  whole point (only server-tagged opaque tokens are redacted) but must be understood: anything
  eSTS/CCS does not tag will pass through (Gap #2 / coverage scope). If `fpc` is deemed sensitive
  it needs its own tag server-side.

Also note this token only got logged because the SDK logged an entire `HttpResponseMessage`
inside an error string — reinforcing that the scrubber must sit at the **log choke point** (so it
sees the fully-assembled error/exception text), not only at token-handling call sites (Gap #9).

## Per-SDK integration (detailed designs — Go excluded: it has no logger)

### .NET — `microsoft-authentication-library-for-dotnet`

- **New file:** `src/client/Microsoft.Identity.Client/Internal/Logger/TokenScrubber.cs` (`internal static`).
- **Patterns:** private static readonly array of the 3 eSTS patterns + MSA form. Matched with
  ordinal `string.IndexOf` + a hand-rolled boundary scan (zero allocation on the no-match path).
- **Single integration point (verified):** both `IdentityLogger.Log()` and
  `CallbackIdentityLogger.Log()` call `LoggerHelper.FormatLogMessage()` as their last step, so scrub
  there once to cover **both** the `LogCallback` and `IIdentityLogger` public paths:
  ```csharp
  public static string FormatLogMessage(string message, bool piiEnabled, string correlationId, string clientInformation)
  {
      string formatted = string.Format(CultureInfo.InvariantCulture,
          "{0} MSAL {1} {2} {3} {4} [{5}{6}]{7} {8}",
          piiEnabled, s_msalVersionLazy.Value, s_skuLazy.Value, s_runtimeVersionLazy.Value,
          s_osLazy.Value, DateTime.UtcNow.ToString("u"), correlationId, clientInformation, message);
      return TokenScrubber.Scrub(formatted);   // <-- single choke point, both loggers covered
  }
  ```
  Running here means the scrubber sees the fully-assembled line, so it also catches tokens buried
  inside a logged `HttpResponseMessage` (the `esctx` case).
- **Fast path:** `ContainsAnyPattern` (ordinal `IndexOf` per pattern); return the original reference
  unchanged when nothing matches → no allocation.
- **Config / kill switch:** a static `AppContext` switch (e.g.
  `Microsoft.Identity.Client.DisableOpaqueTokenScrubbing`) checked once and cached, default ON.
  Keep it independent of `EnablePiiLogging`.
- **Public API analyzer:** keep `TokenScrubber` `internal`; add to `InternalAPI.Unshipped.txt`:
  `Microsoft.Identity.Client.Internal.Logger.TokenScrubber` and
  `static Microsoft.Identity.Client.Internal.Logger.TokenScrubber.Scrub(string message) -> string`.
- **Tests:** MSTest in `tests/Microsoft.Identity.Test.Unit/CoreTests/LoggerTests/TokenScrubberTests.cs`
  — drive the shared corpus (all 3 offsets, base64 + base64url, MSA, the `esctx` cookie sample),
  assert redaction + no false positives on ordinary messages, plus a no-match perf check.
- **Frameworks:** no `#if` guards needed — `string.IndexOf(string, int, StringComparison)`,
  `StringBuilder`, and relational patterns all exist on `netstandard2.0`/`net462`/`net8.0`/`net9.0`.
  (Guards only become necessary if switching to `[GeneratedRegex]`.)

### JavaScript/TypeScript — `microsoft-authentication-library-for-js`

- **New file:** `lib/msal-common/src/logger/TokenScrubber.ts`, exported from `msal-common` so
  `msal-browser` and `msal-node` inherit one implementation.
- **Patterns:** module-level `const PATTERNS` + a single compiled `RegExp`
  (`/RXZvU3RzQXJ0aWZhY3Rz|V2b1N0c0FydGlmYWN0c|dm9TdHNBcnRpZmFjdH|.../g`), case-sensitive.
- **Hook:** in `Logger.logMessage()`, wrap the assembled `log` string:
  `this.executeCallback(opts.logLevel, TokenScrubber.scrub(log), opts.containsPii ?? false);`
  Single choke point → covers every log call and every downstream `localCallback`.
- **Fast path:** `if (!log.includes("Artifacts-anchor")) return log;` before running the regex.
- **Boundary logic:** implement `scrub` with a `String.replace(anchorRegex, ...)` that, on a hit,
  expands over `[A-Za-z0-9+/_\-=]` in both directions (indexOf/scan) and substitutes the run.
- **Config / kill switch:** add an optional `LoggerOptions.disableTokenScrubbing` (default false);
  read in the `Logger` constructor. Keep independent of `piiLoggingEnabled`.
- **Tests:** Jest specs in `lib/msal-common/test/logger/` running the shared corpus; add a
  bundle-size note (msal-browser is size-sensitive — keep the scrubber tiny, no new deps).

### Python — `microsoft-authentication-library-for-python`

- **New file:** `msal/_scrubber.py` with `scrub(text: str) -> str` and a `TokenScrubbingFilter`
  (`logging.Filter` subclass).
- **Patterns:** module-level compiled `re.compile("|".join(PATTERNS))` (no `re.IGNORECASE`).
- **Registration (choke point):** because MSAL uses per-module `logging.getLogger(__name__)` under
  the `"msal"` namespace, attach the filter once to the parent logger:
  `logging.getLogger("msal").addFilter(TokenScrubbingFilter())` — do this lazily inside
  `ClientApplication.__init__` (idempotent guard) so it activates without app changes.
- **Interpolation caveat:** scrub `record.getMessage()` (post `%s`-interpolation) and then reset
  `record.msg = scrubbed; record.args = ()`, because tokens usually arrive as `%s` args, not in the
  literal template. Filters run before handler emission → single effective choke point.
- **Config / kill switch:** module-level flag or an `ClientApplication(..., enable_token_scrubbing=True)`
  kwarg (default True); keep independent of `enable_pii_log`.
- **Tests:** `tests/test_scrubber.py` (pytest/unittest) running the shared corpus and asserting a
  `caplog`/handler receives redacted text; verify ordinary debug lines are untouched.

### Java — `microsoft-authentication-library-for-java`

- **New file:** `msal4j-sdk/src/main/java/com/microsoft/aad/msal4j/TokenScrubber.java`
  (package-private) with `static String scrub(String)` + a precompiled `java.util.regex.Pattern`.
- **Two-part coverage (no central choke point in SLF4J):**
  1. **Call-site coverage:** route messages through `LogHelper.createMessage()` and scrub there —
     `return TokenScrubber.scrub(String.format("[Correlation ID: %s] %s", correlationId, msg));`
     Covers everything built via `LogHelper` (audit call sites to widen this).
  2. **Sink coverage (complete):** ship an optional turnkey wrapper appender for the common
     bindings — a Logback `AppenderBase<ILoggingEvent>` and/or Log4j2 rewrite policy — that scrubs
     `event.getFormattedMessage()`. Document how apps register it; this catches messages that don't
     pass through `LogHelper`.
- **Fast path:** `if (!msg.contains(anchor)) return msg;` before the regex.
- **Config / kill switch:** `logPii`-independent flag on the builder
  (e.g. `.tokenScrubbing(true)`), default ON for (1); (2) is opt-in by nature.
- **Tests:** JUnit in `msal4j-sdk/src/test/java/.../TokenScrubberTest.java` running the shared corpus;
  a Logback-appender integration test asserting redacted output at the sink.

### Cross-SDK parity harness

- One canonical JSON corpus (checked into each repo or a shared location): each case =
  `{ input, expectedOutput, note }`, covering 3 offsets × base64/base64url × eSTS/MSA, the real
  `esctx` cookie line, URL/query placement, JSON-escaped placement, truncated-tail, and
  negative cases (ordinary logs, look-alike strings). All four SDKs run it verbatim to guarantee
  identical redaction behavior.

## Gap Analysis — is the current pattern approach missing anything?

**Detection gaps**
1. **base64url + percent-encoding boundaries.** Core prefixes are all-alnum (safe), but the
   *boundary expansion* and any token in a URL query (`+`→`%2B`, `/`→`%2F`, `=`→`%3D`) must be
   handled or the redaction extent will be wrong.
2. **Only base64 form is caught.** Hex-encoded, JSON-escaped, or already-decoded token bytes
   won't match. Must be stated as an explicit limitation.
3. **MSA literal inconsistency.** If MSA tokens are themselves base64-encoded as a whole, the
   literal `MsaArtifacts` will appear base64-encoded (with its own 3 offsets), not literally.
   Need to confirm whether the scrubber searches ASCII `MsaArtifacts` or its base64 form(s).
4. **Truncated / chunked tokens.** Loggers truncate long strings; a token split across the
   truncation or across two log calls may not match. Document limitation; consider conservative
   redaction on a trailing partial-prefix match.

**Redaction-correctness gaps (biggest risk)**
5. **Redaction extent is undefined.** The pattern constants only detect *presence*. The design
   must specify start/end boundary rules; otherwise over-redaction (eating following text) or
   under-redaction (leaving part of the token) results.
6. **Tag is mid-token, not at the start.** Because format is `{header}{tag}{body}`, the match is
   *after* the header — boundary expansion must go **left** as well as right. This is the reason
   for the 3 offsets and is easy to get wrong.

**Engineering gaps**
7. **Performance / allocations.** Runs on every log line. Needs cheap pre-filter, compiled
   patterns, zero-alloc no-match path, and benchmarks (esp. .NET/JS hot paths).
8. **Case sensitivity.** Matching must be ordinal/case-sensitive — never lowercase.
9. **Coverage beyond the SDK logger.** Tokens still leak via app logging of `AccessToken`,
   HTTP wire traces, exception messages/telemetry, and the Go SDK (no logger). Scope must be
   explicit; recommend also scrubbing at the HTTP-trace layer.
10. **Go has no logging; Java/Python have no central choke point** — non-trivial to guarantee
    complete coverage; needs a per-SDK decision (filter/appender/HTTP wrapper).

**Process gaps**
11. **Rollout ordering / version skew.** Scrubber patterns must ship and enable only after
    server tagging is validated in prod (mirrors the server rollout). Old SDKs won't scrub, but
    tokens stay functional (tag is opaque extra bytes) — only the redaction benefit is missing.
12. **Generic-prefix guarantee.** The whole point is future token types are auto-covered; confirm
    the scrubber keys **only** off the generic prefix, never per-type strings.
13. **Cross-SDK parity via shared test vectors.** Currently absent; without a shared corpus the
    5 implementations will drift.
14. **Safe telemetry / kill switch / pattern updatability.** Count scrubs without leaking; be able
    to disable and (ideally) update patterns without a full SDK release.

## Implementation roadmap

High level: resolve the open questions (MSA form, base64url boundaries, redaction extent, coverage
scope) → finalize the shared spec → per-SDK implementation (.NET first — cleanest hook) → shared
cross-SDK test vectors → rollout/validation alignment with the server tagging rollout.

## Open questions for stakeholders
- Does the scrubber match ASCII `MsaArtifacts` or its base64 form(s)? (Gap #3)
- Redaction placeholder: fixed string, or decode CallerId to include token type?
- Coverage: SDK logger only, or also HTTP-trace layer + Go HTTP wrapper?
- On-by-default independent of the PII flag, with a kill switch — confirm.

## Recommended defaults (pending stakeholder confirmation)
These are the proposed answers so implementation is unblocked; flag any you disagree with.
1. **MSA form:** search for **both** the ASCII literal `MsaArtifacts` **and** its base64 offset
   forms. Rationale: MSA tokens can appear either raw (cookie/header context) or base64-embedded;
   covering both is cheap and avoids a detection hole. Confirm with the MSA token owners.
2. **Placeholder:** default to a fixed `[Redacted opaque token]` for v1 (simplest, safest).
   Decode the CallerId to emit `[Redacted <TokenType> token]` as a fast-follow once the CallerId
   byte layout is verified against `TransmissionDataCaller`.
3. **Coverage:** v1 = each SDK's own log choke point (highest ROI, catches the `esctx`/error-blob
   case). HTTP-trace-layer scrubbing tracked as a follow-up; Go remains out of scope (no logger).
4. **On-by-default:** yes — always on, **independent of the PII flag** (defense in depth), with a
   documented kill switch per SDK for perf/debugging.

## Rollout & validation alignment (client side)
- Ship the scrubber code **disabled or pattern-empty** first, enable patterns only **after** the
  server tagging is validated in prod (mirrors the server rollout in the attachment).
- Because the scrubber keys **only** off the generic `EvoStsArtifacts`/`MsaArtifacts` prefix, new
  opaque token types added server-side are auto-covered with no client change — validate this
  invariant with a test that adds a synthetic new token type and confirms redaction.
- Field validation: the safe `tokensScrubbed` counter (by token type where available) confirms the
  scrubber is firing in production without ever emitting token values.
