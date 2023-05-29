// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Creates the value of an authorization header that the caller can use to call a protected web API.
    /// </summary>
    public interface IAuthorizationHeaderProvider
    {
        /// <summary>
        /// Creates the authorization header used to call a protected web API on behalf of a user.
        /// </summary>
        /// <param name="scopes">Scopes for which to request the authorization header.</param>
        /// <param name="authorizationHeaderProviderOptions">Information about the API that will be called (for some
        /// protocols like Pop), and token acquisition options.</param>
        /// <param name="claimsPrincipal">Inbound authentication elements. In a web API, this is usually the result of the
        /// validation of a token. In a web app, this would be information about the signed-in user. This is not useful in
        /// daemon applications. In Microsoft.Identity.Web you rarely need to provide this parameter as it's infered from the
        /// context.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A string containing the authorization header, that is protocol and tokens
        /// (for instance: "Bearer token", "PoP token", etc ...).
        /// </returns>
        Task<string> CreateAuthorizationHeaderForUserAsync(
            IEnumerable<string> scopes, 
            AuthorizationHeaderProviderOptions? authorizationHeaderProviderOptions = null,
            ClaimsPrincipal? claimsPrincipal = default,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates the authorization header used to call a protected web API on behalf
        /// of the application itself.
        /// </summary>
        /// <param name="scopes">Scopes for which to request the authorization header.</param>
        /// <param name="downstreamApiOptions">Information about the API that will be called (for some
        /// protocols like Pop), and token acquisition options. Optional. If not provided, the
        /// authentication header will be for a bearer token.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A string containing the authorization header, that is protocol and tokens
        /// (for instance: "Bearer token", "PoP token", etc ...).
        /// </returns>
        Task<string> CreateAuthorizationHeaderForAppAsync(
            string scopes,
            AuthorizationHeaderProviderOptions? downstreamApiOptions = null,
            CancellationToken cancellationToken = default);
    }
}
