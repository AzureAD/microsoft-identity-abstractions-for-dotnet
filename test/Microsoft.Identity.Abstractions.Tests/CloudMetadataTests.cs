// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Xunit;

namespace Microsoft.Identity.Abstractions.Tests
{
    public class CloudMetadataTests
    {
        [Fact]
        public void CloudMetadata_CopiesValues_AndIsCaseInsensitive()
        {
            // Arrange
            var source = new Dictionary<string, string>
            {
                [AbstractionsCloudKeys.TokenExchangeAudience] = "api://AzureADTokenExchangeUSGov",
            };

            // Act
            var metadata = new CloudMetadata(source);
            source[AbstractionsCloudKeys.TokenExchangeAudience] = "mutated"; // must not affect the copy

            // Assert
            Assert.Equal("api://AzureADTokenExchangeUSGov", metadata.GetValueOrDefault(AbstractionsCloudKeys.TokenExchangeAudience));
            Assert.Equal("api://AzureADTokenExchangeUSGov", metadata.GetValueOrDefault("TOKEN_EXCHANGE_AUDIENCE"));
            Assert.True(metadata.TryGetValue(AbstractionsCloudKeys.TokenExchangeAudience, out string? value));
            Assert.Equal("api://AzureADTokenExchangeUSGov", value);
        }

        [Fact]
        public void CloudMetadata_MissingKey_ReturnsNullOrFalse()
        {
            // Arrange
            var metadata = new CloudMetadata(new Dictionary<string, string>());

            // Act & Assert
            Assert.Null(metadata.GetValueOrDefault(AbstractionsCloudKeys.TokenExchangeAudience));
            Assert.False(metadata.TryGetValue(AbstractionsCloudKeys.TokenExchangeAudience, out string? value));
            Assert.Null(value);
        }

        [Fact]
        public void CloudMetadata_NullValues_Throws()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CloudMetadata(null!));
        }

        [Fact]
        public void InMemoryProvider_ReturnsAddedMetadata_ByHost_CaseInsensitive()
        {
            // Arrange
            var provider = new InMemoryCloudMetadataProvider().AddOrUpdate(
                "login.microsoftonline.us",
                new Dictionary<string, string>
                {
                    [AbstractionsCloudKeys.TokenExchangeAudience] = "api://AzureADTokenExchangeUSGov",
                });

            // Act
            CloudMetadata? metadata = provider.GetByAuthorityHost("LOGIN.MICROSOFTONLINE.US");

            // Assert
            Assert.NotNull(metadata);
            Assert.Equal("api://AzureADTokenExchangeUSGov", metadata!.GetValueOrDefault(AbstractionsCloudKeys.TokenExchangeAudience));
        }

        [Fact]
        public void InMemoryProvider_AddOrUpdate_Overwrites_AndChains()
        {
            // Arrange & Act
            var provider = new InMemoryCloudMetadataProvider()
                .AddOrUpdate("login.partner.example", new Dictionary<string, string>
                {
                    [AbstractionsCloudKeys.TokenExchangeAudience] = "api://first",
                })
                .AddOrUpdate("login.partner.example", new Dictionary<string, string>
                {
                    [AbstractionsCloudKeys.TokenExchangeAudience] = "api://second",
                });

            // Assert
            Assert.Equal("api://second", provider.GetByAuthorityHost("login.partner.example")!.GetValueOrDefault(AbstractionsCloudKeys.TokenExchangeAudience));
        }

        [Fact]
        public void InMemoryProvider_UnknownHost_FallsBackToFallbackProviderThenNull()
        {
            // Arrange
            var fallback = new InMemoryCloudMetadataProvider().AddOrUpdate(
                "login.microsoftonline.com",
                new Dictionary<string, string>
                {
                    [AbstractionsCloudKeys.TokenExchangeAudience] = "api://AzureADTokenExchange",
                });
            var provider = new InMemoryCloudMetadataProvider(fallback);

            // Act & Assert: own entries are empty, so it defers to the fallback, then null for truly unknown.
            Assert.Equal("api://AzureADTokenExchange", provider.GetByAuthorityHost("login.microsoftonline.com")!.GetValueOrDefault(AbstractionsCloudKeys.TokenExchangeAudience));
            Assert.Null(provider.GetByAuthorityHost("login.unknown.example"));
        }

        [Fact]
        public void InMemoryProvider_NullArguments_Throw()
        {
            // Arrange
            var provider = new InMemoryCloudMetadataProvider();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => provider.AddOrUpdate(null!, new Dictionary<string, string>()));
            Assert.Throws<ArgumentNullException>(() => provider.AddOrUpdate("host", null!));
        }
    }
}
