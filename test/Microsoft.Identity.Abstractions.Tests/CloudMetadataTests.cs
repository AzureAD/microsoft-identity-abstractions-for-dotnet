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
        public void InMemoryProvider_PerKeyOverride_OverFallback_Wins_AndKeepsOtherKeys()
        {
            // Arrange: fallback carries two keys for a host.
            var fallback = new InMemoryCloudMetadataProvider().AddOrUpdate(
                "login.microsoftonline.us",
                new Dictionary<string, string>
                {
                    [AbstractionsCloudKeys.TokenExchangeAudience] = "api://AzureADTokenExchangeUSGov",
                    ["other_key"] = "keep-me",
                });
            var provider = new InMemoryCloudMetadataProvider(fallback)
                .AddOrUpdate("login.microsoftonline.us", AbstractionsCloudKeys.TokenExchangeAudience, "api://custom");

            // Act
            CloudMetadata? metadata = provider.GetByAuthorityHost("login.microsoftonline.us");

            // Assert: own key wins, fallback's other key is preserved.
            Assert.Equal("api://custom", metadata!.GetValueOrDefault(AbstractionsCloudKeys.TokenExchangeAudience));
            Assert.Equal("keep-me", metadata.GetValueOrDefault("other_key"));
        }

        [Fact]
        public void InMemoryProvider_PerKeyAdd_KeepsFallbackKeys()
        {
            // Arrange: fallback has the audience; caller adds an unrelated key over it.
            var fallback = new InMemoryCloudMetadataProvider().AddOrUpdate(
                "login.microsoftonline.us",
                new Dictionary<string, string>
                {
                    [AbstractionsCloudKeys.TokenExchangeAudience] = "api://AzureADTokenExchangeUSGov",
                });
            var provider = new InMemoryCloudMetadataProvider(fallback)
                .AddOrUpdate("login.microsoftonline.us", "extra_key", "extra-value");

            // Act
            CloudMetadata? metadata = provider.GetByAuthorityHost("login.microsoftonline.us");

            // Assert: both the fallback key and the added key resolve.
            Assert.Equal("api://AzureADTokenExchangeUSGov", metadata!.GetValueOrDefault(AbstractionsCloudKeys.TokenExchangeAudience));
            Assert.Equal("extra-value", metadata.GetValueOrDefault("extra_key"));
        }

        [Fact]
        public void InMemoryProvider_PerKey_MergesWithPriorValues_ForSameHost()
        {
            // Arrange & Act: accumulate two distinct keys via two per-key calls (no fallback).
            var provider = new InMemoryCloudMetadataProvider()
                .AddOrUpdate("login.partner.example", AbstractionsCloudKeys.TokenExchangeAudience, "api://first")
                .AddOrUpdate("login.partner.example", "second_key", "second-value");

            // Assert
            CloudMetadata? metadata = provider.GetByAuthorityHost("login.partner.example");
            Assert.Equal("api://first", metadata!.GetValueOrDefault(AbstractionsCloudKeys.TokenExchangeAudience));
            Assert.Equal("second-value", metadata.GetValueOrDefault("second_key"));
        }

        [Fact]
        public void InMemoryProvider_PerKey_NullArguments_Throw()
        {
            // Arrange
            var provider = new InMemoryCloudMetadataProvider();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => provider.AddOrUpdate(null!, "key", "value"));
            Assert.Throws<ArgumentNullException>(() => provider.AddOrUpdate("host", null!, "value"));
            Assert.Throws<ArgumentNullException>(() => provider.AddOrUpdate("host", "key", null!));
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

        [Fact]
        public void CloudMetadata_Values_ExposesAllPairs_CaseInsensitively()
        {
            // Arrange
            var metadata = new CloudMetadata(new Dictionary<string, string>
            {
                [AbstractionsCloudKeys.TokenExchangeAudience] = "api://AzureADTokenExchangeUSGov",
                ["other_key"] = "other-value",
            });

            // Act & Assert: the read-only Values view exposes every stored pair, keyed case-insensitively.
            Assert.Equal(2, metadata.Values.Count);
            Assert.Equal("api://AzureADTokenExchangeUSGov", metadata.Values[AbstractionsCloudKeys.TokenExchangeAudience]);
            Assert.Equal("other-value", metadata.Values["OTHER_KEY"]);
        }

        [Fact]
        public void InMemoryProvider_NullOrEmptyHost_WithoutFallback_ReturnsNull()
        {
            // Arrange
            var provider = new InMemoryCloudMetadataProvider().AddOrUpdate(
                "login.microsoftonline.us",
                new Dictionary<string, string>
                {
                    [AbstractionsCloudKeys.TokenExchangeAudience] = "api://AzureADTokenExchangeUSGov",
                });

            // Act & Assert: a null or empty host resolves to null rather than throwing.
            Assert.Null(provider.GetByAuthorityHost(null!));
            Assert.Null(provider.GetByAuthorityHost(string.Empty));
        }
    }
}
