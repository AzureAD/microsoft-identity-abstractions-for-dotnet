// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Source from which an access token was obtained.
    /// </summary>
    public enum AcquiredTokenSource
    {
        /// <summary>
        /// Token was obtained directly from the identity provider (an STS token endpoint).
        /// </summary>
        IdentityProvider = 0,

        /// <summary>
        /// Token was served from the token cache (in-memory L1 or distributed L2).
        /// </summary>
        Cache = 1,

        /// <summary>
        /// Token was obtained from a broker (for example WAM on Windows).
        /// </summary>
        Broker = 2
    }
}
