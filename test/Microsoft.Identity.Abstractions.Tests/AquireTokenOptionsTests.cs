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
                    "ManagedIdentity": {
                        "ManagedIdentityType": "SystemAssigned"
                    }
                }
            }
            // </managedidentitysystem_json>
            */

            // <managedidentity_csharp>
            AcquireTokenOptions acquireTokenOptions = new AcquireTokenOptions
            {
                ManagedIdentity = new ManagedIdentityOptions()
                {
                    // default: ManagedIdentityType = ManagedIdentityType.SystemAssigned
                }
            };
            // </managedidentitysystem_csharp>

            Assert.Equal(ManagedIdentityType.SystemAssigned, acquireTokenOptions.ManagedIdentity.ManagedIdentityType);
            Assert.Null(acquireTokenOptions.ManagedIdentity.ClientId);
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
                        "ManagedIdentityType": "UserAssigned"
                        "ClientId": "[ClientIdForTheManagedIdentityResource]"
                    }
                }
            }
            // </managedidentityuser_json>
            */

            // <managedidentityuser_csharp>
            ManagedIdentityOptions managedIdentityDescription = new ManagedIdentityOptions
            {
                ManagedIdentityType = ManagedIdentityType.UserAssigned,
                ClientId = "[ClientIdForTheManagedIdentityResource]"
            };

            AcquireTokenOptions acquireTokenOptions = new AcquireTokenOptions
            {
                ManagedIdentity = managedIdentityDescription
            };
            // </managedidentityuser_csharp>

            Assert.Equal(ManagedIdentityType.UserAssigned, acquireTokenOptions.ManagedIdentity.ManagedIdentityType);
            Assert.Equal(managedIdentityDescription.ClientId, acquireTokenOptions.ManagedIdentity.ClientId);
        }
    }
}
