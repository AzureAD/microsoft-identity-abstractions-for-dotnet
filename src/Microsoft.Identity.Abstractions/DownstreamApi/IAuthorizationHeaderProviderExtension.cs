// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Provides a method to create an authorization header for calling protected web APIs 
    /// on behalf of a user or the application itself.
    /// </summary>
    public interface IAuthorizationHeaderProviderExtension
    {
        /// <summary>
        /// Creates an authorization header for calling a protected web API.
        /// </summary>
        /// <param name="requestContext">Information about the incoming request.</param>
        /// <param name="scopes">The scopes for which to request the authorization header.</param>
        /// <param name="options">Options containing information about the API to be called and token acquisition settings.</param>
        /// <param name="subject">The claims principal representing the authenticated user or entity making the request.
        /// Typically provided in a web API or web app context; not usually needed in daemon applications.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A string containing the authorization header, such as "Bearer token" or "PoP token".</returns>
        Task<string> CreateAuthorizationHeaderAsync(
            RequestContext requestContext,
            IEnumerable<string> scopes,
            AuthorizationHeaderProviderOptions? options = null,
            ClaimsPrincipal? subject = null,
            CancellationToken cancellationToken = default);
    }
}
