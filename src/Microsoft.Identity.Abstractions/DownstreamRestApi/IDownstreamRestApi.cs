// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Interface used to call a downstream REST API, for instance from controllers.
    /// </summary>
    public partial interface IDownstreamRestApi
    {
        /// <summary>
        /// Calls the downstream REST API based on a programmatic description of the
        /// downstream REST API. The choice of calling the API on behalf of the user or the app, is made using
        /// <see cref="AuthorizationHeaderProviderOptions.RequestAppToken"/>.
        /// </summary>
        /// <param name="downstreamRestApiOptions">Options.</param>
        /// <param name="user">(Optional) Claims representing a user. This is useful on platforms like Blazor
        /// or Azure Signal R, where the HttpContext is not available. In other platforms, the library
        /// will find the user from the HTTP request context.</param>
        /// <param name="content">Content to send to the REST API in the case where
        /// <see cref="AuthorizationHeaderProviderOptions.HttpMethod"/> is <code>HttpMethod.Patch</code>, 
        /// <see cref="HttpMethod.Post"/>, <see cref="HttpMethod.Put"/>.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>An <see cref="HttpResponseMessage"/> that the application will process.</returns>
        public Task<HttpResponseMessage> CallRestApiAsync(
            DownstreamRestApiOptions? downstreamRestApiOptions,
            ClaimsPrincipal? user = null,
            HttpContent? content = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Calls the downstream REST API based on a description of the
        /// downstream REST API in the configuration (service name), overridatable programmatically. The choice
        /// of calling the API on behalf of the user or the app, is made by the configuration or programmatically. 
        /// This is the lowest level API. There are other APIs for specific Http methods.
        /// </summary>
        /// <param name="serviceName">Name of the service describing the downstream web API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamRestApiOptions"/>,
        /// each for one downstream REST API. You can pass-in null, but in that case <paramref name="downstreamRestApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="downstreamRestApiOptionsOverride">(Optional) Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>. Can be null, in which case only the configuration is applied.</param>
        /// <param name="user">(Optional) Claims representing a user. This is useful on platforms like Blazor
        /// or Azure Signal R, where the HttpContext is not available. In other platforms, the library
        /// will find the user from the HTTP request context.</param>
        /// <param name="content">Content to send to the REST API in the case where
        /// <see cref="AuthorizationHeaderProviderOptions.HttpMethod"/> is <code>HttpMethod.Patch</code>, 
        /// <see cref="HttpMethod.Post"/>, <see cref="HttpMethod.Put"/>.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>An <see cref="HttpResponseMessage"/> that the application will process.</returns>
        public Task<HttpResponseMessage> CallRestApiAsync(
            string? serviceName,
            Action<DownstreamRestApiOptions>? downstreamRestApiOptionsOverride = null,
            ClaimsPrincipal? user = null,
            HttpContent? content = null,
            CancellationToken cancellationToken = default);


        /// <summary>
        /// Calls the downstream REST API on behalf of the user, based on a description of the
        /// downstream REST API in the configuration (service name), overridatable programmatically. This
        /// is a lower level API. There are other APIs for specific Http methods.
        /// </summary>
        /// <param name="serviceName">Name of the service describing the downstream web API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamRestApiOptions"/>,
        /// each for one downstream REST API. You can pass-in null, but in that case <paramref name="downstreamRestApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="downstreamRestApiOptionsOverride">(Optional) Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="user">(Optional) Claims representing a user. This is useful on platforms like Blazor
        /// or Azure Signal R, where the HttpContext is not available. In other platforms, the library
        /// will find the user from the HTTP request context.</param>
        /// <param name="content">Content to send to the REST API in the case where
        /// <see cref="AuthorizationHeaderProviderOptions.HttpMethod"/> is <code>HttpMethod.Patch</code>, 
        /// <see cref="HttpMethod.Post"/>, <see cref="HttpMethod.Put"/>.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>An <see cref="HttpResponseMessage"/> that the application will process.</returns>
        Task<HttpResponseMessage> CallRestApiForUserAsync(
            string? serviceName,
            Action<DownstreamRestApiOptions>? downstreamRestApiOptionsOverride = null,
            ClaimsPrincipal? user = default,
            HttpContent? content = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Calls the downstream REST API on behalf of the app itself, with the required scopes.
        /// </summary>
        /// <param name="serviceName">Name of the service describing the downstream REST API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamRestApiOptions"/>,
        /// each for one downstream REST API. You can pass-in null, but in that case <paramref name="downstreamRestApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="downstreamRestApiOptionsOverride">(Optional) Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="content">Content to send to the REST API in the case where
        /// <see cref="AuthorizationHeaderProviderOptions.HttpMethod"/> is <code>HttpMethod.Patch</code>, 
        /// <see cref="HttpMethod.Post"/>, <see cref="HttpMethod.Put"/>.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>An <see cref="HttpResponseMessage"/> that the application will process.</returns>
        Task<HttpResponseMessage> CallRestApiForAppAsync(
            string? serviceName,
            Action<DownstreamRestApiOptions>? downstreamRestApiOptionsOverride = null,
            HttpContent? content = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Calls a downstream REST API consuming JSON with some data and returns data.
        /// </summary>
        /// <typeparam name="TInput">Input type.</typeparam>
        /// <typeparam name="TOutput">Output type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream REST API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamRestApiOptions"/>,
        /// each for one downstream REST API. You can pass-in null, but in that case <paramref name="downstreamRestApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Input parameter to the downstream web API.</param>
        /// <param name="downstreamRestApiOptionsOverride">Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="user">[Optional] Claims representing a user. This is useful in platforms like Blazor
        /// or Azure Signal R, where the HttpContext is not available. In other platforms, the library
        /// will find the user from the HttpContext.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The value returned by the downstream web API.</returns>
        /// <example>
        /// A list method that returns an IEnumerable&lt;MyItem&gt;&gt;.
        /// <code>
        /// public Task&lt;IEnumerable&lt;MyItem&gt;&gt; GetAsync()
        /// {
        ///  return _downstreamWebApi.CallWebApiForUserAsync&lt;object, IEnumerable&lt;MyItem&gt;&gt;(
        ///         ServiceName,
        ///         null,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist";
        ///         });
        /// }
        /// </code>
        ///
        /// Example of editing.
        /// <code>
        /// public Task&lt;MyItem&gt; EditAsync(MyItem myItem)
        /// {
        ///   return _downstreamWebApi.CallWebApiForUserAsync&lt;MyItem, MyItem&gt;(
        ///         ServiceName,
        ///         nyItem,
        ///         options =>
        ///         {
        ///            options.HttpMethod = HttpMethod.Patch;
        ///            options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// }
        /// </code>
        /// </example>
        Task<TOutput?> CallRestApiForUserAsync<TInput, TOutput>(
            string? serviceName,
            TInput input,
            Action<DownstreamRestApiOptions>? downstreamRestApiOptionsOverride = null,
            ClaimsPrincipal? user = default,
            CancellationToken cancellationToken = default) where TOutput : class;

        /// <summary>
        /// Call a web API endpoint with an HttpGet, and return strongly typed data.
        /// </summary>
        /// <typeparam name="TOutput">Output type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream REST API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamRestApiOptions"/>,
        /// each for one downstream REST API. You can pass-in null, but in that case <paramref name="downstreamRestApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="downstreamRestApiOptionsOverride">Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="user">[Optional] Claims representing a user. This is useful in platforms like Blazor
        /// or Azure Signal R, where the HttpContext is not available. In other platforms, the library
        /// will find the user from the HttpContext.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The value returned by the downstream web API.</returns>
        Task<TOutput?> CallRestApiForUserAsync<TOutput>(
            string serviceName,
            Action<DownstreamRestApiOptions>? downstreamRestApiOptionsOverride = null,
            ClaimsPrincipal? user = default,
            CancellationToken cancellationToken = default)
            where TOutput : class;
    }
}
