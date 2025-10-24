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
    /// This interface extends <see cref="IAuthorizationHeaderProvider"/> to provide methods that return
    /// authorization header information along with bound certificate details.
    /// </summary>
    public interface IAuthorizationHeaderBoundProvider : IAuthorizationHeaderProvider
    {
        /// <summary>
        /// Creates the authorization header used to call a protected web API on behalf of a user.
        /// </summary>
        /// <param name="scopes">Scopes for which to request the authorization header.</param>
        /// <param name="authorizationHeaderProviderOptions">Information about the API that will be called (for some
        /// protocols like Pop), and token acquisition options.</param>
        /// <param name="claimsPrincipal">Inbound authentication elements. In a web API, this is usually the result of the
        /// validation of a token. In a web app, this would be information about the signed-in user. This is not useful in
        /// daemon applications. In Microsoft.Identity.Web you rarely need to provide this parameter as it's inferred from the
        /// context.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A result which contains authorization header with protocol and token, e.g., "Bearer _token_", as well
        /// as certificate which is bound to the token (if any).
        /// </returns>
        Task<AuthorizationHeaderInformation> CreateAuthorizationHeaderBoundForUserAsync(
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
        /// <returns>A result which contains authorization header with protocol and token, e.g., "Bearer _token_", as well
        /// as certificate which is bound to the token (if any).
        /// </returns>
        Task<AuthorizationHeaderInformation> CreateAuthorizationHeaderBoundForAppAsync(
            string scopes,
            AuthorizationHeaderProviderOptions? downstreamApiOptions = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates an authorization header for calling a protected web API on behalf of a user or the application.
        /// </summary>
        /// <param name="scopes">The scopes for which to request the authorization header.
        /// Provide a single scope if the header needs to be created on behalf of an application.</param>
        /// <param name="options">The <see cref="AuthorizationHeaderProviderOptions"/> containing information about the API
        /// to be called and token acquisition settings. If not provided, the header will be for a bearer token.</param>
        /// <param name="claimsPrincipal">Inbound authentication elements. In a web API, this is usually the result of the
        /// validation of a token. In a web app, this would be information about the signed-in user. This is not useful in
        /// daemon applications. In Microsoft.Identity.Web you rarely need to provide this parameter as it's inferred from the
        /// context.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A result which contains authorization header with protocol and token, e.g., "Bearer _token_", as well
        /// as certificate which is bound to the token (if any).
        /// </returns>
        Task<AuthorizationHeaderInformation> CreateAuthorizationHeaderBoundAsync(
            IEnumerable<string> scopes,
            AuthorizationHeaderProviderOptions? options = null,
            ClaimsPrincipal? claimsPrincipal = null,
            CancellationToken cancellationToken = default);
    }
}