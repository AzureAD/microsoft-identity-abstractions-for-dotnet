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
        public void DefaultConstructor_InitializesEmptyProperties()
        {
            // Arrange & Act
            CredentialSourceLoaderParameters parameters = new();

            // Assert
            Assert.Equal(string.Empty, parameters.ClientId);
            Assert.Equal(string.Empty, parameters.Authority);
            Assert.Equal(string.Empty, parameters.Protocol);
            Assert.Equal(string.Empty, parameters.ApiUrl);
        }

        [Fact]
        public void ParameterizedConstructor_InitializesClientIdAndAuthority()
        {
            // Arrange & Act
            CredentialSourceLoaderParameters parameters = new(clientId, authority);

            // Assert
            Assert.Equal(clientId, parameters.ClientId);
            Assert.Equal(authority, parameters.Authority);
            Assert.Equal(string.Empty, parameters.Protocol);
            Assert.Equal(string.Empty, parameters.ApiUrl);
        }

        [Fact]
        public void Properties_CanBeSetIndividually()
        {
            // Arrange
            CredentialSourceLoaderParameters parameters = new();

            // Act
            parameters.ClientId = clientId;
            parameters.Authority = authority;
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
