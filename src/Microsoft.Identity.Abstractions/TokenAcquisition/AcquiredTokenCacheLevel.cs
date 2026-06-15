// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Cache tier that served the access token.
    /// </summary>
    public enum AcquiredTokenCacheLevel
    {
        /// <summary>
        /// Token was retrieved from the identity provider rather than from a cache tier.
        /// This includes cache misses (entry absent or expired), forced or proactive refresh,
        /// and cases where the cache was bypassed.
        /// </summary>
        None = 0,

        /// <summary>
        /// The cache tier is not known.
        /// </summary>
        Unknown = 1,

        /// <summary>
        /// Token served from the in-memory (L1) cache.
        /// </summary>
        L1Cache = 2,

        /// <summary>
        /// Token served from the distributed (L2) cache.
        /// </summary>
        L2Cache = 3
    }
}
