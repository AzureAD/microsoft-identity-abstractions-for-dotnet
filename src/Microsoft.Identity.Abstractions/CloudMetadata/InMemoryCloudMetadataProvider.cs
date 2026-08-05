// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// A ready-made, thread-safe, mutable <see cref="ICloudMetadataProvider"/> that a caller can populate at
    /// runtime, with an optional fallback provider for hosts it does not itself know about.
    /// </summary>
    /// <remarks>
    /// This is the type customers instantiate directly when they need to inject a new cloud, or adjust an
    /// existing one, without implementing <see cref="ICloudMetadataProvider"/> by hand. Resolution layers
    /// the registered values over the optional fallback provider <b>per key</b>: a key registered here wins,
    /// a key not registered here falls back to the same host in the fallback, and a host unknown to both
    /// resolves to <c>null</c>.
    /// </remarks>
    /// <example>
    /// <code>
    /// // Inject a brand-new cloud (or adjust an existing one — same call, this provider wins per key):
    /// ICloudMetadataProvider provider = new InMemoryCloudMetadataProvider()
    ///     .AddOrUpdate("login.mynewcloud.example", new Dictionary&lt;string, string&gt;
    ///     {
    ///         [AbstractionsCloudKeys.TokenExchangeAudience] = "api://AzureADTokenExchangeMyCloud",
    ///     });
    ///
    /// services.AddSingleton&lt;ICloudMetadataProvider&gt;(provider);
    /// </code>
    /// </example>
    public sealed class InMemoryCloudMetadataProvider : ICloudMetadataProvider
    {
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, string>> _entries =
            new ConcurrentDictionary<string, ConcurrentDictionary<string, string>>(StringComparer.OrdinalIgnoreCase);

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
        /// <see cref="AbstractionsCloudKeys"/> (or an SDK-specific extension of that vocabulary).</param>
        /// <returns>This same instance, to allow chaining multiple <see cref="AddOrUpdate(string, IReadOnlyDictionary{string, string})"/> calls.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="authorityHost"/> is null or whitespace.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <c>null</c>.</exception>
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

            ConcurrentDictionary<string, string> bag = GetOrAddBag(authorityHost);
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
        /// <param name="key">The value's key, typically one of the <see cref="AbstractionsCloudKeys"/> literals.</param>
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
        public CloudMetadata? GetByAuthorityHost(string authorityHost)
        {
            if (string.IsNullOrEmpty(authorityHost))
            {
                return null;
            }

            CloudMetadata? fallbackMetadata = _fallback?.GetByAuthorityHost(authorityHost);

            if (!_entries.TryGetValue(authorityHost, out ConcurrentDictionary<string, string>? bag))
            {
                return fallbackMetadata;
            }

            return CloudMetadata.Merge(fallbackMetadata, new CloudMetadata(bag));
        }

        private ConcurrentDictionary<string, string> GetOrAddBag(string authorityHost)
        {
            return _entries.GetOrAdd(
                authorityHost,
                _ => new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase));
        }
    }
}
