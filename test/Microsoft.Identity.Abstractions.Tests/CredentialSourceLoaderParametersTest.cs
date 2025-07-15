// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Xunit;

namespace Microsoft.Identity.Abstractions.ApplicationOptions.Tests
{
    public class CredentialSourceLoaderParametersTest
    {
        const string ClientId = "ClientId-from-app-registration";
        const string Authority = "https://login.microsoftonline.com/common/v2.0";
        
        [Fact]
        public void CredentialSourceLoaderParametersProperties()
        {
            CredentialSourceLoaderParameters parameters = new(ClientId, Authority);

            Assert.Equal(ClientId, parameters.ClientId);
            Assert.Equal(Authority, parameters.Authority);
            Assert.NotNull(parameters.ClientCapabilities);
            Assert.Empty(parameters.ClientCapabilities);
        }

        [Fact]
        public void CredentialSourceLoaderParametersProperties_WithCapabilities()
        {
            // Arrange
            var caps = new[] { "cp1", "llt" };

            // Act
            CredentialSourceLoaderParameters parameters = new(ClientId, Authority, caps);

            // Assert
            Assert.Equal(ClientId, parameters.ClientId);
            Assert.Equal(Authority, parameters.Authority);
            Assert.Equal(caps, parameters.ClientCapabilities);
        }
    }
}
