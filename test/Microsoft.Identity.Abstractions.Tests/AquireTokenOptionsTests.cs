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

        [Fact]
        public void AuthorizationTokenProviderUrlTest()
        {
            // Authorization token provider URL to acquire AuthZ token.
            // -------------------------------------------------
            /*
            // <authorizationtokenproviderurl_json>
            {
                "AquireTokenOptions": {
                    "AuthorizationTokenProviderUrl": "https://authzTest.microsoft.com/"
                }
            }
            // </authorizationtokenproviderurl_json>
            */

            // <authorizationtokenproviderurl_csharp>
            AcquireTokenOptions acquireTokenOptions = new()
            {
                AuthorizationTokenProviderUrl = "https://authzTest.microsoft.com/"
            };
            // </authorizationtokenproviderurl_csharp>

            Assert.NotNull(acquireTokenOptions.AuthorizationTokenProviderUrl);
            Assert.Equal("https://authzTest.microsoft.com/", acquireTokenOptions.AuthorizationTokenProviderUrl);
        }

        [Fact]
        public void AuthorizationDetailsTest()
        {
            // AuthorizationDetails is a json blob that can be used to pass additional information to the authorization server.

            // -------------------------------------------------
            /*
            // <authorizationdetails_json>
            {
                "AquireTokenOptions": {
                    "AuthorizationDetails": {
                                              "authorization_details": [
                                                {
                                                  "scope": "/subscriptions/ce06ac4b-5d7b-4324-9196-8a4c6d2f7ffd/resourceGroups/testrg/providers/Microsoft.Storage/storageAccounts/testaccount/containers/testContainer",
                                                  "actions": [
                                                    { "id": "Microsoft.Storage/storageAccounts/blobServices/containers/blobs/read", "isDataAction": true },
                                                    { "id": "Microsoft.Storage/storageAccounts/blobServices/containers/blobs/write", "isDataAction": true },
                                                    { "id": "Microsoft.Storage/storageAccounts/blobServices/containers/blobs/delete", "isDataAction": true }
                                                  ]
                                                }
                                              ],
                                              "resource": "storage.azure.com"
                                            }
                 }
            }
            // </authorizationdetails_json>
            */

            // <authorizationdetails_csharp>
            string AuthorizationDetailsJson = @"
            {
              ""authorization_details"": [
                {
                  ""scope"": ""/subscriptions/ab15ac4b-5d7b-4324-9196-8a4c6d2f7ffd/resourceGroups/testrg/providers/Microsoft.Storage/storageAccounts/testaccount/containers/testContainer"",
                  ""actions"": [
                    {
                      ""id"": ""Microsoft.Storage/storageAccounts/blobServices/containers/blobs/read"",
                      ""isDataAction"": true
                    },
                    {
                      ""id"": ""Microsoft.Storage/storageAccounts/blobServices/containers/blobs/write"",
                      ""isDataAction"": true
                    },
                    {
                      ""id"": ""Microsoft.Storage/storageAccounts/blobServices/containers/blobs/delete"",
                      ""isDataAction"": true
                    }
                  ]
                }
              ],
              ""resource"": ""storage.azure.com""
            }";

            AcquireTokenOptions acquireTokenOptions = new()
            {
                AuthorizationDetails = AuthorizationDetailsJson
            };
            // </authorizationdetails_csharp>

            Assert.NotNull(acquireTokenOptions.AuthorizationDetails);
            Assert.Equal(AuthorizationDetailsJson, acquireTokenOptions.AuthorizationDetails);
        }
    }
}
