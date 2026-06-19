// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Outcome of regional ESTS routing for a token-acquisition attempt.
    /// </summary>
    public enum AcquiredTokenRegionOutcome
    {
        /// <summary>
        /// Regional routing was not requested.
        /// </summary>
        None = 0,

        /// <summary>
        /// A user-supplied region was used and matched the autodetected region.
        /// </summary>
        UserProvidedValid = 1,

        /// <summary>
        /// A user-supplied region was used; autodetection could not verify it.
        /// </summary>
        UserProvidedAutodetectionFailed = 2,

        /// <summary>
        /// A user-supplied region was used but disagreed with the autodetected region.
        /// </summary>
        UserProvidedInvalid = 3,

        /// <summary>
        /// The region was discovered through autodetection.
        /// </summary>
        AutodetectSuccess = 4,

        /// <summary>
        /// Autodetection failed and the request was sent to the global ESTS.
        /// </summary>
        FallbackToGlobal = 5
    }
}
