// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// <inheritdoc cref="IAuthorizationHeaderProvider"/>
    /// </summary>
    /// <typeparam name="TResult">The result type.</typeparam>
    public interface IAuthorizationHeaderProvider<TResult>
    {
        /// <inheritdoc cref="IAuthorizationHeaderProvider.CreateAuthorizationHeaderAsync"/>
        /// <param name="downstreamApiOptions">
        /// The options containing information about the API
        /// to be called and token acquisition settings.
        /// Use <see cref="DownstreamApiOptions.Scopes"/> to request scope(s)
        /// for the authorization header.
        /// </param>
        /// <param name="claimsPrincipal">
        /// <inheritdoc cref="IAuthorizationHeaderProvider.CreateAuthorizationHeaderAsync"/>
        /// </param>
        /// <param name="cancellationToken">
        /// <inheritdoc cref="IAuthorizationHeaderProvider.CreateAuthorizationHeaderAsync"/>
        /// </param>        
        Task<TResult> CreateAuthorizationHeaderAsync(
            DownstreamApiOptions downstreamApiOptions,
            ClaimsPrincipal? claimsPrincipal = null,
            CancellationToken cancellationToken = default);
    }
}
