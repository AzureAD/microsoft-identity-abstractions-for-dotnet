// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Xunit;

namespace Microsoft.Identity.Abstractions.ApplicationOptions.Tests
{
    public class CredentialSourceLoaderParametersTest
    {
        const string clientId = "ClientId-from-app-registration";
        const string authority = "https://login.microsoftonline.com/common/v2.0";
        const string protocol = "Bearer";
        const string apiUrl = "https://api.example.com/v1";
        
        [Fact]
        public void ParameterizedConstructor_InitializesClientIdAndAuthority()
        {
            // Arrange & Act
            CredentialSourceLoaderParameters parameters = new(clientId, authority);

            // Assert
            Assert.Equal(clientId, parameters.ClientId);
            Assert.Equal(authority, parameters.Authority);
            Assert.Null(parameters.Protocol);
            Assert.Null(parameters.ApiUrl);
        }

        [Fact]
        public void Properties_CanBeSetIndividually()
        {
            // Arrange & Act
            CredentialSourceLoaderParameters parameters = new(clientId, authority);
            parameters.Protocol = protocol;
            parameters.ApiUrl = apiUrl;

            // Assert
            Assert.Equal(clientId, parameters.ClientId);
            Assert.Equal(authority, parameters.Authority);
            Assert.Equal(protocol, parameters.Protocol);
            Assert.Equal(apiUrl, parameters.ApiUrl);
        }
    }
}
