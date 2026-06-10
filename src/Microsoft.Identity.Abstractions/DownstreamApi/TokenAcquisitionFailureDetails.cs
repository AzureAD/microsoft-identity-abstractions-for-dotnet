// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Diagnostic details about a failed token-acquisition attempt, mirroring fields
    /// exposed by the underlying token-service exception.
    /// </summary>
    /// <remarks>
    /// All members are diagnostic only. Do not program against specific values of <see cref="SubError"/>
    /// or <see cref="EstsErrorCode"/>; they are set by the token service and may change without notice.
    /// </remarks>
    public sealed class TokenAcquisitionFailureDetails
    {
        /// <summary>
        /// Token-service error code (for example an OAuth error code or an ESTS error code).
        /// </summary>
        public string? EstsErrorCode { get; init; }

        /// <summary>
        /// Server-emitted sub-error code refining <see cref="EstsErrorCode"/> (for example
        /// <c>consent_required</c>, <c>bad_token</c>, <c>protection_policy_required</c>);
        /// diagnostic only.
        /// </summary>
        public string? SubError { get; init; }

        /// <summary>
        /// HTTP status code returned by the token endpoint, when available.
        /// </summary>
        public int? StatusCode { get; init; }

        /// <summary>
        /// Claims challenge returned by the token service, when available.
        /// </summary>
        public string? Claims { get; init; }

        /// <summary>
        /// Correlation id associated with the failing request.
        /// </summary>
        public string? CorrelationId { get; init; }
    }
}
