// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Abstractions;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Interface used to call a downstream REST API, for instance from controllers, including with
    /// specialized methods depending on the Http Methods.
    /// </summary>
    public partial interface IDownstreamRestApi
    {

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Get"/>, a downstream REST API returning data.
        /// By default the returned data is deserialized from JSON but you can provide your own deserializer in the action
        /// you pass-in through the <paramref name="downstreamRestApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TOutput">Generic output type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream REST API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamRestApiOptions"/>,
        /// each for one downstream REST API. You can pass-in null, but in that case <paramref name="downstreamRestApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="downstreamRestApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="user">[Optional] Claims representing a user. This is useful in platforms like Blazor
        /// or Azure Signal R, where the HttpContext is not available. In other platforms, the library
        /// will find the user from the HttpContext.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> a Task</returns>
        /// <example>
        /// <code>
        ///  var result = await _downstreamWebApi.GetForUserAsync&lt;IEnumerable&lt;MyItem&gt;&gt;(
        ///         "MyService",
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist";
        ///         });
        /// </code>
        /// </example>
        public Task<TOutput?> GetForUserAsync<TOutput>(
            string? serviceName,
            Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null,
            ClaimsPrincipal? user = null,
            CancellationToken cancellationToken = default)where TOutput : class;

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Get"/>, a downstream REST API with some input data and returning data.
        /// By default the input data is serialized in JSON and the returned data is deserialized from JSON but you can provide your own serializer and your own deserializer in the action
        /// you pass-in through the <paramref name="downstreamRestApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <typeparam name="TOutput">Generic output type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream REST API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamRestApiOptions"/>,
        /// each for one downstream REST API. You can pass-in null, but in that case <paramref name="downstreamRestApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamRestApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="user">[Optional] Claims representing a user. This is useful in platforms like Blazor
        /// or Azure Signal R, where the HttpContext is not available. In other platforms, the library
        /// will find the user from the HttpContext.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///  var result = await _downstreamWebApi.GetForUserAsync&lt;MyItem, IEnumerable&lt;MyItem&gt;&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
        public Task<TOutput?> GetForUserAsync<TInput, TOutput>(
            string? serviceName,
            TInput input,
            Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null,
            ClaimsPrincipal? user = null,
            CancellationToken cancellationToken = default)where TOutput : class;

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Get"/>, a downstream REST API returning data.
        /// By default the returned data is deserialized from JSON but you can provide your own deserializer in the action
        /// you pass-in through the <paramref name="downstreamRestApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TOutput">Generic output type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream REST API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamRestApiOptions"/>,
        /// each for one downstream REST API. You can pass-in null, but in that case <paramref name="downstreamRestApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="downstreamRestApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> a Task</returns>
        /// <example>
        /// <code>
        ///  var result = await _downstreamWebApi.GetForAppAsync&lt;IEnumerable&lt;MyItem&gt;&gt;(
        ///         "MyService",
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist";
        ///         });
        /// </code>
        /// </example>
        public Task<TOutput?> GetForAppAsync<TOutput>(
            string? serviceName,
            Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null,
            CancellationToken cancellationToken = default)where TOutput : class;

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Get"/>, a downstream REST API with some input data and returning data.
        /// By default the input data is serialized in JSON and the returned data is deserialized from JSON but you can provide your own serializer and your own deserializer in the action
        /// you pass-in through the <paramref name="downstreamRestApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <typeparam name="TOutput">Generic output type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream REST API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamRestApiOptions"/>,
        /// each for one downstream REST API. You can pass-in null, but in that case <paramref name="downstreamRestApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamRestApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///  var result = await _downstreamWebApi.GetForAppAsync&lt;MyItem, IEnumerable&lt;MyItem&gt;&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
        public Task<TOutput?> GetForAppAsync<TInput, TOutput>(
            string? serviceName,
            TInput input,
            Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null,
            CancellationToken cancellationToken = default)where TOutput : class;

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Post"/>, a downstream REST API with some input data .
        /// By default the input data is serialized in JSON  but you can provide your own serializer  in the action
        /// you pass-in through the <paramref name="downstreamRestApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream REST API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamRestApiOptions"/>,
        /// each for one downstream REST API. You can pass-in null, but in that case <paramref name="downstreamRestApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamRestApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="user">[Optional] Claims representing a user. This is useful in platforms like Blazor
        /// or Azure Signal R, where the HttpContext is not available. In other platforms, the library
        /// will find the user from the HttpContext.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///   await _downstreamWebApi.PostForUserAsync&lt;MyItem&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
        public Task PostForUserAsync<TInput>(
            string? serviceName,
            TInput input,
            Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null,
            ClaimsPrincipal? user = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Post"/>, a downstream REST API with some input data and returning data.
        /// By default the input data is serialized in JSON and the returned data is deserialized from JSON but you can provide your own serializer and your own deserializer in the action
        /// you pass-in through the <paramref name="downstreamRestApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <typeparam name="TOutput">Generic output type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream REST API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamRestApiOptions"/>,
        /// each for one downstream REST API. You can pass-in null, but in that case <paramref name="downstreamRestApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamRestApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="user">[Optional] Claims representing a user. This is useful in platforms like Blazor
        /// or Azure Signal R, where the HttpContext is not available. In other platforms, the library
        /// will find the user from the HttpContext.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///  var result = await _downstreamWebApi.PostForUserAsync&lt;MyItem, IEnumerable&lt;MyItem&gt;&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
        public Task<TOutput?> PostForUserAsync<TInput, TOutput>(
            string? serviceName,
            TInput input,
            Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null,
            ClaimsPrincipal? user = null,
            CancellationToken cancellationToken = default)where TOutput : class;

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Post"/>, a downstream REST API with some input data .
        /// By default the input data is serialized in JSON  but you can provide your own serializer  in the action
        /// you pass-in through the <paramref name="downstreamRestApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream REST API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamRestApiOptions"/>,
        /// each for one downstream REST API. You can pass-in null, but in that case <paramref name="downstreamRestApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamRestApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///   await _downstreamWebApi.PostForAppAsync&lt;MyItem&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
        public Task PostForAppAsync<TInput>(
            string? serviceName,
            TInput input,
            Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Post"/>, a downstream REST API with some input data and returning data.
        /// By default the input data is serialized in JSON and the returned data is deserialized from JSON but you can provide your own serializer and your own deserializer in the action
        /// you pass-in through the <paramref name="downstreamRestApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <typeparam name="TOutput">Generic output type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream REST API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamRestApiOptions"/>,
        /// each for one downstream REST API. You can pass-in null, but in that case <paramref name="downstreamRestApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamRestApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///  var result = await _downstreamWebApi.PostForAppAsync&lt;MyItem, IEnumerable&lt;MyItem&gt;&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
        public Task<TOutput?> PostForAppAsync<TInput, TOutput>(
            string? serviceName,
            TInput input,
            Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null,
            CancellationToken cancellationToken = default)where TOutput : class;

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Put"/>, a downstream REST API with some input data .
        /// By default the input data is serialized in JSON  but you can provide your own serializer  in the action
        /// you pass-in through the <paramref name="downstreamRestApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream REST API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamRestApiOptions"/>,
        /// each for one downstream REST API. You can pass-in null, but in that case <paramref name="downstreamRestApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamRestApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="user">[Optional] Claims representing a user. This is useful in platforms like Blazor
        /// or Azure Signal R, where the HttpContext is not available. In other platforms, the library
        /// will find the user from the HttpContext.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///   await _downstreamWebApi.PutForUserAsync&lt;MyItem&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
        public Task PutForUserAsync<TInput>(
            string? serviceName,
            TInput input,
            Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null,
            ClaimsPrincipal? user = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Put"/>, a downstream REST API with some input data and returning data.
        /// By default the input data is serialized in JSON and the returned data is deserialized from JSON but you can provide your own serializer and your own deserializer in the action
        /// you pass-in through the <paramref name="downstreamRestApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <typeparam name="TOutput">Generic output type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream REST API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamRestApiOptions"/>,
        /// each for one downstream REST API. You can pass-in null, but in that case <paramref name="downstreamRestApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamRestApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="user">[Optional] Claims representing a user. This is useful in platforms like Blazor
        /// or Azure Signal R, where the HttpContext is not available. In other platforms, the library
        /// will find the user from the HttpContext.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///  var result = await _downstreamWebApi.PutForUserAsync&lt;MyItem, IEnumerable&lt;MyItem&gt;&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
        public Task<TOutput?> PutForUserAsync<TInput, TOutput>(
            string? serviceName,
            TInput input,
            Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null,
            ClaimsPrincipal? user = null,
            CancellationToken cancellationToken = default)where TOutput : class;

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Put"/>, a downstream REST API with some input data .
        /// By default the input data is serialized in JSON  but you can provide your own serializer  in the action
        /// you pass-in through the <paramref name="downstreamRestApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream REST API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamRestApiOptions"/>,
        /// each for one downstream REST API. You can pass-in null, but in that case <paramref name="downstreamRestApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamRestApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///   await _downstreamWebApi.PutForAppAsync&lt;MyItem&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
        public Task PutForAppAsync<TInput>(
            string? serviceName,
            TInput input,
            Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Put"/>, a downstream REST API with some input data and returning data.
        /// By default the input data is serialized in JSON and the returned data is deserialized from JSON but you can provide your own serializer and your own deserializer in the action
        /// you pass-in through the <paramref name="downstreamRestApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <typeparam name="TOutput">Generic output type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream REST API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamRestApiOptions"/>,
        /// each for one downstream REST API. You can pass-in null, but in that case <paramref name="downstreamRestApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamRestApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///  var result = await _downstreamWebApi.PutForAppAsync&lt;MyItem, IEnumerable&lt;MyItem&gt;&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
        public Task<TOutput?> PutForAppAsync<TInput, TOutput>(
            string? serviceName,
            TInput input,
            Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null,
            CancellationToken cancellationToken = default)where TOutput : class;

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Delete"/>, a downstream REST API with some input data .
        /// By default the input data is serialized in JSON  but you can provide your own serializer  in the action
        /// you pass-in through the <paramref name="downstreamRestApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream REST API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamRestApiOptions"/>,
        /// each for one downstream REST API. You can pass-in null, but in that case <paramref name="downstreamRestApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamRestApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="user">[Optional] Claims representing a user. This is useful in platforms like Blazor
        /// or Azure Signal R, where the HttpContext is not available. In other platforms, the library
        /// will find the user from the HttpContext.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///   await _downstreamWebApi.DeleteForUserAsync&lt;MyItem&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
        public Task DeleteForUserAsync<TInput>(
            string? serviceName,
            TInput input,
            Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null,
            ClaimsPrincipal? user = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Delete"/>, a downstream REST API with some input data and returning data.
        /// By default the input data is serialized in JSON and the returned data is deserialized from JSON but you can provide your own serializer and your own deserializer in the action
        /// you pass-in through the <paramref name="downstreamRestApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <typeparam name="TOutput">Generic output type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream REST API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamRestApiOptions"/>,
        /// each for one downstream REST API. You can pass-in null, but in that case <paramref name="downstreamRestApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamRestApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="user">[Optional] Claims representing a user. This is useful in platforms like Blazor
        /// or Azure Signal R, where the HttpContext is not available. In other platforms, the library
        /// will find the user from the HttpContext.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///  var result = await _downstreamWebApi.DeleteForUserAsync&lt;MyItem, IEnumerable&lt;MyItem&gt;&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
        public Task<TOutput?> DeleteForUserAsync<TInput, TOutput>(
            string? serviceName,
            TInput input,
            Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null,
            ClaimsPrincipal? user = null,
            CancellationToken cancellationToken = default)where TOutput : class;

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Delete"/>, a downstream REST API with some input data .
        /// By default the input data is serialized in JSON  but you can provide your own serializer  in the action
        /// you pass-in through the <paramref name="downstreamRestApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream REST API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamRestApiOptions"/>,
        /// each for one downstream REST API. You can pass-in null, but in that case <paramref name="downstreamRestApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamRestApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///   await _downstreamWebApi.DeleteForAppAsync&lt;MyItem&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
        public Task DeleteForAppAsync<TInput>(
            string? serviceName,
            TInput input,
            Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Delete"/>, a downstream REST API with some input data and returning data.
        /// By default the input data is serialized in JSON and the returned data is deserialized from JSON but you can provide your own serializer and your own deserializer in the action
        /// you pass-in through the <paramref name="downstreamRestApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <typeparam name="TOutput">Generic output type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream REST API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamRestApiOptions"/>,
        /// each for one downstream REST API. You can pass-in null, but in that case <paramref name="downstreamRestApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamRestApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///  var result = await _downstreamWebApi.DeleteForAppAsync&lt;MyItem, IEnumerable&lt;MyItem&gt;&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
        public Task<TOutput?> DeleteForAppAsync<TInput, TOutput>(
            string? serviceName,
            TInput input,
            Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null,
            CancellationToken cancellationToken = default)where TOutput : class;
    }
}
