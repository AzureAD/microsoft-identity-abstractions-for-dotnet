// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Newtonsoft.Json;
using Xunit;

namespace Microsoft.Identity.Abstractions.Tests
{
    public class AquireTokenOptionsTests
    {
        [Theory]
        [InlineData(
            "{\"ManagedIdentity\":{\"UserAssignedClientId\":\"\"}}",
            false, MtlsBindingStrength.None)]
        [InlineData(
            "{\"ManagedIdentity\":{\"UserAssignedClientId\":\"\"},\"RequestOverMtls\":true}",
            true, MtlsBindingStrength.None)]
        [InlineData(
            "{\"ManagedIdentity\":{\"UserAssignedClientId\":\"\"},\"RequestOverMtls\":false,\"MinimumTokenBindingStrength\":\"None\"}",
            false, MtlsBindingStrength.None)]
        [InlineData(
            "{\"ManagedIdentity\":{\"UserAssignedClientId\":\"\"},\"MinimumTokenBindingStrength\":\"Software\"}",
            false, MtlsBindingStrength.Software)]
        [InlineData(
            "{\"ManagedIdentity\":{\"UserAssignedClientId\":\"\"},\"MinimumTokenBindingStrength\":\"KeyGuard\"}",
            false, MtlsBindingStrength.KeyGuard)]
        [InlineData(
            "{\"ManagedIdentity\":{\"UserAssignedClientId\":\"\"},\"MinimumTokenBindingStrength\":3}",
            false, MtlsBindingStrength.KeyGuard)]
        public void MtlsOptionsDeserializeAndPreserveBindingStrength(
            string json, bool expectedRequestOverMtls, MtlsBindingStrength expectedStrength)
        {
            // Arrange
            AcquireTokenOptions options = Assert.IsType<AcquireTokenOptions>(
                JsonConvert.DeserializeObject<AcquireTokenOptions>(json));

            // Act
            AcquireTokenOptions copy = new AcquireTokenOptions(options);
            AcquireTokenOptions clone = options.Clone();
            AcquireTokenOptions roundTrip = Assert.IsType<AcquireTokenOptions>(
                JsonConvert.DeserializeObject<AcquireTokenOptions>(JsonConvert.SerializeObject(options)));

            // Assert
            Assert.NotNull(options.ManagedIdentity);
            Assert.Equal(expectedRequestOverMtls, options.RequestOverMtls);
            Assert.Equal(expectedStrength, options.MinimumTokenBindingStrength);
            Assert.Equal(expectedRequestOverMtls, copy.RequestOverMtls);
            Assert.Equal(expectedStrength, copy.MinimumTokenBindingStrength);
            Assert.Equal(expectedRequestOverMtls, clone.RequestOverMtls);
            Assert.Equal(expectedStrength, clone.MinimumTokenBindingStrength);
            Assert.Equal(expectedRequestOverMtls, roundTrip.RequestOverMtls);
            Assert.Equal(expectedStrength, roundTrip.MinimumTokenBindingStrength);
        }

        [Fact]
        public void MtlsOptionsDoNotImposeTransportOrBindingStrength()
        {
            // Arrange
            AcquireTokenOptions options = new AcquireTokenOptions();

            // Act
            AcquireTokenOptions clone = options.Clone();

            // Assert
            Assert.False(options.RequestOverMtls);
            Assert.Equal(MtlsBindingStrength.None, options.MinimumTokenBindingStrength);
            Assert.False(clone.RequestOverMtls);
            Assert.Equal(MtlsBindingStrength.None, clone.MinimumTokenBindingStrength);
        }

        [Theory]
        [InlineData(MtlsBindingStrength.None, 0)]
        [InlineData(MtlsBindingStrength.Software, 1)]
        [InlineData(MtlsBindingStrength.KeyGuard, 3)]
        public void MtlsBindingStrengthValuesMatchMsal(MtlsBindingStrength strength, int expectedValue)
        {
            Assert.Equal(expectedValue, (int)strength);
        }

        [Fact]
        public void ManagedIdentitySystemAssigned()
        {
            // App token from a system-assigned managed identity.
            // -------------------------------------------------
            // https://aka.ms/Entra/ManagedIdentityOverview
            /*
            // <managedidentity_json>
            {
                "AquireTokenOptions": {
                    "ManagedIdentity": {}
                }
            }
            // </managedidentitysystem_json>
            */

            // <managedidentity_csharp>
            AcquireTokenOptions acquireTokenOptions = new AcquireTokenOptions
            {
                ManagedIdentity = new ManagedIdentityOptions()
            };
            // </managedidentitysystem_csharp>

            Assert.NotNull(acquireTokenOptions.ManagedIdentity);
            Assert.Null(acquireTokenOptions.ManagedIdentity.UserAssignedClientId);
        }

        [Fact]
        public void ManagedIdentityUserAssigned()
        {
            // App token from a user-assigned managed identity.
            // -------------------------------------------------
            // https://aka.ms/Entra/ManagedIdentityOverview
            /*
            // <managedidentityuser_json>
            {
                "AquireTokenOptions": {
                    "ManagedIdentity": {
                        "UserAssignedClientId": "[ClientIdForTheManagedIdentityResource]"
                    }
                }
            }
            // </managedidentityuser_json>
            */

            // <managedidentityuser_csharp>
            ManagedIdentityOptions managedIdentityOptions = new ManagedIdentityOptions
            {
                UserAssignedClientId = "[ClientIdForTheManagedIdentityResource]"
            };

            AcquireTokenOptions acquireTokenOptions = new AcquireTokenOptions
            {
                ManagedIdentity = managedIdentityOptions
            };
            // </managedidentityuser_csharp>

            Assert.NotNull(acquireTokenOptions.ManagedIdentity);
            Assert.Equal(managedIdentityOptions.UserAssignedClientId, acquireTokenOptions.ManagedIdentity.UserAssignedClientId);
        }

        [Fact]
        public void FmiPathTest()
        {
            // App token from a Federated Managed Identity (FMI).
            // -------------------------------------------------
            /*
            // <fmipath_json>
            {
                "AquireTokenOptions": {
                    "FmiPath": "/example.org/service/my-service"
                }
            }
            // </fmipath_json>
            */

            // <fmipath_csharp>
            AcquireTokenOptions acquireTokenOptions = new AcquireTokenOptions
            {
                FmiPath = "/example.org/service/my-service"
            };
            // </fmipath_csharp>

            Assert.NotNull(acquireTokenOptions.FmiPath);
            Assert.Equal("/example.org/service/my-service", acquireTokenOptions.FmiPath);
        }
    }
}
