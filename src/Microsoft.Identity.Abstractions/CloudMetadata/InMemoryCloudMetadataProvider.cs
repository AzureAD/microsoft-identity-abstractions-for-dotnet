// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// A ready-made, mutable <see cref="ICloudMetadataProvider"/> that a caller populates at startup, with an
    /// optional fallback provider for hosts it does not itself know about.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is the type customers instantiate directly when they need to inject a new cloud, or adjust an
    /// existing one, without implementing <see cref="ICloudMetadataProvider"/> by hand.
    /// </para>
    /// <para>
    /// Like the rest of the identity SDK configuration surface, this provider follows a
    /// <b>configure-then-read</b> model: populate it via <see cref="AddOrUpdate(string, IReadOnlyDictionary{string, string})"/>
    /// while composing your app (typically before registering it as a singleton), then treat it as read-only.
    /// The <c>AddOrUpdate</c> mutators are not safe to call concurrently with reads or with each other;
    /// <see cref="GetByAuthorityHost(string)"/> is safe for concurrent reads once population is complete.
    /// </para>
    /// <para>
    /// Resolution layers the registered values over the optional fallback provider <b>per key</b>: a key
    /// registered here wins, a key not registered here falls back to the same host in the fallback, and a host
    /// unknown to both resolves to <c>null</c>. This per-key merge lets a caller override a single value for a
    /// cloud (for example one cloud's audience) while inheriting the rest of that cloud's metadata from the
    /// fallback, instead of re-declaring every key.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // Inject a brand-new cloud (or adjust an existing one — same call, this provider wins per key):
    /// ICloudMetadataProvider provider = new InMemoryCloudMetadataProvider()
    ///     .AddOrUpdate("login.mynewcloud.example", new Dictionary&lt;string, string&gt;
    ///     {
    ///         [CloudMetadataKeyNames.FederatedCredentialAudience] = "api://AzureADTokenExchangeMyCloud",
    ///     });
    ///
    /// services.AddSingleton&lt;ICloudMetadataProvider&gt;(provider);
    /// </code>
    /// </example>
    public sealed class InMemoryCloudMetadataProvider : ICloudMetadataProvider
    {
        private readonly Dictionary<string, Dictionary<string, string>> _entries =
            new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);

        private readonly ICloudMetadataProvider? _fallback;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryCloudMetadataProvider"/> class.
        /// </summary>
        /// <param name="fallback">An optional provider consulted, per key, for values this instance does not
        /// have its own entry for. When <c>null</c>, hosts not registered here resolve to <c>null</c>.</param>
        public InMemoryCloudMetadataProvider(ICloudMetadataProvider? fallback = null)
        {
            _fallback = fallback;
        }

        /// <summary>
        /// Adds or updates the metadata for the cloud identified by <paramref name="authorityHost"/>, merged
        /// <b>per key</b> over any values already registered for that host (and, at resolution time, over the
        /// fallback). Keys in <paramref name="values"/> win; keys not supplied are left untouched.
        /// </summary>
        /// <param name="authorityHost">The authority host (for example <c>login.microsoftonline.us</c>).</param>
        /// <param name="values">The cloud-specific key/value pairs. Keys should come from
        /// <see cref="CloudMetadataKeyNames"/> (or an SDK-specific extension of that vocabulary).</param>
        /// <returns>This same instance, to allow chaining multiple <see cref="AddOrUpdate(string, IReadOnlyDictionary{string, string})"/> calls.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="authorityHost"/> is null or whitespace,
        /// or when any key in <paramref name="values"/> is null or whitespace.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <c>null</c>, or when
        /// any value in <paramref name="values"/> is <c>null</c>.</exception>
        public InMemoryCloudMetadataProvider AddOrUpdate(string authorityHost, IReadOnlyDictionary<string, string> values)
        {
            if (string.IsNullOrWhiteSpace(authorityHost))
            {
                throw new ArgumentException("Authority host cannot be null or whitespace.", nameof(authorityHost));
            }

#if NET8_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(values);
#else
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }
#endif

            // Validate every pair up front so this bulk overload cannot store null values or whitespace keys
            // that the per-key AddOrUpdate(host, key, value) overload rejects — the two must behave consistently
            // and the public contract is a non-null string-to-string map.
            foreach (KeyValuePair<string, string> pair in values)
            {
                if (string.IsNullOrWhiteSpace(pair.Key))
                {
                    throw new ArgumentException("Keys cannot be null or whitespace.", nameof(values));
                }

                if (pair.Value is null)
                {
                    throw new ArgumentNullException(nameof(values), $"Value for key '{pair.Key}' cannot be null.");
                }
            }

            Dictionary<string, string> bag = GetOrAddBag(authorityHost);
            foreach (KeyValuePair<string, string> pair in values)
            {
                bag[pair.Key] = pair.Value;
            }

            return this;
        }

        /// <summary>
        /// Adds or updates a <b>single</b> cloud-specific value for an authority host, merged per key over any
        /// values already registered for that host (and, at resolution time, over the fallback). Use this to
        /// adjust or add one value for a cloud while leaving its other values as-is.
        /// </summary>
        /// <param name="authorityHost">The authority host (for example <c>login.microsoftonline.us</c>).</param>
        /// <param name="key">The value's key, typically one of the <see cref="CloudMetadataKeyNames"/> literals.</param>
        /// <param name="value">The value to set.</param>
        /// <returns>This same instance, to allow chaining.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="authorityHost"/> or
        /// <paramref name="key"/> is null or whitespace.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c>.</exception>
        public InMemoryCloudMetadataProvider AddOrUpdate(string authorityHost, string key, string value)
        {
            if (string.IsNullOrWhiteSpace(authorityHost))
            {
                throw new ArgumentException("Authority host cannot be null or whitespace.", nameof(authorityHost));
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Key cannot be null or whitespace.", nameof(key));
            }

#if NET8_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(value);
#else
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }
#endif

            GetOrAddBag(authorityHost)[key] = value;
            return this;
        }

        /// <inheritdoc/>
        public IReadOnlyDictionary<string, string>? GetByAuthorityHost(string authorityHost)
        {
            if (string.IsNullOrEmpty(authorityHost))
            {
                return null;
            }

            IReadOnlyDictionary<string, string>? fallbackMetadata = _fallback?.GetByAuthorityHost(authorityHost);

            if (!_entries.TryGetValue(authorityHost, out Dictionary<string, string>? bag))
            {
                return fallbackMetadata;
            }

            // Layer this instance's values (higher) over the fallback (lower), per key, into a read-only bag.
            return Merge(fallbackMetadata, bag);
        }

        private Dictionary<string, string> GetOrAddBag(string authorityHost)
        {
            if (!_entries.TryGetValue(authorityHost, out Dictionary<string, string>? bag))
            {
                bag = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                _entries[authorityHost] = bag;
            }

            return bag;
        }

        // Merges two per-host bags into a new read-only, case-insensitive dictionary. Values in
        // <paramref name="higher"/> win per key; <paramref name="lower"/> supplies any keys the higher layer
        // does not set. <paramref name="lower"/> may be null (no fallback contribution).
        private static ReadOnlyDictionary<string, string> Merge(
            IReadOnlyDictionary<string, string>? lower,
            IReadOnlyDictionary<string, string> higher)
        {
            var merged = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            if (lower is not null)
            {
                foreach (KeyValuePair<string, string> pair in lower)
                {
                    merged[pair.Key] = pair.Value;
                }
            }

            foreach (KeyValuePair<string, string> pair in higher)
            {
                merged[pair.Key] = pair.Value;
            }

            return new ReadOnlyDictionary<string, string>(merged);
        }
    }
}
