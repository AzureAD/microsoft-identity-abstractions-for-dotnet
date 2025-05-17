// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Abstractions;

#if NET8_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization.Metadata;
#endif

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Interface used to call a downstream API, for instance from controllers, including with
    /// specialized methods depending on the Http Methods.
    /// </summary>
    public partial interface IDownstreamApi
    {

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Get"/>, a downstream API returning data.
        /// By default the returned data is deserialized from JSON but you can provide your own deserializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TOutput">Generic output type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="user">[Optional] Claims representing a user. This is useful in platforms like Blazor
        /// or Azure Signal R, where the HttpContext is not available. In other platforms, the library
        /// will find the user from the HttpContext.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> a Task</returns>
        /// <example>
        /// <code>
        ///  var result = await _downstreamApi.GetForUserAsync&lt;IEnumerable&lt;MyItem&gt;&gt;(
        ///         "MyService",
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist";
        ///         });
        /// </code>
        /// </example>
#if NET8_0_OR_GREATER
        [RequiresUnreferencedCode("Calls JsonSerializer.Serialize<TInput>")]
        [RequiresDynamicCode("Calls JsonSerializer.Serialize<TInput>")]
#endif
        public Task<TOutput?> GetForUserAsync<TOutput>(
            string? serviceName,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            ClaimsPrincipal? user = null,
            CancellationToken cancellationToken = default)where TOutput : class;

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Get"/>, a downstream API with some input data and returning data.
        /// By default the input data is serialized in JSON and the returned data is deserialized from JSON but you can provide your own serializer and your own deserializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <typeparam name="TOutput">Generic output type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="user">[Optional] Claims representing a user. This is useful in platforms like Blazor
        /// or Azure Signal R, where the HttpContext is not available. In other platforms, the library
        /// will find the user from the HttpContext.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///  var result = await _downstreamApi.GetForUserAsync&lt;MyItem, IEnumerable&lt;MyItem&gt;&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
#if NET8_0_OR_GREATER
        [RequiresUnreferencedCode("Calls JsonSerializer.Serialize<TInput>")]
        [RequiresDynamicCode("Calls JsonSerializer.Serialize<TInput>")]
#endif
        public Task<TOutput?> GetForUserAsync<TInput, TOutput>(
            string? serviceName,
            TInput input,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            ClaimsPrincipal? user = null,
            CancellationToken cancellationToken = default)where TOutput : class;

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Get"/>, a downstream API returning data.
        /// By default the returned data is deserialized from JSON but you can provide your own deserializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TOutput">Generic output type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> a Task</returns>
        /// <example>
        /// <code>
        ///  var result = await _downstreamApi.GetForAppAsync&lt;IEnumerable&lt;MyItem&gt;&gt;(
        ///         "MyService",
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist";
        ///         });
        /// </code>
        /// </example>
#if NET8_0_OR_GREATER
        [RequiresUnreferencedCode("Calls JsonSerializer.Serialize<TInput>")]
        [RequiresDynamicCode("Calls JsonSerializer.Serialize<TInput>")]
#endif
        public Task<TOutput?> GetForAppAsync<TOutput>(
            string? serviceName,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            CancellationToken cancellationToken = default)where TOutput : class;

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Get"/>, a downstream API with some input data and returning data.
        /// By default the input data is serialized in JSON and the returned data is deserialized from JSON but you can provide your own serializer and your own deserializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <typeparam name="TOutput">Generic output type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///  var result = await _downstreamApi.GetForAppAsync&lt;MyItem, IEnumerable&lt;MyItem&gt;&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
#if NET8_0_OR_GREATER
        [RequiresUnreferencedCode("Calls JsonSerializer.Serialize<TInput>")]
        [RequiresDynamicCode("Calls JsonSerializer.Serialize<TInput>")]
#endif
        public Task<TOutput?> GetForAppAsync<TInput, TOutput>(
            string? serviceName,
            TInput input,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            CancellationToken cancellationToken = default)where TOutput : class;

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Post"/>, a downstream API with some input data .
        /// By default the input data is serialized in JSON  but you can provide your own serializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="user">[Optional] Claims representing a user. This is useful in platforms like Blazor
        /// or Azure Signal R, where the HttpContext is not available. In other platforms, the library
        /// will find the user from the HttpContext.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///   await _downstreamApi.PostForUserAsync&lt;MyItem&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
#if NET8_0_OR_GREATER
        [RequiresUnreferencedCode("Calls JsonSerializer.Serialize<TInput>")]
        [RequiresDynamicCode("Calls JsonSerializer.Serialize<TInput>")]
#endif
        public Task PostForUserAsync<TInput>(
            string? serviceName,
            TInput input,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            ClaimsPrincipal? user = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Post"/>, a downstream API with some input data and returning data.
        /// By default the input data is serialized in JSON and the returned data is deserialized from JSON but you can provide your own serializer and your own deserializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <typeparam name="TOutput">Generic output type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="user">[Optional] Claims representing a user. This is useful in platforms like Blazor
        /// or Azure Signal R, where the HttpContext is not available. In other platforms, the library
        /// will find the user from the HttpContext.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///  var result = await _downstreamApi.PostForUserAsync&lt;MyItem, IEnumerable&lt;MyItem&gt;&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
#if NET8_0_OR_GREATER
        [RequiresUnreferencedCode("Calls JsonSerializer.Serialize<TInput>")]
        [RequiresDynamicCode("Calls JsonSerializer.Serialize<TInput>")]
#endif
        public Task<TOutput?> PostForUserAsync<TInput, TOutput>(
            string? serviceName,
            TInput input,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            ClaimsPrincipal? user = null,
            CancellationToken cancellationToken = default)where TOutput : class;

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Post"/>, a downstream API with some input data .
        /// By default the input data is serialized in JSON  but you can provide your own serializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///   await _downstreamApi.PostForAppAsync&lt;MyItem&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
#if NET8_0_OR_GREATER
        [RequiresUnreferencedCode("Calls JsonSerializer.Serialize<TInput>")]
        [RequiresDynamicCode("Calls JsonSerializer.Serialize<TInput>")]
#endif
        public Task PostForAppAsync<TInput>(
            string? serviceName,
            TInput input,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Post"/>, a downstream API with some input data and returning data.
        /// By default the input data is serialized in JSON and the returned data is deserialized from JSON but you can provide your own serializer and your own deserializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <typeparam name="TOutput">Generic output type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///  var result = await _downstreamApi.PostForAppAsync&lt;MyItem, IEnumerable&lt;MyItem&gt;&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
#if NET8_0_OR_GREATER
        [RequiresUnreferencedCode("Calls JsonSerializer.Serialize<TInput>")]
        [RequiresDynamicCode("Calls JsonSerializer.Serialize<TInput>")]
#endif
        public Task<TOutput?> PostForAppAsync<TInput, TOutput>(
            string? serviceName,
            TInput input,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            CancellationToken cancellationToken = default)where TOutput : class;

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Put"/>, a downstream API with some input data .
        /// By default the input data is serialized in JSON  but you can provide your own serializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="user">[Optional] Claims representing a user. This is useful in platforms like Blazor
        /// or Azure Signal R, where the HttpContext is not available. In other platforms, the library
        /// will find the user from the HttpContext.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///   await _downstreamApi.PutForUserAsync&lt;MyItem&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
#if NET8_0_OR_GREATER
        [RequiresUnreferencedCode("Calls JsonSerializer.Serialize<TInput>")]
        [RequiresDynamicCode("Calls JsonSerializer.Serialize<TInput>")]
#endif
        public Task PutForUserAsync<TInput>(
            string? serviceName,
            TInput input,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            ClaimsPrincipal? user = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Put"/>, a downstream API with some input data and returning data.
        /// By default the input data is serialized in JSON and the returned data is deserialized from JSON but you can provide your own serializer and your own deserializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <typeparam name="TOutput">Generic output type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="user">[Optional] Claims representing a user. This is useful in platforms like Blazor
        /// or Azure Signal R, where the HttpContext is not available. In other platforms, the library
        /// will find the user from the HttpContext.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///  var result = await _downstreamApi.PutForUserAsync&lt;MyItem, IEnumerable&lt;MyItem&gt;&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
#if NET8_0_OR_GREATER
        [RequiresUnreferencedCode("Calls JsonSerializer.Serialize<TInput>")]
        [RequiresDynamicCode("Calls JsonSerializer.Serialize<TInput>")]
#endif
        public Task<TOutput?> PutForUserAsync<TInput, TOutput>(
            string? serviceName,
            TInput input,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            ClaimsPrincipal? user = null,
            CancellationToken cancellationToken = default)where TOutput : class;

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Put"/>, a downstream API with some input data .
        /// By default the input data is serialized in JSON  but you can provide your own serializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///   await _downstreamApi.PutForAppAsync&lt;MyItem&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
#if NET8_0_OR_GREATER
        [RequiresUnreferencedCode("Calls JsonSerializer.Serialize<TInput>")]
        [RequiresDynamicCode("Calls JsonSerializer.Serialize<TInput>")]
#endif
        public Task PutForAppAsync<TInput>(
            string? serviceName,
            TInput input,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Put"/>, a downstream API with some input data and returning data.
        /// By default the input data is serialized in JSON and the returned data is deserialized from JSON but you can provide your own serializer and your own deserializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <typeparam name="TOutput">Generic output type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///  var result = await _downstreamApi.PutForAppAsync&lt;MyItem, IEnumerable&lt;MyItem&gt;&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
#if NET8_0_OR_GREATER
        [RequiresUnreferencedCode("Calls JsonSerializer.Serialize<TInput>")]
        [RequiresDynamicCode("Calls JsonSerializer.Serialize<TInput>")]
#endif
        public Task<TOutput?> PutForAppAsync<TInput, TOutput>(
            string? serviceName,
            TInput input,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            CancellationToken cancellationToken = default)where TOutput : class;

#if NETSTANDARD2_1_OR_GREATER || NET

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Patch"/>, a downstream API with some input data .
        /// By default the input data is serialized in JSON  but you can provide your own serializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="user">[Optional] Claims representing a user. This is useful in platforms like Blazor
        /// or Azure Signal R, where the HttpContext is not available. In other platforms, the library
        /// will find the user from the HttpContext.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///   await _downstreamApi.PatchForUserAsync&lt;MyItem&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
#if NET8_0_OR_GREATER
        [RequiresUnreferencedCode("Calls JsonSerializer.Serialize<TInput>")]
        [RequiresDynamicCode("Calls JsonSerializer.Serialize<TInput>")]
#endif
        public Task PatchForUserAsync<TInput>(
            string? serviceName,
            TInput input,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            ClaimsPrincipal? user = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Patch"/>, a downstream API with some input data and returning data.
        /// By default the input data is serialized in JSON and the returned data is deserialized from JSON but you can provide your own serializer and your own deserializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <typeparam name="TOutput">Generic output type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="user">[Optional] Claims representing a user. This is useful in platforms like Blazor
        /// or Azure Signal R, where the HttpContext is not available. In other platforms, the library
        /// will find the user from the HttpContext.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///  var result = await _downstreamApi.PatchForUserAsync&lt;MyItem, IEnumerable&lt;MyItem&gt;&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
#if NET8_0_OR_GREATER
        [RequiresUnreferencedCode("Calls JsonSerializer.Serialize<TInput>")]
        [RequiresDynamicCode("Calls JsonSerializer.Serialize<TInput>")]
#endif
        public Task<TOutput?> PatchForUserAsync<TInput, TOutput>(
            string? serviceName,
            TInput input,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            ClaimsPrincipal? user = null,
            CancellationToken cancellationToken = default)where TOutput : class;

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Patch"/>, a downstream API with some input data .
        /// By default the input data is serialized in JSON  but you can provide your own serializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///   await _downstreamApi.PatchForAppAsync&lt;MyItem&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
#if NET8_0_OR_GREATER
        [RequiresUnreferencedCode("Calls JsonSerializer.Serialize<TInput>")]
        [RequiresDynamicCode("Calls JsonSerializer.Serialize<TInput>")]
#endif
        public Task PatchForAppAsync<TInput>(
            string? serviceName,
            TInput input,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Patch"/>, a downstream API with some input data and returning data.
        /// By default the input data is serialized in JSON and the returned data is deserialized from JSON but you can provide your own serializer and your own deserializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <typeparam name="TOutput">Generic output type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///  var result = await _downstreamApi.PatchForAppAsync&lt;MyItem, IEnumerable&lt;MyItem&gt;&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
#if NET8_0_OR_GREATER
        [RequiresUnreferencedCode("Calls JsonSerializer.Serialize<TInput>")]
        [RequiresDynamicCode("Calls JsonSerializer.Serialize<TInput>")]
#endif
        public Task<TOutput?> PatchForAppAsync<TInput, TOutput>(
            string? serviceName,
            TInput input,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            CancellationToken cancellationToken = default)where TOutput : class;

#endif // NETSTANDARD2_1_OR_GREATER || NET

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Delete"/>, a downstream API with some input data .
        /// By default the input data is serialized in JSON  but you can provide your own serializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="user">[Optional] Claims representing a user. This is useful in platforms like Blazor
        /// or Azure Signal R, where the HttpContext is not available. In other platforms, the library
        /// will find the user from the HttpContext.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///   await _downstreamApi.DeleteForUserAsync&lt;MyItem&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
#if NET8_0_OR_GREATER
        [RequiresUnreferencedCode("Calls JsonSerializer.Serialize<TInput>")]
        [RequiresDynamicCode("Calls JsonSerializer.Serialize<TInput>")]
#endif
        public Task DeleteForUserAsync<TInput>(
            string? serviceName,
            TInput input,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            ClaimsPrincipal? user = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Delete"/>, a downstream API with some input data and returning data.
        /// By default the input data is serialized in JSON and the returned data is deserialized from JSON but you can provide your own serializer and your own deserializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <typeparam name="TOutput">Generic output type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="user">[Optional] Claims representing a user. This is useful in platforms like Blazor
        /// or Azure Signal R, where the HttpContext is not available. In other platforms, the library
        /// will find the user from the HttpContext.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///  var result = await _downstreamApi.DeleteForUserAsync&lt;MyItem, IEnumerable&lt;MyItem&gt;&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
#if NET8_0_OR_GREATER
        [RequiresUnreferencedCode("Calls JsonSerializer.Serialize<TInput>")]
        [RequiresDynamicCode("Calls JsonSerializer.Serialize<TInput>")]
#endif
        public Task<TOutput?> DeleteForUserAsync<TInput, TOutput>(
            string? serviceName,
            TInput input,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            ClaimsPrincipal? user = null,
            CancellationToken cancellationToken = default)where TOutput : class;

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Delete"/>, a downstream API with some input data .
        /// By default the input data is serialized in JSON  but you can provide your own serializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///   await _downstreamApi.DeleteForAppAsync&lt;MyItem&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
#if NET8_0_OR_GREATER
        [RequiresUnreferencedCode("Calls JsonSerializer.Serialize<TInput>")]
        [RequiresDynamicCode("Calls JsonSerializer.Serialize<TInput>")]
#endif
        public Task DeleteForAppAsync<TInput>(
            string? serviceName,
            TInput input,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Delete"/>, a downstream API with some input data and returning data.
        /// By default the input data is serialized in JSON and the returned data is deserialized from JSON but you can provide your own serializer and your own deserializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <typeparam name="TOutput">Generic output type.</typeparam>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///  var result = await _downstreamApi.DeleteForAppAsync&lt;MyItem, IEnumerable&lt;MyItem&gt;&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
#if NET8_0_OR_GREATER
        [RequiresUnreferencedCode("Calls JsonSerializer.Serialize<TInput>")]
        [RequiresDynamicCode("Calls JsonSerializer.Serialize<TInput>")]
#endif
        public Task<TOutput?> DeleteForAppAsync<TInput, TOutput>(
            string? serviceName,
            TInput input,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            CancellationToken cancellationToken = default)where TOutput : class;

#if NET8_0_OR_GREATER

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Get"/>, a downstream API returning data.
        /// By default the returned data is deserialized from JSON but you can provide your own deserializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TOutput">Generic output type.</typeparam>
        /// <param name="outputJsonTypeInfo">JSON serialization metadata for TOutput</param>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="user">[Optional] Claims representing a user. This is useful in platforms like Blazor
        /// or Azure Signal R, where the HttpContext is not available. In other platforms, the library
        /// will find the user from the HttpContext.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> a Task</returns>
        /// <example>
        /// <code>
        ///  var result = await _downstreamApi.GetForUserAsync&lt;IEnumerable&lt;MyItem&gt;&gt;(
        ///         "MyService",
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist";
        ///         });
        /// </code>
        /// </example>
        public Task<TOutput?> GetForUserAsync<TOutput>(
            string? serviceName,
            JsonTypeInfo<TOutput> outputJsonTypeInfo,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            ClaimsPrincipal? user = null,
            CancellationToken cancellationToken = default)where TOutput : class;

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Get"/>, a downstream API with some input data and returning data.
        /// By default the input data is serialized in JSON and the returned data is deserialized from JSON but you can provide your own serializer and your own deserializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <typeparam name="TOutput">Generic output type.</typeparam>
        /// <param name="inputJsonTypeInfo">JSON serialization metadata for TInput</param>
        /// <param name="outputJsonTypeInfo">JSON serialization metadata for TOutput</param>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="user">[Optional] Claims representing a user. This is useful in platforms like Blazor
        /// or Azure Signal R, where the HttpContext is not available. In other platforms, the library
        /// will find the user from the HttpContext.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///  var result = await _downstreamApi.GetForUserAsync&lt;MyItem, IEnumerable&lt;MyItem&gt;&gt;(
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
            JsonTypeInfo<TInput> inputJsonTypeInfo,
            JsonTypeInfo<TOutput> outputJsonTypeInfo,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            ClaimsPrincipal? user = null,
            CancellationToken cancellationToken = default)where TOutput : class;

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Get"/>, a downstream API returning data.
        /// By default the returned data is deserialized from JSON but you can provide your own deserializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TOutput">Generic output type.</typeparam>
        /// <param name="outputJsonTypeInfo">JSON serialization metadata for TOutput</param>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> a Task</returns>
        /// <example>
        /// <code>
        ///  var result = await _downstreamApi.GetForAppAsync&lt;IEnumerable&lt;MyItem&gt;&gt;(
        ///         "MyService",
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist";
        ///         });
        /// </code>
        /// </example>
        public Task<TOutput?> GetForAppAsync<TOutput>(
            string? serviceName,
            JsonTypeInfo<TOutput> outputJsonTypeInfo,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            CancellationToken cancellationToken = default)where TOutput : class;

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Get"/>, a downstream API with some input data and returning data.
        /// By default the input data is serialized in JSON and the returned data is deserialized from JSON but you can provide your own serializer and your own deserializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <typeparam name="TOutput">Generic output type.</typeparam>
        /// <param name="inputJsonTypeInfo">JSON serialization metadata for TInput</param>
        /// <param name="outputJsonTypeInfo">JSON serialization metadata for TOutput</param>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///  var result = await _downstreamApi.GetForAppAsync&lt;MyItem, IEnumerable&lt;MyItem&gt;&gt;(
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
            JsonTypeInfo<TInput> inputJsonTypeInfo,
            JsonTypeInfo<TOutput> outputJsonTypeInfo,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            CancellationToken cancellationToken = default)where TOutput : class;

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Post"/>, a downstream API with some input data .
        /// By default the input data is serialized in JSON  but you can provide your own serializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <param name="inputJsonTypeInfo">JSON serialization metadata for TInput</param>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="user">[Optional] Claims representing a user. This is useful in platforms like Blazor
        /// or Azure Signal R, where the HttpContext is not available. In other platforms, the library
        /// will find the user from the HttpContext.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///   await _downstreamApi.PostForUserAsync&lt;MyItem&gt;(
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
            JsonTypeInfo<TInput> inputJsonTypeInfo,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            ClaimsPrincipal? user = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Post"/>, a downstream API with some input data and returning data.
        /// By default the input data is serialized in JSON and the returned data is deserialized from JSON but you can provide your own serializer and your own deserializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <typeparam name="TOutput">Generic output type.</typeparam>
        /// <param name="inputJsonTypeInfo">JSON serialization metadata for TInput</param>
        /// <param name="outputJsonTypeInfo">JSON serialization metadata for TOutput</param>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="user">[Optional] Claims representing a user. This is useful in platforms like Blazor
        /// or Azure Signal R, where the HttpContext is not available. In other platforms, the library
        /// will find the user from the HttpContext.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///  var result = await _downstreamApi.PostForUserAsync&lt;MyItem, IEnumerable&lt;MyItem&gt;&gt;(
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
            JsonTypeInfo<TInput> inputJsonTypeInfo,
            JsonTypeInfo<TOutput> outputJsonTypeInfo,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            ClaimsPrincipal? user = null,
            CancellationToken cancellationToken = default)where TOutput : class;

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Post"/>, a downstream API with some input data .
        /// By default the input data is serialized in JSON  but you can provide your own serializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <param name="inputJsonTypeInfo">JSON serialization metadata for TInput</param>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///   await _downstreamApi.PostForAppAsync&lt;MyItem&gt;(
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
            JsonTypeInfo<TInput> inputJsonTypeInfo,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Post"/>, a downstream API with some input data and returning data.
        /// By default the input data is serialized in JSON and the returned data is deserialized from JSON but you can provide your own serializer and your own deserializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <typeparam name="TOutput">Generic output type.</typeparam>
        /// <param name="inputJsonTypeInfo">JSON serialization metadata for TInput</param>
        /// <param name="outputJsonTypeInfo">JSON serialization metadata for TOutput</param>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///  var result = await _downstreamApi.PostForAppAsync&lt;MyItem, IEnumerable&lt;MyItem&gt;&gt;(
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
            JsonTypeInfo<TInput> inputJsonTypeInfo,
            JsonTypeInfo<TOutput> outputJsonTypeInfo,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            CancellationToken cancellationToken = default)where TOutput : class;

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Put"/>, a downstream API with some input data .
        /// By default the input data is serialized in JSON  but you can provide your own serializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <param name="inputJsonTypeInfo">JSON serialization metadata for TInput</param>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="user">[Optional] Claims representing a user. This is useful in platforms like Blazor
        /// or Azure Signal R, where the HttpContext is not available. In other platforms, the library
        /// will find the user from the HttpContext.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///   await _downstreamApi.PutForUserAsync&lt;MyItem&gt;(
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
            JsonTypeInfo<TInput> inputJsonTypeInfo,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            ClaimsPrincipal? user = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Put"/>, a downstream API with some input data and returning data.
        /// By default the input data is serialized in JSON and the returned data is deserialized from JSON but you can provide your own serializer and your own deserializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <typeparam name="TOutput">Generic output type.</typeparam>
        /// <param name="inputJsonTypeInfo">JSON serialization metadata for TInput</param>
        /// <param name="outputJsonTypeInfo">JSON serialization metadata for TOutput</param>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="user">[Optional] Claims representing a user. This is useful in platforms like Blazor
        /// or Azure Signal R, where the HttpContext is not available. In other platforms, the library
        /// will find the user from the HttpContext.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///  var result = await _downstreamApi.PutForUserAsync&lt;MyItem, IEnumerable&lt;MyItem&gt;&gt;(
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
            JsonTypeInfo<TInput> inputJsonTypeInfo,
            JsonTypeInfo<TOutput> outputJsonTypeInfo,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            ClaimsPrincipal? user = null,
            CancellationToken cancellationToken = default)where TOutput : class;

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Put"/>, a downstream API with some input data .
        /// By default the input data is serialized in JSON  but you can provide your own serializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <param name="inputJsonTypeInfo">JSON serialization metadata for TInput</param>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///   await _downstreamApi.PutForAppAsync&lt;MyItem&gt;(
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
            JsonTypeInfo<TInput> inputJsonTypeInfo,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Put"/>, a downstream API with some input data and returning data.
        /// By default the input data is serialized in JSON and the returned data is deserialized from JSON but you can provide your own serializer and your own deserializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <typeparam name="TOutput">Generic output type.</typeparam>
        /// <param name="inputJsonTypeInfo">JSON serialization metadata for TInput</param>
        /// <param name="outputJsonTypeInfo">JSON serialization metadata for TOutput</param>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///  var result = await _downstreamApi.PutForAppAsync&lt;MyItem, IEnumerable&lt;MyItem&gt;&gt;(
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
            JsonTypeInfo<TInput> inputJsonTypeInfo,
            JsonTypeInfo<TOutput> outputJsonTypeInfo,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            CancellationToken cancellationToken = default)where TOutput : class;

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Patch"/>, a downstream API with some input data .
        /// By default the input data is serialized in JSON  but you can provide your own serializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <param name="inputJsonTypeInfo">JSON serialization metadata for TInput</param>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="user">[Optional] Claims representing a user. This is useful in platforms like Blazor
        /// or Azure Signal R, where the HttpContext is not available. In other platforms, the library
        /// will find the user from the HttpContext.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///   await _downstreamApi.PatchForUserAsync&lt;MyItem&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
        public Task PatchForUserAsync<TInput>(
            string? serviceName,
            TInput input,
            JsonTypeInfo<TInput> inputJsonTypeInfo,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            ClaimsPrincipal? user = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Patch"/>, a downstream API with some input data and returning data.
        /// By default the input data is serialized in JSON and the returned data is deserialized from JSON but you can provide your own serializer and your own deserializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <typeparam name="TOutput">Generic output type.</typeparam>
        /// <param name="inputJsonTypeInfo">JSON serialization metadata for TInput</param>
        /// <param name="outputJsonTypeInfo">JSON serialization metadata for TOutput</param>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="user">[Optional] Claims representing a user. This is useful in platforms like Blazor
        /// or Azure Signal R, where the HttpContext is not available. In other platforms, the library
        /// will find the user from the HttpContext.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///  var result = await _downstreamApi.PatchForUserAsync&lt;MyItem, IEnumerable&lt;MyItem&gt;&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
        public Task<TOutput?> PatchForUserAsync<TInput, TOutput>(
            string? serviceName,
            TInput input,
            JsonTypeInfo<TInput> inputJsonTypeInfo,
            JsonTypeInfo<TOutput> outputJsonTypeInfo,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            ClaimsPrincipal? user = null,
            CancellationToken cancellationToken = default)where TOutput : class;

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Patch"/>, a downstream API with some input data .
        /// By default the input data is serialized in JSON  but you can provide your own serializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <param name="inputJsonTypeInfo">JSON serialization metadata for TInput</param>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///   await _downstreamApi.PatchForAppAsync&lt;MyItem&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
        public Task PatchForAppAsync<TInput>(
            string? serviceName,
            TInput input,
            JsonTypeInfo<TInput> inputJsonTypeInfo,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Patch"/>, a downstream API with some input data and returning data.
        /// By default the input data is serialized in JSON and the returned data is deserialized from JSON but you can provide your own serializer and your own deserializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <typeparam name="TOutput">Generic output type.</typeparam>
        /// <param name="inputJsonTypeInfo">JSON serialization metadata for TInput</param>
        /// <param name="outputJsonTypeInfo">JSON serialization metadata for TOutput</param>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///  var result = await _downstreamApi.PatchForAppAsync&lt;MyItem, IEnumerable&lt;MyItem&gt;&gt;(
        ///         "MyService",
        ///         myItem,
        ///         options =>
        ///         {
        ///           options.RelativePath = $"api/todolist/{myItem.Id}";
        ///         });
        /// </code>
        /// </example>
        public Task<TOutput?> PatchForAppAsync<TInput, TOutput>(
            string? serviceName,
            TInput input,
            JsonTypeInfo<TInput> inputJsonTypeInfo,
            JsonTypeInfo<TOutput> outputJsonTypeInfo,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            CancellationToken cancellationToken = default)where TOutput : class;

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Delete"/>, a downstream API with some input data .
        /// By default the input data is serialized in JSON  but you can provide your own serializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <param name="inputJsonTypeInfo">JSON serialization metadata for TInput</param>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="user">[Optional] Claims representing a user. This is useful in platforms like Blazor
        /// or Azure Signal R, where the HttpContext is not available. In other platforms, the library
        /// will find the user from the HttpContext.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///   await _downstreamApi.DeleteForUserAsync&lt;MyItem&gt;(
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
            JsonTypeInfo<TInput> inputJsonTypeInfo,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            ClaimsPrincipal? user = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Delete"/>, a downstream API with some input data and returning data.
        /// By default the input data is serialized in JSON and the returned data is deserialized from JSON but you can provide your own serializer and your own deserializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <typeparam name="TOutput">Generic output type.</typeparam>
        /// <param name="inputJsonTypeInfo">JSON serialization metadata for TInput</param>
        /// <param name="outputJsonTypeInfo">JSON serialization metadata for TOutput</param>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="user">[Optional] Claims representing a user. This is useful in platforms like Blazor
        /// or Azure Signal R, where the HttpContext is not available. In other platforms, the library
        /// will find the user from the HttpContext.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///  var result = await _downstreamApi.DeleteForUserAsync&lt;MyItem, IEnumerable&lt;MyItem&gt;&gt;(
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
            JsonTypeInfo<TInput> inputJsonTypeInfo,
            JsonTypeInfo<TOutput> outputJsonTypeInfo,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            ClaimsPrincipal? user = null,
            CancellationToken cancellationToken = default)where TOutput : class;

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Delete"/>, a downstream API with some input data .
        /// By default the input data is serialized in JSON  but you can provide your own serializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <param name="inputJsonTypeInfo">JSON serialization metadata for TInput</param>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///   await _downstreamApi.DeleteForAppAsync&lt;MyItem&gt;(
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
            JsonTypeInfo<TInput> inputJsonTypeInfo,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Calls, using <see cref="HttpMethod.Delete"/>, a downstream API with some input data and returning data.
        /// By default the input data is serialized in JSON and the returned data is deserialized from JSON but you can provide your own serializer and your own deserializer in the action
        /// you pass-in through the <paramref name="downstreamApiOptionsOverride"/> parameter.
        /// </summary>
        /// <typeparam name="TInput">Generic input type.</typeparam>
        /// <typeparam name="TOutput">Generic output type.</typeparam>
        /// <param name="inputJsonTypeInfo">JSON serialization metadata for TInput</param>
        /// <param name="outputJsonTypeInfo">JSON serialization metadata for TOutput</param>
        /// <param name="serviceName">Name of the service describing the downstream API. There can
        /// be several configuration named sections mapped to a <see cref="DownstreamApiOptions"/>,
        /// each for one downstream API. You can pass-in null, but in that case <paramref name="downstreamApiOptionsOverride"/>
        /// needs to be set.</param>
        /// <param name="input">Data sent to the downstream web API, through the body or the HTTP request.</param>
        /// <param name="downstreamApiOptionsOverride">[Optional] Overrides the options proposed in the configuration described
        /// by <paramref name="serviceName"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns> The value returned by the downstream web API.</returns>
        /// <example>
        /// <code>
        ///  var result = await _downstreamApi.DeleteForAppAsync&lt;MyItem, IEnumerable&lt;MyItem&gt;&gt;(
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
            JsonTypeInfo<TInput> inputJsonTypeInfo,
            JsonTypeInfo<TOutput> outputJsonTypeInfo,
            Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null,
            CancellationToken cancellationToken = default)where TOutput : class;
#endif // NET8_0_OR_GREATER
    }
}
