// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Diagnostic details about a failed token-acquisition attempt, mirroring fields
    /// exposed by the underlying token-service exception.
    /// </summary>
    /// <remarks>
    /// All members are diagnostic only. Do not program against specific values of <see cref="SubError"/>,
    /// <see cref="ErrorCode"/>, or <see cref="ServiceErrorCodes"/>; they are set by the token service and
    /// may change without notice.
    /// </remarks>
    public sealed class TokenAcquisitionFailureDetails
    {
        /// <summary>
        /// Token-service error code (for example an OAuth error code or an ESTS error code).
        /// Mirrors <c>MsalServiceException.ErrorCode</c> when MSAL is the underlying acquirer.
        /// </summary>
        public string? ErrorCode { get; init; }

        /// <summary>
        /// Server-emitted sub-error code refining <see cref="ErrorCode"/> (for example
        /// <c>consent_required</c>, <c>bad_token</c>, <c>protection_policy_required</c>);
        /// diagnostic only.
        /// </summary>
        public string? SubError { get; init; }

        /// <summary>
        /// The raw STS-specific error codes returned by the token service (for example the numeric
        /// <c>AADSTS</c> codes) that further refine <see cref="ErrorCode"/>. Mirrors
        /// <c>MsalServiceException.ErrorCodesForLogging</c> when MSAL is the underlying acquirer; diagnostic only.
        /// </summary>
        public IReadOnlyList<string>? ServiceErrorCodes { get; init; }

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
