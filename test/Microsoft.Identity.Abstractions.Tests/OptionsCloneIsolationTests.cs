// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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
