// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Metadata describing how an access token was acquired: timings, cache state,
    /// region routing, and any additional response parameters returned by the token endpoint.
    /// </summary>
    public sealed class TokenAcquisitionMetadata
    {
        /// <summary>
        /// Source the token was obtained from.
        /// </summary>
        public AcquiredTokenSource TokenSource { get; init; }

        /// <summary>
        /// Reason the cache was refreshed during this attempt, if any.
        /// </summary>
        public AcquiredTokenCacheRefreshReason CacheRefreshReason { get; init; }

        /// <summary>
        /// Cache tier that served the token.
        /// </summary>
        public AcquiredTokenCacheLevel CacheLevel { get; init; }

        /// <summary>
        /// Token endpoint contacted, or <see langword="null"/> if the token was served from the cache.
        /// </summary>
        public string? TokenEndpoint { get; init; }

        /// <summary>
        /// Total wall-clock duration of the token-acquisition attempt, in milliseconds.
        /// </summary>
        public long DurationTotalInMs { get; init; }

        /// <summary>
        /// Time spent in HTTP calls to the token endpoint, in milliseconds.
        /// </summary>
        public long DurationInHttpInMs { get; init; }

        /// <summary>
        /// Time spent reading and writing the token cache, in milliseconds.
        /// </summary>
        public long DurationInCacheInMs { get; init; }

        /// <summary>
        /// Point in time at which the access token expires, or <see langword="null"/> when not captured.
        /// Mirrors <see cref="AcquireTokenResult.ExpiresOn"/>.
        /// </summary>
        public DateTimeOffset? ExpiresOn { get; init; }

        /// <summary>
        /// Point in time at which the token should be proactively refreshed,
        /// or <see langword="null"/> when no proactive refresh hint was provided.
        /// </summary>
        public DateTimeOffset? RefreshOn { get; init; }

        /// <summary>
        /// Regional ESTS routing details, or <see langword="null"/> when regional routing was not used.
        /// </summary>
        public AcquiredTokenRegionDetails? RegionDetails { get; init; }
    }
}
