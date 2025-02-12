// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Xunit;

namespace Microsoft.Identity.Abstractions.Tests
{
    public class AquireTokenOptionsTests
    {
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
            // https://aka.ms/FederatedIdentityCredentials
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
