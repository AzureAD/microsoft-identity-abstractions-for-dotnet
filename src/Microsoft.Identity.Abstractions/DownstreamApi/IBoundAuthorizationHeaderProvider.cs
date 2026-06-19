// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Creates an authorization header value that the caller can use to access a protected web API, which supports either unbound or
    /// bound to a certificate (for example, in an mTLS PoP scenario) tokens.
    /// </summary>
    public interface IBoundAuthorizationHeaderProvider
    {
         /// <summary>
        /// Creates the authorization header used to call a protected web API with either unbound or bound to certificate tokens.
        /// </summary>
        /// <param name="downstreamApiOptions">Information about the API that will be called and token acquisition options.</param>
        /// <param name="claimsPrincipal">Inbound authentication elements. In a web API, this is usually the result of the
        /// validation of a token. In a web app, this would be information about the signed-in user. This is not useful in
        /// daemon applications. In Microsoft.Identity.Web you rarely need to provide this parameter as it's inferred from the
        /// context.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A result which contains authorization token with optional bound certificate</returns>
        Task<OperationResult<AuthorizationHeaderInformation, AuthorizationHeaderError>> CreateBoundAuthorizationHeaderAsync(
            DownstreamApiOptions downstreamApiOptions,
            ClaimsPrincipal? claimsPrincipal = null,
            CancellationToken cancellationToken = default);
    }
}
