// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Information about regional ESTS routing for a token-acquisition attempt.
    /// </summary>
    public sealed class AcquiredTokenRegionDetails
    {
        /// <summary>
        /// Region that was used for the request, or <see langword="null"/> if no region was used.
        /// </summary>
        public string? RegionUsed { get; init; }

        /// <summary>
        /// How the value of <see cref="RegionUsed"/> was obtained.
        /// </summary>
        public AcquiredTokenRegionOutcome RegionOutcome { get; init; }

        /// <summary>
        /// Diagnostic message populated when region autodetection failed; <see langword="null"/> otherwise.
        /// </summary>
        public string? AutoDetectionError { get; init; }
    }
}
