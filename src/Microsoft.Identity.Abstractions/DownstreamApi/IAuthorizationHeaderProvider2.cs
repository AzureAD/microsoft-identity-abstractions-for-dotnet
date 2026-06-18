// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Extends <see cref="IAuthorizationHeaderProvider"/> with request-aware authorization header creation.
    /// </summary>
    /// <remarks>
    /// Some protocols bind the authorization header to the outgoing HTTP request. For example, a PoP
    /// SignedHttpRequest can include the <c>q</c> (query), <c>h</c> (headers), and <c>b</c> (body) claims, which
    /// hash parts of the request actually sent. Those parts are only known once the <see cref="HttpRequestMessage"/>
    /// is materialized, so this interface passes the request to the provider. Protocols that don't bind request
    /// material behave identically to <see cref="IAuthorizationHeaderProvider.CreateAuthorizationHeaderAsync"/>.
    /// </remarks>
    public interface IAuthorizationHeaderProvider2 : IAuthorizationHeaderProvider
    {
        /// <summary>
        /// Creates an authorization header for calling a protected web API, binding the header to the supplied
        /// outgoing <see cref="HttpRequestMessage"/>.
        /// </summary>
        /// <param name="scopes">The scopes for which to request the authorization header. Provide a single scope
        /// if the header needs to be created on behalf of an application.</param>
        /// <param name="httpRequestMessage">The outgoing HTTP request the authorization header will be added to.
        /// Depending on the protocol, its method, URI, headers, and content may be bound into the header. The
        /// request's content should already be set when this method is called so that body binding is accurate.</param>
        /// <param name="options">The <see cref="AuthorizationHeaderProviderOptions"/> containing information about
        /// the API to be called and token acquisition settings. If not provided, the header will be for a bearer token.</param>
        /// <param name="claimsPrincipal">Inbound authentication elements. In a web API, this is usually the result of
        /// the validation of a token. In a web app, this would be information about the signed-in user. This is not
        /// useful in daemon applications. In Microsoft.Identity.Web you rarely need to provide this parameter as it's
        /// inferred from the context.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A string containing the authorization header, such as "Bearer token" or "PoP token".</returns>
        Task<string> CreateAuthorizationHeaderAsync(
            IEnumerable<string> scopes,
            HttpRequestMessage httpRequestMessage,
            AuthorizationHeaderProviderOptions? options = null,
            ClaimsPrincipal? claimsPrincipal = null,
            CancellationToken cancellationToken = default);
    }
}
