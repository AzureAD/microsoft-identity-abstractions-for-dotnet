// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Reason a token-acquisition attempt refreshed the cached access token.
    /// </summary>
    public enum AcquiredTokenCacheRefreshReason
    {
        /// <summary>
        /// No refresh occurred. The cached token satisfied the request.
        /// </summary>
        NotApplicable = 0,

        /// <summary>
        /// Refresh was requested explicitly (for example a force-refresh flag) or driven by a claims challenge.
        /// </summary>
        ForceRefreshOrClaims = 1,

        /// <summary>
        /// No matching token was found in the cache.
        /// </summary>
        NoCachedAccessToken = 2,

        /// <summary>
        /// The cached token had expired.
        /// </summary>
        Expired = 3,

        /// <summary>
        /// The token was refreshed proactively before its expiration to smooth out load.
        /// </summary>
        ProactivelyRefreshed = 4,

        /// <summary>
        /// The token cache was disabled, so the cache was not consulted.
        /// </summary>
        CacheDisabled = 5
    }
}
