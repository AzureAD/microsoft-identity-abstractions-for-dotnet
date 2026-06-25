// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Extends <see cref="IAuthorizationHeaderProvider"/> with metadata-carrying counterparts of its three
    /// header-creation methods. Each returns the full <see cref="AuthorizationHeaderInformation"/> (header
    /// value, binding certificate, and token-acquisition metadata) as an
    /// <see cref="OperationResult{TResult, TError}"/> instead of a bare header string.
    /// </summary>
    /// <remarks>
    /// Follows the additive numeric-suffix interface-evolution convention: the new interface extends the
    /// existing one so a single implementation can serve both, and existing string-returning callers are
    /// unaffected. The user, application, and unified entry points mirror the inputs of
    /// <see cref="IAuthorizationHeaderProvider.CreateAuthorizationHeaderForUserAsync"/>,
    /// <see cref="IAuthorizationHeaderProvider.CreateAuthorizationHeaderForAppAsync"/>, and
    /// <see cref="IAuthorizationHeaderProvider.CreateAuthorizationHeaderAsync"/> respectively; when the
    /// selected protocol is bound (for example mTLS Proof-of-Possession) the binding certificate is
    /// populated on the returned <see cref="AuthorizationHeaderInformation.BindingCertificate"/>.
    /// </remarks>
    public interface IAuthorizationHeaderProvider2 : IAuthorizationHeaderProvider
    {
        /// <summary>
        /// Rich-result counterpart of <see cref="IAuthorizationHeaderProvider.CreateAuthorizationHeaderForUserAsync"/>:
        /// creates the authorization header used to call a protected web API on behalf of a user and returns
        /// the full <see cref="AuthorizationHeaderInformation"/> on success, with metadata and (when the
        /// protocol is bound) the binding certificate populated. On failure the
        /// <see cref="AuthorizationHeaderError"/> carries the token-acquisition metadata and structured
        /// failure details when available.
        /// </summary>
        /// <param name="scopes">Scopes for which to request the authorization header.</param>
        /// <param name="authorizationHeaderProviderOptions">Information about the API that will be called (for
        /// some protocols like PoP), and token acquisition options.</param>
        /// <param name="claimsPrincipal">Inbound authentication elements. In a web API, this is usually the
        /// result of the validation of a token. In a web app, this would be information about the signed-in
        /// user. This is not useful in daemon applications. In Microsoft.Identity.Web you rarely need to
        /// provide this parameter as it's inferred from the context.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>An <see cref="OperationResult{TResult, TError}"/> containing the
        /// <see cref="AuthorizationHeaderInformation"/> on success, or an <see cref="AuthorizationHeaderError"/>
        /// on failure.</returns>
        Task<OperationResult<AuthorizationHeaderInformation, AuthorizationHeaderError>> CreateAuthorizationHeaderInformationForUserAsync(
            IEnumerable<string> scopes,
            AuthorizationHeaderProviderOptions? authorizationHeaderProviderOptions = null,
            ClaimsPrincipal? claimsPrincipal = default,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Rich-result counterpart of <see cref="IAuthorizationHeaderProvider.CreateAuthorizationHeaderForAppAsync"/>:
        /// creates the authorization header used to call a protected web API on behalf of the application
        /// itself and returns the full <see cref="AuthorizationHeaderInformation"/> on success, with metadata
        /// and (when the protocol is bound) the binding certificate populated. On failure the
        /// <see cref="AuthorizationHeaderError"/> carries the token-acquisition metadata and structured
        /// failure details when available.
        /// </summary>
        /// <param name="scopes">Scopes for which to request the authorization header.</param>
        /// <param name="downstreamApiOptions">Information about the API that will be called (for some
        /// protocols like PoP), and token acquisition options. Optional. If not provided, the authorization
        /// header will be for a bearer token.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>An <see cref="OperationResult{TResult, TError}"/> containing the
        /// <see cref="AuthorizationHeaderInformation"/> on success, or an <see cref="AuthorizationHeaderError"/>
        /// on failure.</returns>
        Task<OperationResult<AuthorizationHeaderInformation, AuthorizationHeaderError>> CreateAuthorizationHeaderInformationForAppAsync(
            string scopes,
            AuthorizationHeaderProviderOptions? downstreamApiOptions = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Rich-result counterpart of <see cref="IAuthorizationHeaderProvider.CreateAuthorizationHeaderAsync"/>:
        /// creates an authorization header for calling a protected web API on behalf of a user or the
        /// application and returns the full <see cref="AuthorizationHeaderInformation"/> on success, with
        /// metadata and (when the protocol is bound) the binding certificate populated. On failure the
        /// <see cref="AuthorizationHeaderError"/> carries the token-acquisition metadata and structured
        /// failure details when available.
        /// </summary>
        /// <param name="scopes">The scopes for which to request the authorization header. Provide a single
        /// scope if the header needs to be created on behalf of an application.</param>
        /// <param name="options">The <see cref="AuthorizationHeaderProviderOptions"/> containing information
        /// about the API to be called and token acquisition settings. If not provided, the header will be for
        /// a bearer token.</param>
        /// <param name="claimsPrincipal">Inbound authentication elements. In a web API, this is usually the
        /// result of the validation of a token. In a web app, this would be information about the signed-in
        /// user. This is not useful in daemon applications. In Microsoft.Identity.Web you rarely need to
        /// provide this parameter as it's inferred from the context.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>An <see cref="OperationResult{TResult, TError}"/> containing the
        /// <see cref="AuthorizationHeaderInformation"/> on success, or an <see cref="AuthorizationHeaderError"/>
        /// on failure.</returns>
        Task<OperationResult<AuthorizationHeaderInformation, AuthorizationHeaderError>> CreateAuthorizationHeaderInformationAsync(
            IEnumerable<string> scopes,
            AuthorizationHeaderProviderOptions? options = null,
            ClaimsPrincipal? claimsPrincipal = null,
            CancellationToken cancellationToken = default);
    }
}
