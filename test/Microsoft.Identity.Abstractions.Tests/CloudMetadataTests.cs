// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Xunit;

namespace Microsoft.Identity.Abstractions.Tests
{
    public class CloudMetadataTests
    {
        private static string? ValueOrNull(IReadOnlyDictionary<string, string>? bag, string key)
        {
            return bag is not null && bag.TryGetValue(key, out string? value) ? value : null;
        }

        [Fact]
        public void InMemoryProvider_ReturnsAddedMetadata_ByHost_CaseInsensitive()
        {
            // Arrange
            var provider = new InMemoryCloudMetadataProvider().AddOrUpdate(
                "login.microsoftonline.us",
                new Dictionary<string, string>
                {
                    [CloudMetadataKeyNames.FederatedCredentialAudience] = "api://AzureADTokenExchangeUSGov",
                });

            // Act
            IReadOnlyDictionary<string, string>? metadata = provider.GetByAuthorityHost("LOGIN.MICROSOFTONLINE.US");

            // Assert
            Assert.NotNull(metadata);
            Assert.Equal("api://AzureADTokenExchangeUSGov", ValueOrNull(metadata, CloudMetadataKeyNames.FederatedCredentialAudience));
        }

        [Fact]
        public void InMemoryProvider_AddOrUpdate_Overwrites_AndChains()
        {
            // Arrange & Act
            var provider = new InMemoryCloudMetadataProvider()
                .AddOrUpdate("login.partner.example", new Dictionary<string, string>
                {
                    [CloudMetadataKeyNames.FederatedCredentialAudience] = "api://first",
                })
                .AddOrUpdate("login.partner.example", new Dictionary<string, string>
                {
                    [CloudMetadataKeyNames.FederatedCredentialAudience] = "api://second",
                });

            // Assert
            Assert.Equal("api://second", ValueOrNull(provider.GetByAuthorityHost("login.partner.example"), CloudMetadataKeyNames.FederatedCredentialAudience));
        }

        [Fact]
        public void InMemoryProvider_UnknownHost_FallsBackToFallbackProviderThenNull()
        {
            // Arrange
            var fallback = new InMemoryCloudMetadataProvider().AddOrUpdate(
                "login.microsoftonline.com",
                new Dictionary<string, string>
                {
                    [CloudMetadataKeyNames.FederatedCredentialAudience] = "api://AzureADTokenExchange",
                });
            var provider = new InMemoryCloudMetadataProvider(fallback);

            // Act & Assert: own entries are empty, so it defers to the fallback, then null for truly unknown.
            Assert.Equal("api://AzureADTokenExchange", ValueOrNull(provider.GetByAuthorityHost("login.microsoftonline.com"), CloudMetadataKeyNames.FederatedCredentialAudience));
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
                    [CloudMetadataKeyNames.FederatedCredentialAudience] = "api://AzureADTokenExchangeUSGov",
                    ["other_key"] = "keep-me",
                });
            var provider = new InMemoryCloudMetadataProvider(fallback)
                .AddOrUpdate("login.microsoftonline.us", CloudMetadataKeyNames.FederatedCredentialAudience, "api://custom");

            // Act
            IReadOnlyDictionary<string, string>? metadata = provider.GetByAuthorityHost("login.microsoftonline.us");

            // Assert: own key wins, fallback's other key is preserved.
            Assert.Equal("api://custom", ValueOrNull(metadata, CloudMetadataKeyNames.FederatedCredentialAudience));
            Assert.Equal("keep-me", ValueOrNull(metadata, "other_key"));
        }

        [Fact]
        public void InMemoryProvider_PerKeyAdd_KeepsFallbackKeys()
        {
            // Arrange: fallback has the audience; caller adds an unrelated key over it.
            var fallback = new InMemoryCloudMetadataProvider().AddOrUpdate(
                "login.microsoftonline.us",
                new Dictionary<string, string>
                {
                    [CloudMetadataKeyNames.FederatedCredentialAudience] = "api://AzureADTokenExchangeUSGov",
                });
            var provider = new InMemoryCloudMetadataProvider(fallback)
                .AddOrUpdate("login.microsoftonline.us", "extra_key", "extra-value");

            // Act
            IReadOnlyDictionary<string, string>? metadata = provider.GetByAuthorityHost("login.microsoftonline.us");

            // Assert: both the fallback key and the added key resolve.
            Assert.Equal("api://AzureADTokenExchangeUSGov", ValueOrNull(metadata, CloudMetadataKeyNames.FederatedCredentialAudience));
            Assert.Equal("extra-value", ValueOrNull(metadata, "extra_key"));
        }

        [Fact]
        public void InMemoryProvider_PerKey_MergesWithPriorValues_ForSameHost()
        {
            // Arrange & Act: accumulate two distinct keys via two per-key calls (no fallback).
            var provider = new InMemoryCloudMetadataProvider()
                .AddOrUpdate("login.partner.example", CloudMetadataKeyNames.FederatedCredentialAudience, "api://first")
                .AddOrUpdate("login.partner.example", "second_key", "second-value");

            // Assert
            IReadOnlyDictionary<string, string>? metadata = provider.GetByAuthorityHost("login.partner.example");
            Assert.Equal("api://first", ValueOrNull(metadata, CloudMetadataKeyNames.FederatedCredentialAudience));
            Assert.Equal("second-value", ValueOrNull(metadata, "second_key"));
        }

        [Fact]
        public void InMemoryProvider_PerKey_InvalidArguments_Throw()
        {
            // Arrange
            var provider = new InMemoryCloudMetadataProvider();

            // Act & Assert: host and key are rejected for null/empty/whitespace (ArgumentException); a null
            // value is an ArgumentNullException. Kept consistent with the MSAL and MISE twins.
            Assert.Throws<ArgumentException>(() => provider.AddOrUpdate(null!, "key", "value"));
            Assert.Throws<ArgumentException>(() => provider.AddOrUpdate(string.Empty, "key", "value"));
            Assert.Throws<ArgumentException>(() => provider.AddOrUpdate("   ", "key", "value"));
            Assert.Throws<ArgumentException>(() => provider.AddOrUpdate("host", null!, "value"));
            Assert.Throws<ArgumentException>(() => provider.AddOrUpdate("host", string.Empty, "value"));
            Assert.Throws<ArgumentException>(() => provider.AddOrUpdate("host", "   ", "value"));
            Assert.Throws<ArgumentNullException>(() => provider.AddOrUpdate("host", "key", null!));
        }

        [Fact]
        public void InMemoryProvider_InvalidArguments_Throw()
        {
            // Arrange
            var provider = new InMemoryCloudMetadataProvider();

            // Act & Assert: host is rejected for null/empty/whitespace (ArgumentException); a null values
            // bag is an ArgumentNullException.
            Assert.Throws<ArgumentException>(() => provider.AddOrUpdate(null!, new Dictionary<string, string>()));
            Assert.Throws<ArgumentException>(() => provider.AddOrUpdate(string.Empty, new Dictionary<string, string>()));
            Assert.Throws<ArgumentException>(() => provider.AddOrUpdate("   ", new Dictionary<string, string>()));
            Assert.Throws<ArgumentNullException>(() => provider.AddOrUpdate("host", null!));
        }

        [Fact]
        public void InMemoryProvider_NullOrEmptyHost_WithoutFallback_ReturnsNull()
        {
            // Arrange
            var provider = new InMemoryCloudMetadataProvider().AddOrUpdate(
                "login.microsoftonline.us",
                new Dictionary<string, string>
                {
                    [CloudMetadataKeyNames.FederatedCredentialAudience] = "api://AzureADTokenExchangeUSGov",
                });

            // Act & Assert: a null or empty host resolves to null rather than throwing.
            Assert.Null(provider.GetByAuthorityHost(null!));
            Assert.Null(provider.GetByAuthorityHost(string.Empty));
        }

        [Fact]
        public void InMemoryProvider_NullOrEmptyHost_WithFallback_ShortCircuitsToNull()
        {
            // Arrange: a fallback that would resolve any host is present.
            var fallback = new InMemoryCloudMetadataProvider().AddOrUpdate(
                "login.microsoftonline.com",
                new Dictionary<string, string>
                {
                    [CloudMetadataKeyNames.FederatedCredentialAudience] = "api://AzureADTokenExchange",
                });
            var provider = new InMemoryCloudMetadataProvider(fallback);

            // Act & Assert: a null/empty host short-circuits to null and is NOT forwarded to the fallback.
            Assert.Null(provider.GetByAuthorityHost(null!));
            Assert.Null(provider.GetByAuthorityHost(string.Empty));
        }

        [Fact]
        public void InMemoryProvider_ReturnedDictionary_IsCaseInsensitive()
        {
            // Arrange
            var provider = new InMemoryCloudMetadataProvider().AddOrUpdate(
                "login.microsoftonline.us",
                new Dictionary<string, string>
                {
                    [CloudMetadataKeyNames.FederatedCredentialAudience] = "api://AzureADTokenExchangeUSGov",
                    ["other_key"] = "other-value",
                });

            // Act
            IReadOnlyDictionary<string, string>? metadata = provider.GetByAuthorityHost("login.microsoftonline.us");

            // Assert: the returned bag exposes every stored pair, keyed case-insensitively.
            Assert.NotNull(metadata);
            Assert.Equal(2, metadata!.Count);
            Assert.Equal("api://AzureADTokenExchangeUSGov", metadata["FEDERATED_CREDENTIAL_AUDIENCE"]);
            Assert.Equal("other-value", metadata["OTHER_KEY"]);
        }

        [Fact]
        public void InMemoryProvider_ReturnedDictionary_IsReadOnly()
        {
            // Arrange
            var provider = new InMemoryCloudMetadataProvider().AddOrUpdate(
                "login.microsoftonline.us",
                new Dictionary<string, string>
                {
                    [CloudMetadataKeyNames.FederatedCredentialAudience] = "api://AzureADTokenExchangeUSGov",
                });

            // Act
            IReadOnlyDictionary<string, string>? metadata = provider.GetByAuthorityHost("login.microsoftonline.us");

            // Assert: the returned view cannot be downcast to mutate the underlying bag.
            Assert.NotNull(metadata);
            Assert.IsNotType<Dictionary<string, string>>(metadata);
            Assert.Throws<NotSupportedException>(
                () => ((IDictionary<string, string>)metadata!).Add("k", "v"));
        }
    }
}
