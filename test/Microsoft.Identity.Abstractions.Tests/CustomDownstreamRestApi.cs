// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Identity.Abstractions.DownstreamRestApi.Tests
{
    internal class CustomDownstreamRestApi : IDownstreamRestApi
    {
        public CustomDownstreamRestApi()
        {
        }

        public Task<HttpResponseMessage> CallRestApiAsync(string? serviceName, Action<DownstreamRestApiOptions>? downstreamRestApiOptionsOverride = null, ClaimsPrincipal? user = null, HttpContent? content = null, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<HttpResponseMessage>(null!);
        }

        public Task<HttpResponseMessage> CallRestApiAsync(DownstreamRestApiOptions? downstreamRestApiOptions, ClaimsPrincipal? user = null, HttpContent? content = null, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<HttpResponseMessage>(null!);
        }

        public Task<HttpResponseMessage> CallRestApiForAppAsync(string? serviceName, Action<DownstreamRestApiOptions>? downstreamRestApiOptionsOverride = null, HttpContent? content = null, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<HttpResponseMessage>(null!);
        }

        public Task<TOutput?> CallRestApiForAppAsync<TInput, TOutput>(string? serviceName, TInput input, Action<DownstreamRestApiOptions>? downstreamRestApiOptionsOverride = null, CancellationToken cancellationToken = default) where TOutput : class
        {
            return Task.FromResult<TOutput?>(default);
        }

        public Task<TOutput?> CallRestApiForAppAsync<TOutput>(string serviceName, Action<DownstreamRestApiOptions>? downstreamRestApiOptionsOverride = null, CancellationToken cancellationToken = default) where TOutput : class
        {
            return Task.FromResult<TOutput?>(default);
        }

        public Task<HttpResponseMessage> CallRestApiForUserAsync(string? serviceName, Action<DownstreamRestApiOptions>? downstreamRestApiOptionsOverride = null, ClaimsPrincipal? user = null, HttpContent? content = null, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<HttpResponseMessage>(null!);
        }

        public Task<TOutput?> CallRestApiForUserAsync<TInput, TOutput>(string? serviceName, TInput input, Action<DownstreamRestApiOptions>? downstreamRestApiOptionsOverride = null, ClaimsPrincipal? user = null, CancellationToken cancellationToken = default) where TOutput : class
        {
            return Task.FromResult<TOutput?>(default);
        }

        public Task<TOutput?> CallRestApiForUserAsync<TOutput>(string? serviceName, Action<DownstreamRestApiOptions>? downstreamRestApiOptionsOverride = null, ClaimsPrincipal? user = null, CancellationToken cancellationToken = default) where TOutput : class
        {
            return Task.FromResult<TOutput?>(default);
        }

        public Task DeleteForAppAsync<TInput>(string? serviceName, TInput input, Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task<TOutput?> DeleteForAppAsync<TInput, TOutput>(string? serviceName, TInput input, Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null, CancellationToken cancellationToken = default) where TOutput : class
        {
            return Task.FromResult<TOutput?>(default);
        }

        public Task DeleteForUserAsync<TInput>(string? serviceName, TInput input, Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null, ClaimsPrincipal? user = null, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task<TOutput?> DeleteForUserAsync<TInput, TOutput>(string? serviceName, TInput input, Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null, ClaimsPrincipal? user = null, CancellationToken cancellationToken = default) where TOutput : class
        {
            return Task.FromResult<TOutput?>(default);
        }

        public Task<TOutput?> GetForAppAsync<TOutput>(string? serviceName, Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null, CancellationToken cancellationToken = default) where TOutput : class
        {
            return Task.FromResult<TOutput?>(default);
        }

        public Task<TOutput?> GetForAppAsync<TInput, TOutput>(string? serviceName, TInput input, Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null, CancellationToken cancellationToken = default) where TOutput : class
        {
            return Task.FromResult<TOutput?>(default);
        }

        public Task<TOutput?> GetForUserAsync<TOutput>(string? serviceName, Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null, ClaimsPrincipal? user = null, CancellationToken cancellationToken = default) where TOutput : class
        {
            return Task.FromResult<TOutput?>(default);
        }

        public Task<TOutput?> GetForUserAsync<TInput, TOutput>(string? serviceName, TInput input, Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null, ClaimsPrincipal? user = null, CancellationToken cancellationToken = default) where TOutput : class
        {
            return Task.FromResult<TOutput?>(default);
        }

        public Task PatchForAppAsync<TInput>(string? serviceName, TInput input, Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<TOutput?> PatchForAppAsync<TOutput>(string? serviceName, Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null, CancellationToken cancellationToken = default) where TOutput : class
        {
            throw new NotImplementedException();
        }

        public Task<TOutput?> PatchForAppAsync<TInput, TOutput>(string? serviceName, TInput input, Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null, CancellationToken cancellationToken = default) where TOutput : class
        {
            return Task.FromResult<TOutput?>(null);
        }

        public Task PatchForUserAsync<TInput>(string? serviceName, TInput input, Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null, ClaimsPrincipal? user = null, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<HttpResponseMessage>(null!);
        }

        public Task<TOutput?> PatchForUserAsync<TOutput>(string? serviceName, Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null, ClaimsPrincipal? user = null, CancellationToken cancellationToken = default) where TOutput : class
        {
            return Task.FromResult<TOutput?>(null);
        }

        public Task<TOutput?> PatchForUserAsync<TInput, TOutput>(string? serviceName, TInput input, Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null, ClaimsPrincipal? user = null, CancellationToken cancellationToken = default) where TOutput : class
        {
            return Task.FromResult<TOutput?>(null);
        }

        public Task PostForAppAsync<TInput>(string? serviceName, TInput input, Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task<TOutput?> PostForAppAsync<TInput, TOutput>(string? serviceName, TInput input, Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null, CancellationToken cancellationToken = default) where TOutput : class
        {
            return Task.FromResult<TOutput?>(default);
        }

        public Task PostForUserAsync<TInput>(string? serviceName, TInput input, Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null, ClaimsPrincipal? user = null, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task<TOutput?> PostForUserAsync<TInput, TOutput>(string? serviceName, TInput input, Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null, ClaimsPrincipal? user = null, CancellationToken cancellationToken = default) where TOutput : class
        {
            return Task.FromResult<TOutput?>(default);
        }

        public Task PutForAppAsync<TInput>(string? serviceName, TInput input, Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task<TOutput?> PutForAppAsync<TInput, TOutput>(string? serviceName, TInput input, Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null, CancellationToken cancellationToken = default) where TOutput : class
        {
            return Task.FromResult<TOutput?>(default);
        }

        public Task PutForUserAsync<TInput>(string? serviceName, TInput input, Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null, ClaimsPrincipal? user = null, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task<TOutput?> PutForUserAsync<TInput, TOutput>(string? serviceName, TInput input, Action<DownstreamRestApiOptionsReadOnlyHttpMethod>? downstreamRestApiOptionsOverride = null, ClaimsPrincipal? user = null, CancellationToken cancellationToken = default) where TOutput : class
        {
            return Task.FromResult<TOutput?>(default);
        }
    }
}
