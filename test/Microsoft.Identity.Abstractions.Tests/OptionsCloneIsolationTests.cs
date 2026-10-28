// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Xunit;

namespace Microsoft.Identity.Abstractions.DownstreamApi.Tests
{
    /// <summary>
    /// Verifies that cloning options allocates independent copies of the mutable collections, so
    /// that overriding a clone (as done per request by consumers) cannot mutate the collections of
    /// the original options (typically a shared, named-configuration instance).
    /// </summary>
    public class OptionsCloneIsolationTests
    {
        [Theory]
        [InlineData(0, false)]
        [InlineData(0, true)]
        [InlineData(1, false)]
        [InlineData(1, true)]
        [InlineData(2, false)]
        [InlineData(2, true)]
        public void AcquireTokenOptionsCopy_PreservesDictionaryComparers(int dictionaryKind, bool useCopyConstructor)
        {
            // Arrange
            var original = new AcquireTokenOptions
            {
                ExtraParameters = CreateDictionary<object>(dictionaryKind),
                ExtraQueryParameters = CreateDictionary<string>(dictionaryKind),
                ExtraHeadersParameters = CreateDictionary<string>(dictionaryKind),
            };
            original.ExtraParameters["Configured"] = "value";
            original.ExtraQueryParameters["Configured"] = "value";
            original.ExtraHeadersParameters["Configured"] = "value";

            // Act
            AcquireTokenOptions copy = useCopyConstructor ? new AcquireTokenOptions(original) : original.Clone();

            // Assert
            Assert.NotSame(original.ExtraParameters, copy.ExtraParameters);
            Assert.NotSame(original.ExtraQueryParameters, copy.ExtraQueryParameters);
            Assert.NotSame(original.ExtraHeadersParameters, copy.ExtraHeadersParameters);
            Assert.Equal("value", copy.ExtraParameters!["CONFIGURED"]);
            Assert.Equal("value", copy.ExtraQueryParameters!["CONFIGURED"]);
            Assert.Equal("value", copy.ExtraHeadersParameters!["CONFIGURED"]);

            copy.ExtraParameters["CONFIGURED"] = "request";
            copy.ExtraQueryParameters["CONFIGURED"] = "request";
            copy.ExtraHeadersParameters["CONFIGURED"] = "request";
            Assert.Single(copy.ExtraParameters);
            Assert.Single(copy.ExtraQueryParameters);
            Assert.Single(copy.ExtraHeadersParameters);
            Assert.Equal("value", original.ExtraParameters["Configured"]);
            Assert.Equal("value", original.ExtraQueryParameters["Configured"]);
            Assert.Equal("value", original.ExtraHeadersParameters["Configured"]);
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(0, true)]
        [InlineData(1, false)]
        [InlineData(1, true)]
        [InlineData(2, false)]
        [InlineData(2, true)]
        public void DownstreamApiOptionsCopy_PreservesDictionaryComparers(int dictionaryKind, bool useCopyConstructor)
        {
            // Arrange
            var original = new DownstreamApiOptions
            {
                ExtraHeaderParameters = CreateDictionary<string>(dictionaryKind),
                ExtraQueryParameters = CreateDictionary<string>(dictionaryKind),
            };
            original.ExtraHeaderParameters["Configured"] = "value";
            original.ExtraQueryParameters["Configured"] = "value";
            original.AcquireTokenOptions.ExtraParameters = CreateDictionary<object>(dictionaryKind);
            original.AcquireTokenOptions.ExtraParameters["Configured"] = "value";

            // Act
            DownstreamApiOptions copy = useCopyConstructor ? new DownstreamApiOptions(original) : original.Clone();

            // Assert
            Assert.NotSame(original.ExtraHeaderParameters, copy.ExtraHeaderParameters);
            Assert.NotSame(original.ExtraQueryParameters, copy.ExtraQueryParameters);
            Assert.Equal("value", copy.ExtraHeaderParameters!["CONFIGURED"]);
            Assert.Equal("value", copy.ExtraQueryParameters!["CONFIGURED"]);
            Assert.Equal("value", copy.AcquireTokenOptions.ExtraParameters!["CONFIGURED"]);

            copy.ExtraHeaderParameters["CONFIGURED"] = "request";
            copy.ExtraQueryParameters["CONFIGURED"] = "request";
            Assert.Single(copy.ExtraHeaderParameters);
            Assert.Single(copy.ExtraQueryParameters);
            Assert.Equal("value", original.ExtraHeaderParameters["Configured"]);
            Assert.Equal("value", original.ExtraQueryParameters["Configured"]);
        }

        private static IDictionary<string, TValue> CreateDictionary<TValue>(int dictionaryKind)
        {
            return dictionaryKind switch
            {
                0 => new Dictionary<string, TValue>(StringComparer.OrdinalIgnoreCase),
                1 => new SortedDictionary<string, TValue>(StringComparer.OrdinalIgnoreCase),
                2 => new SortedList<string, TValue>(StringComparer.OrdinalIgnoreCase),
                _ => throw new ArgumentOutOfRangeException(nameof(dictionaryKind)),
            };
        }

        [Fact]
        public void AcquireTokenOptionsClone_MutableCollections_AreNewContainers()
        {
            // Arrange
            var original = new AcquireTokenOptions
            {
                ExtraParameters = new Dictionary<string, object> { ["p"] = "v" },
                ExtraQueryParameters = new Dictionary<string, string> { ["q"] = "qv" },
                ExtraHeadersParameters = new Dictionary<string, string> { ["h"] = "hv" },
            };

            // Act
            AcquireTokenOptions clone = original.Clone();

            // Assert - new containers, same content
            Assert.NotSame(original.ExtraParameters, clone.ExtraParameters);
            Assert.NotSame(original.ExtraQueryParameters, clone.ExtraQueryParameters);
            Assert.NotSame(original.ExtraHeadersParameters, clone.ExtraHeadersParameters);
            Assert.Equal(original.ExtraParameters, clone.ExtraParameters);
            Assert.Equal(original.ExtraQueryParameters, clone.ExtraQueryParameters);
            Assert.Equal(original.ExtraHeadersParameters, clone.ExtraHeadersParameters);

            // Assert - mutating the clone does not affect the original
            clone.ExtraParameters!["p2"] = "v2";
            clone.ExtraQueryParameters!["q2"] = "qv2";
            clone.ExtraHeadersParameters!["h2"] = "hv2";
            Assert.Single(original.ExtraParameters!);
            Assert.Single(original.ExtraQueryParameters!);
            Assert.Single(original.ExtraHeadersParameters!);
        }

        [Fact]
        public void AcquireTokenOptionsClone_NullCollections_StayNull()
        {
            // Arrange
            var original = new AcquireTokenOptions();

            // Act
            AcquireTokenOptions clone = original.Clone();

            // Assert
            Assert.Null(clone.ExtraParameters);
            Assert.Null(clone.ExtraQueryParameters);
            Assert.Null(clone.ExtraHeadersParameters);
        }

        [Fact]
        public void AcquireTokenOptionsClone_ClaimsFromClient_PreservesExactRawString()
        {
            // Arrange
            const string claimsFromClient = " { \"access_token\": { \"xms_cc\": { \"values\": [ \"cp1\" ] } } } ";
            var original = new AcquireTokenOptions
            {
                ClaimsFromClient = claimsFromClient,
            };

            // Act
            AcquireTokenOptions clone = original.Clone();

            // Assert
            Assert.Equal(claimsFromClient, clone.ClaimsFromClient);
        }

        [Fact]
        public void DownstreamApiOptionsClone_MutableCollections_AreNewContainers()
        {
            // Arrange
            var original = new DownstreamApiOptions
            {
                ExtraHeaderParameters = new Dictionary<string, string> { ["h"] = "hv" },
                ExtraQueryParameters = new Dictionary<string, string> { ["q"] = "qv" },
            };
            original.AcquireTokenOptions.ExtraParameters = new Dictionary<string, object> { ["p"] = "v" };

            // Act
            DownstreamApiOptions clone = original.Clone();

            // Assert
            Assert.NotSame(original.ExtraHeaderParameters, clone.ExtraHeaderParameters);
            Assert.NotSame(original.ExtraQueryParameters, clone.ExtraQueryParameters);
            Assert.NotSame(original.AcquireTokenOptions.ExtraParameters, clone.AcquireTokenOptions.ExtraParameters);
            Assert.Equal(original.ExtraHeaderParameters, clone.ExtraHeaderParameters);
            Assert.Equal(original.ExtraQueryParameters, clone.ExtraQueryParameters);
            Assert.Equal(original.AcquireTokenOptions.ExtraParameters, clone.AcquireTokenOptions.ExtraParameters);
        }

        [Fact]
        public void AuthorizationHeaderProviderOptionsCopyConstructor_ExtraParameters_IsNewContainer()
        {
            // Arrange
            var original = new AuthorizationHeaderProviderOptions();
            original.AcquireTokenOptions.ExtraParameters = new Dictionary<string, object> { ["p"] = "v" };

            // Act
            var copy = new AuthorizationHeaderProviderOptions(original);

            // Assert
            Assert.NotSame(original.AcquireTokenOptions.ExtraParameters, copy.AcquireTokenOptions.ExtraParameters);
            copy.AcquireTokenOptions.ExtraParameters!["p2"] = "v2";
            Assert.Single(original.AcquireTokenOptions.ExtraParameters!);
        }
    }
}
