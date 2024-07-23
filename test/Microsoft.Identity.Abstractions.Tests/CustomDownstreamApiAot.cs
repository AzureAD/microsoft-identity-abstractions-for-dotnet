// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
#if NET8_0_OR_GREATER
using System;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Identity.Abstractions.DownstreamApi.Tests
{
    internal partial class CustomDownstreamApi : IDownstreamApi
    {
        private const string JsonResponse = "{ \"ErrorCode\": 200 }";

        public Task<TOutput?> CallApiForAppAsync<TInput, TOutput>(string? serviceName, TInput input, JsonTypeInfo<TInput> inputJsonTypeInfo, JsonTypeInfo<TOutput> outputJsonTypeInfo, Action<DownstreamApiOptions>? downstreamApiOptionsOverride = null, CancellationToken cancellationToken = default) where TOutput : class
        {
            string inputJson = JsonSerializer.Serialize(input, inputJsonTypeInfo);
            return Task.FromResult<TOutput?>(JsonSerializer.Deserialize<TOutput>(inputJson, outputJsonTypeInfo));
        }

        public Task<TOutput?> CallApiForAppAsync<TOutput>(string serviceName, JsonTypeInfo<TOutput> outputJsonTypeInfo, Action<DownstreamApiOptions>? downstreamApiOptionsOverride = null, CancellationToken cancellationToken = default) where TOutput : class
        {
            return Task.FromResult<TOutput?>(JsonSerializer.Deserialize<TOutput>(JsonResponse, outputJsonTypeInfo));
        }

        public Task<TOutput?> CallApiForUserAsync<TInput, TOutput>(string? serviceName, TInput input, JsonTypeInfo<TInput> inputJsonTypeInfo, JsonTypeInfo<TOutput> outputJsonTypeInfo, Action<DownstreamApiOptions>? downstreamApiOptionsOverride = null, ClaimsPrincipal? user = null, CancellationToken cancellationToken = default) where TOutput : class
        {
            string inputJson = JsonSerializer.Serialize(input, inputJsonTypeInfo);
            return Task.FromResult<TOutput?>(JsonSerializer.Deserialize<TOutput>(inputJson, outputJsonTypeInfo));
        }

        public Task<TOutput?> CallApiForUserAsync<TOutput>(string serviceName, JsonTypeInfo<TOutput> outputJsonTypeInfo, Action<DownstreamApiOptions>? downstreamApiOptionsOverride = null, ClaimsPrincipal? user = null, CancellationToken cancellationToken = default) where TOutput : class
        {
            return Task.FromResult<TOutput?>(JsonSerializer.Deserialize<TOutput>(JsonResponse, outputJsonTypeInfo));
        }

        public Task DeleteForAppAsync<TInput>(string? serviceName, TInput input, JsonTypeInfo<TInput> inputJsonTypeInfo, Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null, CancellationToken cancellationToken = default)
        {
            JsonSerializer.Serialize(input, inputJsonTypeInfo);
            return Task.CompletedTask;
        }

        public Task<TOutput?> DeleteForAppAsync<TInput, TOutput>(string? serviceName, TInput input, JsonTypeInfo<TInput> inputJsonTypeInfo, JsonTypeInfo<TOutput> outputJsonTypeInfo, Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null, CancellationToken cancellationToken = default) where TOutput : class
        {
            string inputJson = JsonSerializer.Serialize(input, inputJsonTypeInfo);
            return Task.FromResult<TOutput?>(JsonSerializer.Deserialize<TOutput>(inputJson, outputJsonTypeInfo));
        }

        public Task DeleteForUserAsync<TInput>(string? serviceName, TInput input, JsonTypeInfo<TInput> inputJsonTypeInfo, Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null, ClaimsPrincipal? user = null, CancellationToken cancellationToken = default)
        {
            JsonSerializer.Serialize(input, inputJsonTypeInfo);
            return Task.CompletedTask;
        }

        public Task<TOutput?> DeleteForUserAsync<TInput, TOutput>(string? serviceName, TInput input, JsonTypeInfo<TInput> inputJsonTypeInfo, JsonTypeInfo<TOutput> outputJsonTypeInfo, Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null, ClaimsPrincipal? user = null, CancellationToken cancellationToken = default) where TOutput : class
        {
            string inputJson = JsonSerializer.Serialize(input, inputJsonTypeInfo);
            return Task.FromResult<TOutput?>(JsonSerializer.Deserialize<TOutput>(inputJson, outputJsonTypeInfo));
        }

        public Task<TOutput?> GetForAppAsync<TOutput>(string? serviceName, JsonTypeInfo<TOutput> outputJsonTypeInfo, Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null, CancellationToken cancellationToken = default) where TOutput : class
        {
            return Task.FromResult<TOutput?>(JsonSerializer.Deserialize<TOutput>(JsonResponse, outputJsonTypeInfo));
        }

        public Task<TOutput?> GetForAppAsync<TInput, TOutput>(string? serviceName, TInput input, JsonTypeInfo<TInput> inputJsonTypeInfo, JsonTypeInfo<TOutput> outputJsonTypeInfo, Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null, CancellationToken cancellationToken = default) where TOutput : class
        {
            string inputJson = JsonSerializer.Serialize<TInput>(input, inputJsonTypeInfo);
            return Task.FromResult<TOutput?>(JsonSerializer.Deserialize<TOutput>(inputJson, outputJsonTypeInfo));
        }

        public Task<TOutput?> GetForUserAsync<TOutput>(string? serviceName, JsonTypeInfo<TOutput> outputJsonTypeInfo, Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null, ClaimsPrincipal? user = null, CancellationToken cancellationToken = default) where TOutput : class
        {
            return Task.FromResult<TOutput?>(JsonSerializer.Deserialize<TOutput>(JsonResponse, outputJsonTypeInfo));
        }

        public Task<TOutput?> GetForUserAsync<TInput, TOutput>(string? serviceName, TInput input, JsonTypeInfo<TInput> inputJsonTypeInfo, JsonTypeInfo<TOutput> outputJsonTypeInfo, Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null, ClaimsPrincipal? user = null, CancellationToken cancellationToken = default) where TOutput : class
        {
            string inputJson = JsonSerializer.Serialize(input, inputJsonTypeInfo);
            return Task.FromResult<TOutput?>(JsonSerializer.Deserialize<TOutput>(inputJson, outputJsonTypeInfo));
        }

        public Task PostForAppAsync<TInput>(string? serviceName, TInput input, JsonTypeInfo<TInput> inputJsonTypeInfo, Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null, CancellationToken cancellationToken = default)
        {
            JsonSerializer.Serialize(input, inputJsonTypeInfo);
            return Task.CompletedTask;
        }

        public Task<TOutput?> PostForAppAsync<TInput, TOutput>(string? serviceName, TInput input, JsonTypeInfo<TInput> inputJsonTypeInfo, JsonTypeInfo<TOutput> outputJsonTypeInfo, Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null, CancellationToken cancellationToken = default) where TOutput : class
        {
            string inputJson = JsonSerializer.Serialize(input, inputJsonTypeInfo);
            return Task.FromResult<TOutput?>(JsonSerializer.Deserialize<TOutput>(inputJson, outputJsonTypeInfo));
        }

        public Task PostForUserAsync<TInput>(string? serviceName, TInput input, JsonTypeInfo<TInput> inputJsonTypeInfo, Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null, ClaimsPrincipal? user = null, CancellationToken cancellationToken = default)
        {
            JsonSerializer.Serialize(input, inputJsonTypeInfo);
            return Task.CompletedTask;
        }

        public Task<TOutput?> PostForUserAsync<TInput, TOutput>(string? serviceName, TInput input, JsonTypeInfo<TInput> inputJsonTypeInfo, JsonTypeInfo<TOutput> outputJsonTypeInfo, Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null, ClaimsPrincipal? user = null, CancellationToken cancellationToken = default) where TOutput : class
        {
            string inputJson = JsonSerializer.Serialize(input, inputJsonTypeInfo);
            return Task.FromResult<TOutput?>(JsonSerializer.Deserialize<TOutput>(inputJson, outputJsonTypeInfo));
        }

        public Task PutForAppAsync<TInput>(string? serviceName, TInput input, JsonTypeInfo<TInput> inputJsonTypeInfo, Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null, CancellationToken cancellationToken = default)
        {
            JsonSerializer.Serialize(input, inputJsonTypeInfo);
            return Task.CompletedTask;
        }

        public Task<TOutput?> PutForAppAsync<TInput, TOutput>(string? serviceName, TInput input, JsonTypeInfo<TInput> inputJsonTypeInfo, JsonTypeInfo<TOutput> outputJsonTypeInfo, Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null, CancellationToken cancellationToken = default) where TOutput : class
        {
            string inputJson = JsonSerializer.Serialize(input, inputJsonTypeInfo);
            return Task.FromResult<TOutput?>(JsonSerializer.Deserialize<TOutput>(inputJson, outputJsonTypeInfo));
        }

        public Task PutForUserAsync<TInput>(string? serviceName, TInput input, JsonTypeInfo<TInput> inputJsonTypeInfo, Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null, ClaimsPrincipal? user = null, CancellationToken cancellationToken = default)
        {
            JsonSerializer.Serialize(input, inputJsonTypeInfo);
            return Task.CompletedTask;
        }

        public Task<TOutput?> PutForUserAsync<TInput, TOutput>(string? serviceName, TInput input, JsonTypeInfo<TInput> inputJsonTypeInfo, JsonTypeInfo<TOutput> outputJsonTypeInfo, Action<DownstreamApiOptionsReadOnlyHttpMethod>? downstreamApiOptionsOverride = null, ClaimsPrincipal? user = null, CancellationToken cancellationToken = default) where TOutput : class
        {
            string inputJson = JsonSerializer.Serialize(input, inputJsonTypeInfo);
            return Task.FromResult<TOutput?>(JsonSerializer.Deserialize<TOutput>(inputJson, outputJsonTypeInfo));
        }
    }
}
#endif
