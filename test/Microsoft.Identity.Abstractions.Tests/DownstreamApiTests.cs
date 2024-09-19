// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xunit;


#if NET8_0_OR_GREATER
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
#endif

namespace Microsoft.Identity.Abstractions.DownstreamApi.Tests
{
    public partial class DownstreamApiTests
    {
        [Fact]
        public void CloneClonesAllProperties()
        {
            var downstreamApiOptions = new DownstreamApiOptions
            {
                Scopes = new[] { "https://apitocall.domain.com/read" },
                AcquireTokenOptions = new AcquireTokenOptions
                {
                    AuthenticationOptionsName = "AzureAd",
                    Claims = "claims",
                    CorrelationId = Guid.NewGuid(),
                    ExtraHeadersParameters = new Dictionary<string, string> { { "slice", "test" } },
                    ExtraQueryParameters = new Dictionary<string, string> { { "slice", "test" } },
                    ExtraParameters = new Dictionary<string, object> { { "param1", "value1" }, { "param2", "value2" } },
                    ForceRefresh = true,
                    LongRunningWebApiSessionKey = AcquireTokenOptions.LongRunningWebApiSessionKeyAuto,
                    ManagedIdentity = new ManagedIdentityOptions(),
                    PopPublicKey = "PopKey",
                    PopClaim = "jwkClaim",
                    Tenant = "domain.com",
                    UserFlow = "susi"

                },
                BaseUrl = "https://apitocall.domain.com",
                CustomizeHttpRequestMessage = message => message.Headers.Add("x-sku", "sku-value"),
                Deserializer = value => value,
                Serializer = input => (input != null) ? new StringContent(input.ToString()!, Encoding.UTF8
#if !NETFRAMEWORK
                                                           , new MediaTypeHeaderValue("text/json")
#endif
                )
                                                      : null,
                HttpMethod = HttpMethod.Trace.ToString(),
                ProtocolScheme = "bearer",
                RelativePath = "/api/values",
                RequestAppToken = true
            };

            Assert.Equal("https://apitocall.domain.com/api/values", downstreamApiOptions.GetApiUrl());

            // Get a clone for an authorization header provider
            AuthorizationHeaderProviderOptions authorizationHeaderProviderOptions = new(downstreamApiOptions);
            AuthorizationHeaderProviderOptions authorizationHeaderProviderOptionsClone = authorizationHeaderProviderOptions.Clone();

            // Clone
            DownstreamApiOptions downstreamApiClone = downstreamApiOptions.Clone();

            Assert.NotNull(downstreamApiClone);
            Assert.Equal(downstreamApiOptions.Scopes, downstreamApiClone.Scopes!);
            Assert.Equal(downstreamApiOptions.BaseUrl, downstreamApiClone.BaseUrl);
            Assert.Equal(downstreamApiOptions.CustomizeHttpRequestMessage, downstreamApiClone.CustomizeHttpRequestMessage);
            Assert.Equal(downstreamApiOptions.Deserializer, downstreamApiClone.Deserializer);
            Assert.Equal(downstreamApiOptions.Serializer, downstreamApiClone.Serializer);
            Assert.Equal(downstreamApiOptions.HttpMethod, downstreamApiClone.HttpMethod);
            Assert.Equal(downstreamApiOptions.ProtocolScheme, downstreamApiClone.ProtocolScheme);
            Assert.Equal(downstreamApiOptions.RelativePath, downstreamApiClone.RelativePath);
            Assert.Equal(downstreamApiOptions.RequestAppToken, downstreamApiClone.RequestAppToken);

            Assert.Equal(downstreamApiOptions.AcquireTokenOptions.AuthenticationOptionsName, downstreamApiClone.AcquireTokenOptions.AuthenticationOptionsName);
            Assert.Equal(downstreamApiOptions.AcquireTokenOptions.Claims, downstreamApiClone.AcquireTokenOptions.Claims);
            Assert.Equal(downstreamApiOptions.AcquireTokenOptions.CorrelationId, downstreamApiClone.AcquireTokenOptions.CorrelationId);
            Assert.Equal(downstreamApiOptions.AcquireTokenOptions.ExtraHeadersParameters, downstreamApiClone.AcquireTokenOptions.ExtraHeadersParameters);
            Assert.Equal(downstreamApiOptions.AcquireTokenOptions.ExtraQueryParameters, downstreamApiClone.AcquireTokenOptions.ExtraQueryParameters);
            Assert.Equal(downstreamApiOptions.AcquireTokenOptions.ExtraParameters, downstreamApiClone.AcquireTokenOptions.ExtraParameters);
            Assert.Equal(downstreamApiOptions.AcquireTokenOptions.ForceRefresh, downstreamApiClone.AcquireTokenOptions.ForceRefresh);
            Assert.Equal(downstreamApiOptions.AcquireTokenOptions.LongRunningWebApiSessionKey, downstreamApiClone.AcquireTokenOptions.LongRunningWebApiSessionKey);
            Assert.Equal(downstreamApiOptions.AcquireTokenOptions.ManagedIdentity.UserAssignedClientId, downstreamApiClone.AcquireTokenOptions.ManagedIdentity?.UserAssignedClientId);
            Assert.Equal(downstreamApiOptions.AcquireTokenOptions.PopPublicKey, downstreamApiClone.AcquireTokenOptions.PopPublicKey);
            Assert.Equal(downstreamApiOptions.AcquireTokenOptions.PopClaim, downstreamApiClone.AcquireTokenOptions.PopClaim);
            Assert.Equal(downstreamApiOptions.AcquireTokenOptions.Tenant, downstreamApiClone.AcquireTokenOptions.Tenant);
            Assert.Equal(downstreamApiOptions.AcquireTokenOptions.UserFlow, downstreamApiClone.AcquireTokenOptions.UserFlow);
            Assert.Equal("application/json", downstreamApiClone.AcceptHeader);
            Assert.Equal("application/json", downstreamApiClone.ContentType);

            // If this fails, think of also adding a line to test the new property
            Assert.Equal(12, typeof(DownstreamApiOptions).GetProperties().Length);
            Assert.Equal(14, typeof(AcquireTokenOptions).GetProperties().Length);

            DownstreamApiOptionsReadOnlyHttpMethod options = new DownstreamApiOptionsReadOnlyHttpMethod(downstreamApiOptions, HttpMethod.Delete.ToString());
            Assert.Equal(HttpMethod.Delete.ToString(), options.HttpMethod);
            Assert.Equal(HttpMethod.Delete.ToString(), options.Clone().HttpMethod);

            // Special cases
            authorizationHeaderProviderOptions.HttpMethod = null!!;
            authorizationHeaderProviderOptions.ProtocolScheme = null!!;
            Assert.Equal("Get", authorizationHeaderProviderOptions.HttpMethod);
            Assert.Equal("Bearer", authorizationHeaderProviderOptions.ProtocolScheme);
        }

        [Fact]
        public void CloneNull()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new CustomAcquireTokenOptions(null!));
            Assert.Throws<ArgumentNullException>(() => _ = new AuthorizationHeaderProviderOptions(null!));
        }

        [Fact]
        public void DisallowedNullMembers()
        {
            AuthorizationHeaderProviderOptions authenticationHeaderProviderOptions = new();

            Assert.Throws<ArgumentNullException>(() => _ = authenticationHeaderProviderOptions.AcquireTokenOptions = null!);
        }

        [Fact]
        public void ExerciseApi()
        {
            IDownstreamApi downstreamApi = new CustomDownstreamApi();

            // Call a service based on the configuration only. The name "service" maps to a 
            downstreamApi.CallApiAsync("service");

            // Calls a service based on the programmatic description only.
            downstreamApi.CallApiAsync(null,
                options =>
                {
                    options.HttpMethod = HttpMethod.Get.ToString();
                    options.BaseUrl = "https://monApi.domain.com";
                    options.RelativePath = "api/values";
                });

            // Calls a service purely programmatically. 
            downstreamApi.CallApiAsync(new DownstreamApiOptions { HttpMethod = HttpMethod.Get.ToString(), RequestAppToken = false });

            // In the following call, it's not possible to set the HttpMethod in the delegate, as it would no
            // make sense: it's already provided in the name of the method
            // The following code does not build (on purpose):
            // downstreamApi.DeleteForAppAsync("serviceName", "todo", options => { options.HttpMethod = HttpMethod.Put });
        }


        [Theory]
        [InlineData("https://myapi/", "controller/action", "https://myapi/controller/action")]
        [InlineData("https://myapi", "controller/action", "https://myapi/controller/action")]
        [InlineData("https://myapi/", "/controller/action", "https://myapi/controller/action")]
        [InlineData("https://myapi", "/controller/action", "https://myapi/controller/action")]
        [InlineData("https://myapi", null, "https://myapi/")]
        [InlineData(null, "/controller/action", "/controller/action")]
        [InlineData(null, "controller/action", "/controller/action")]
        public void ComputeUrl(string baseUrl, string relativePath, string expectedUrl)
        {
            DownstreamApiOptions options = new DownstreamApiOptions { BaseUrl = baseUrl, RelativePath = relativePath };
            Assert.Equal(expectedUrl, options.GetApiUrl());
        }

#if NET8_0_OR_GREATER
        internal class CustomApiResponse
        {
            public int ErrorCode { get; set; }
        }

        internal class CustomApiInput
        {
            public int ErrorCode { get; set; }
        }

        [JsonSerializable(typeof(CustomApiInput))]
        [JsonSerializable(typeof(CustomApiResponse))]
        internal partial class CustomApiResponseJsonContext : JsonSerializerContext
        {
        }

        [Fact]
        public void ExerciseApiAotOverloads()
        {
            // Test that the AOT Json serialzation overloads work

            IDownstreamApi downstreamApi = new CustomDownstreamApi();
            CustomApiResponse? response = downstreamApi.CallApiForUserAsync<CustomApiResponse>("service", CustomApiResponseJsonContext.Default.CustomApiResponse).Result;
            Assert.Equal(200, response?.ErrorCode);

            CustomApiInput input = new CustomApiInput { ErrorCode = 123 };
            response = downstreamApi.GetForAppAsync<CustomApiInput, CustomApiResponse>("service", input, CustomApiResponseJsonContext.Default.CustomApiInput, CustomApiResponseJsonContext.Default.CustomApiResponse).Result;
            Assert.Equal(123, response?.ErrorCode);
        }
#endif
    }

    internal class CustomAcquireTokenOptions : AcquireTokenOptions
    {
        public CustomAcquireTokenOptions() : base() { }

        public CustomAcquireTokenOptions(CustomAcquireTokenOptions other) : base(other) { }
    }
}
