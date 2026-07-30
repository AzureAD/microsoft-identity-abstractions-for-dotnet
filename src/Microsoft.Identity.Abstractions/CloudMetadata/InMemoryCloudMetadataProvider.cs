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
    /// existing one, without implementing <see cref="ICloudMetadataProvider"/> by hand. Entries added here
    /// win over the fallback provider for the same host.
    /// </remarks>
    /// <example>
    /// <code>
    /// // Inject a brand-new cloud (or adjust an existing one — same call, this provider wins per host):
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
        private readonly ConcurrentDictionary<string, CloudMetadata> _entries =
            new ConcurrentDictionary<string, CloudMetadata>(StringComparer.OrdinalIgnoreCase);

        private readonly ICloudMetadataProvider? _fallback;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryCloudMetadataProvider"/> class.
        /// </summary>
        /// <param name="fallback">An optional provider consulted for any authority host this instance does
        /// not have its own entry for. When <c>null</c>, unknown hosts resolve to <c>null</c>.</param>
        public InMemoryCloudMetadataProvider(ICloudMetadataProvider? fallback = null)
        {
            _fallback = fallback;
        }

        /// <summary>
        /// Adds or replaces the metadata for the cloud identified by <paramref name="authorityHost"/>.
        /// </summary>
        /// <param name="authorityHost">The authority host (for example <c>login.microsoftonline.us</c>).</param>
        /// <param name="values">The cloud-specific key/value pairs. Keys should come from
        /// <see cref="AbstractionsCloudKeys"/> (or an SDK-specific extension of that vocabulary).</param>
        /// <returns>This same instance, to allow chaining multiple <see cref="AddOrUpdate"/> calls.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="authorityHost"/> or
        /// <paramref name="values"/> is <c>null</c>.</exception>
        public InMemoryCloudMetadataProvider AddOrUpdate(string authorityHost, IReadOnlyDictionary<string, string> values)
        {
#if NET8_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(authorityHost);
            ArgumentNullException.ThrowIfNull(values);
#else
            if (authorityHost is null)
            {
                throw new ArgumentNullException(nameof(authorityHost));
            }

            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }
#endif

            _entries[authorityHost] = new CloudMetadata(values);
            return this;
        }

        /// <inheritdoc/>
        public CloudMetadata? GetByAuthorityHost(string authorityHost)
        {
            if (!string.IsNullOrEmpty(authorityHost) && _entries.TryGetValue(authorityHost, out CloudMetadata? metadata))
            {
                return metadata;
            }

            return _fallback?.GetByAuthorityHost(authorityHost);
        }
    }
}
