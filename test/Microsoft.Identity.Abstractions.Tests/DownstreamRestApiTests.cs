// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Xunit;

namespace Microsoft.Identity.Abstractions.DownstreamRestApi.Tests
{
    public class DownstreamRestApiTests
    {
        [Fact]
        public void CloneClonesAllProperties()
        {
            var downstreamRestApiOptions = new DownstreamRestApiOptions
            {
                Scopes = new[] { "https://apitocall.domain.com/read" },
                AcquireTokenOptions = new AcquireTokenOptions
                {
                    AuthenticationOptionsName = "AzureAd",
                    Claims = "claims",
                    CorrelationId = Guid.NewGuid(),
                    ExtraHeadersParameters = new Dictionary<string, string> { { "slice", "test" } },
                    ExtraQueryParameters = new Dictionary<string, string> { { "slice", "test" } },
                    ForceRefresh = true,
                    LongRunningWebApiSessionKey = AcquireTokenOptions.LongRunningWebApiSessionKeyAuto,
                    PopPublicKey = "PopKey",
                    Tenant = "domain.com",
                    UserFlow = "susi"

                },
                BaseUrl = "https://apitocall.domain.com",
                CustomizeHttpRequestMessage = message => message.Headers.Add("x-sku", "sku-value"),
                Deserializer = value => value,
                Serializer = input => (input != null) ? new StringContent(input.ToString()!, new MediaTypeHeaderValue("text/json", "UTF8")) : null,
                HttpMethod = HttpMethod.Trace,
                ProtocolScheme = "bearer",
                RelativePath = "/api/values",
                RequestAppToken = true
            };

            Assert.Equal("https://apitocall.domain.com/api/values", downstreamRestApiOptions.GetApiUrl());

            // Get a clone for an authorization header provider
            AuthorizationHeaderProviderOptions authorizationHeaderProviderOptions = new(downstreamRestApiOptions);
            AuthorizationHeaderProviderOptions authorizationHeaderProviderOptionsClone = authorizationHeaderProviderOptions.Clone();

            // Clone
            DownstreamRestApiOptions downstreamRestApiClone = downstreamRestApiOptions.Clone();

            Assert.NotNull(downstreamRestApiClone);
            Assert.Equal(downstreamRestApiOptions.Scopes, downstreamRestApiClone.Scopes!);
            Assert.Equal(downstreamRestApiOptions.BaseUrl, downstreamRestApiClone.BaseUrl);
            Assert.Equal(downstreamRestApiOptions.CustomizeHttpRequestMessage, downstreamRestApiClone.CustomizeHttpRequestMessage);
            Assert.Equal(downstreamRestApiOptions.Deserializer, downstreamRestApiClone.Deserializer);
            Assert.Equal(downstreamRestApiOptions.Serializer, downstreamRestApiClone.Serializer);
            Assert.Equal(downstreamRestApiOptions.HttpMethod, downstreamRestApiClone.HttpMethod);
            Assert.Equal(downstreamRestApiOptions.ProtocolScheme, downstreamRestApiClone.ProtocolScheme);
            Assert.Equal(downstreamRestApiOptions.RelativePath, downstreamRestApiClone.RelativePath);
            Assert.Equal(downstreamRestApiOptions.RequestAppToken, downstreamRestApiClone.RequestAppToken);

            Assert.Equal(downstreamRestApiOptions.AcquireTokenOptions.AuthenticationOptionsName, downstreamRestApiClone.AcquireTokenOptions.AuthenticationOptionsName);
            Assert.Equal(downstreamRestApiOptions.AcquireTokenOptions.Claims, downstreamRestApiClone.AcquireTokenOptions.Claims);
            Assert.Equal(downstreamRestApiOptions.AcquireTokenOptions.CorrelationId, downstreamRestApiClone.AcquireTokenOptions.CorrelationId);
            Assert.Equal(downstreamRestApiOptions.AcquireTokenOptions.ExtraHeadersParameters, downstreamRestApiClone.AcquireTokenOptions.ExtraHeadersParameters);
            Assert.Equal(downstreamRestApiOptions.AcquireTokenOptions.ExtraQueryParameters, downstreamRestApiClone.AcquireTokenOptions.ExtraQueryParameters);
            Assert.Equal(downstreamRestApiOptions.AcquireTokenOptions.ForceRefresh, downstreamRestApiClone.AcquireTokenOptions.ForceRefresh);
            Assert.Equal(downstreamRestApiOptions.AcquireTokenOptions.LongRunningWebApiSessionKey, downstreamRestApiClone.AcquireTokenOptions.LongRunningWebApiSessionKey);
            Assert.Equal(downstreamRestApiOptions.AcquireTokenOptions.PopPublicKey, downstreamRestApiClone.AcquireTokenOptions.PopPublicKey);
            Assert.Equal(downstreamRestApiOptions.AcquireTokenOptions.Tenant, downstreamRestApiClone.AcquireTokenOptions.Tenant);
            Assert.Equal(downstreamRestApiOptions.AcquireTokenOptions.UserFlow, downstreamRestApiClone.AcquireTokenOptions.UserFlow);

            // If this fails, think of also adding a line to test the new property
            Assert.Equal(10, typeof(DownstreamRestApiOptions).GetProperties().Length);
            Assert.Equal(11, typeof(AcquireTokenOptions).GetProperties().Length);

            DownstreamRestApiOptionsReadOnlyHttpMethod options = new DownstreamRestApiOptionsReadOnlyHttpMethod(downstreamRestApiOptions, HttpMethod.Delete);
            Assert.Equal(HttpMethod.Delete, options.HttpMethod);
            Assert.Equal(HttpMethod.Delete, options.Clone().HttpMethod);
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

            Assert.Throws<ArgumentNullException>(() => _ = authenticationHeaderProviderOptions.ProtocolScheme = null!); ;
            Assert.Throws<ArgumentNullException>(() => _ = authenticationHeaderProviderOptions.AcquireTokenOptions = null!);
        }

        [Fact]
        public void ExerciseApi()
        {
            IDownstreamRestApi downstreamRestApi = new CustomDownstreamRestApi();

            // Call a service based on the configuration only. The name "service" maps to a 
            downstreamRestApi.CallRestApiAsync("service");

            // Calls a service based on the programmatic description only.
            downstreamRestApi.CallRestApiAsync(null,
                options =>
                {
                    options.HttpMethod = HttpMethod.Get;
                    options.BaseUrl = "https://monApi.domain.com";
                    options.RelativePath = "api/values";
                });

            // Calls a service purely programmatically. 
            downstreamRestApi.CallRestApiAsync(new DownstreamRestApiOptions { HttpMethod = HttpMethod.Get, RequestAppToken = false });

            // In the following call, it's not possible to set the HttpMethod in the delegate, as it would no
            // make sense: it's already provided in the name of the method
            // The following code does not build (on purpose):
            // downstreamRestApi.DeleteForAppAsync("serviceName", "todo", options => { options.HttpMethod = HttpMethod.Put });
        }
    }

    internal class CustomAcquireTokenOptions : AcquireTokenOptions
    {
        public CustomAcquireTokenOptions() : base() { }

        public CustomAcquireTokenOptions(CustomAcquireTokenOptions other) : base(other) { }
    }
}