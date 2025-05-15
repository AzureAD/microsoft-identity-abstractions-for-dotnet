// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Xunit;
using System.Collections.Generic;
using System;

namespace Microsoft.Identity.Abstractions.ApplicationOptions.Tests
{
    public class CredentialDescriptionTest
    {
        [Fact]
        public void Base64Encoded()
        {
            // Certificate from Base64 encoding
            // --------------------------------
            /*
            // <base64_json>
            {
             "ClientCredentials": [
             {
              "SourceType": "Base64Encoded",
              "Base64EncodedValue": "MIIDHzCgegA.....r1n8Ta0="
             }]
            }
            // </base64_json>
            */

            // <base64_csharp>
            CredentialDescription credentialDescription = new()
            {
                SourceType = CredentialSource.Base64Encoded,
                Base64EncodedValue = "MIIDHzCgegA.....r1n8Ta0="
            };
            // </base64_csharp>

            Assert.Equal(CredentialType.Certificate, credentialDescription.CredentialType);
            Assert.DoesNotContain(credentialDescription.Id, credentialDescription.Base64EncodedValue, System.StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void CertificateFromPath()
        {
            // Certificate from path
            // ---------------------
            /*
            // <path_json>
            {
             "ClientCredentials": [
             {
              "SourceType": "Path",
              "CertificateDiskPath": "c:\\temp\\WebAppCallingWebApiCert.pfx",
              "CertificatePassword": "password"
             }]
            }
            // </path_json>
            */

            // <path_csharp>
            CredentialDescription credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.Path,
                CertificateDiskPath = "c:\\temp\\WebAppCallingWebApiCert.pfx",
                CertificatePassword = "password"
            };
            // </path_csharp>

            Assert.Equal(CredentialType.Certificate, credentialDescription.CredentialType);
            Assert.DoesNotContain(credentialDescription.Id, credentialDescription.CertificatePassword, System.StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void CertificateFromStoreByThumbprint()
        {
            // Certificate from credential store by thumbprint
            // -------------------------------------------------
            /*
            // <thumbprint_json>
            {
             "ClientCertificates": [
             {
              "SourceType": "StoreWithThumbprint",
              "CertificateStorePath": "CurrentUser/My",
              "CertificateThumbprint": "962D129A...D18EFEB6961684"
             }]
            }
            // </thumbprint_json>
            */

            // <thumbprint_csharp>
            CredentialDescription credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.StoreWithThumbprint,
                CertificateStorePath = "LocalMachine/My",
                CertificateThumbprint = "962D129A...D18EFEB6961684"
            };
            // </thumbprint_csharp>

            Assert.Equal(CredentialType.Certificate, credentialDescription.CredentialType);
        }

        [Fact]
        public void CertificateFromStoreByDistinguishedName()
        {
            // Certificate from credential store by distinguished name
            // -------------------------------------------------------
            /*
            // <distinguishedname_json>
            {
             "ClientCredentials": [
             {
              "SourceType": "StoreWithDistinguishedName",
              "CertificateStorePath": "CurrentUser/My",
              "CertificateDistinguishedName": "CN=WebAppCallingWebApiCert"
             }]
            }
            // </distinguishedname_json>
            */

            // <distinguishedname_csharp>
            CredentialDescription credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.StoreWithDistinguishedName,
                CertificateStorePath = "LocalMachine/My",
                CertificateDistinguishedName = "CN=WebAppCallingWebApiCert"
            };
            // </distinguishedname_csharp>

            Assert.Equal(CredentialType.Certificate, credentialDescription.CredentialType);
        }

        [Fact]
        public void CertificateFromKeyVault()
        {
            // Certificate from KeyVault
            // -------------------------
            /*
            // <keyvault_json>
                {
                 "ClientCredentials": [
                  {
                   "SourceType": "KeyVault",
                   "KeyVaultUrl": "https://msidentitywebsamples.vault.azure.net",
                   "KeyVaultCertificateName": "MicrosoftIdentitySamplesCert"
                  }
                 ]
                }
            // </keyvault_json>
            */
            // <keyvault_csharp>
            CredentialDescription credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.KeyVault,
                KeyVaultUrl = "https://msidentitywebsamples.vault.azure.net",
                KeyVaultCertificateName = "MicrosoftIdentitySamplesCert"
            };
            // </keyvault_csharp>

            Assert.Equal(CredentialType.Certificate, credentialDescription.CredentialType);
        }

        [Fact]
        public void Secret()
        {
            // Secret
            // ------
            /*
            // <secret_json>
            {
             "ClientCredentials": [
             {
              "SourceType": "ClientSecret",
              "ClientSecret": "blah"
             }]
            }
            // </secret_json>
             */

            // <secret_csharp>
            CredentialDescription credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.ClientSecret,
                ClientSecret = "blah"
            };
            // </secret_csharp>

            Assert.Equal(CredentialType.Secret, credentialDescription.CredentialType);
            Assert.DoesNotContain(credentialDescription.Id, credentialDescription.ClientSecret, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void SignedAssertionFromMSI()
        {
            // Signed assertion from Federation Identity Credential (Managed Identity)
            // -------------------------------------------------
            // https://learn.microsoft.com/azure/active-directory/develop/workload-identity-federation
            /*
            // <msi_json>
            {
                "ClientCredentials": [
                {
                    "SourceType": "SignedAssertionFromManagedIdentity",
                    "ManagedIdentityClientId": "12345"
                }]
            }
            // </msi_json>
            */

            // <msi_csharp>
            CredentialDescription credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.SignedAssertionFromManagedIdentity,
                ManagedIdentityClientId = "12345" // optional
            };
            // </msi_csharp>

            Assert.Equal(CredentialType.SignedAssertion, credentialDescription.CredentialType);
        }

        [Fact]
        public void SignedAssertionFromFilePath()
        {
            // Signed assertion from a file (managed identities with workload identity federation
            // from Kubernates pods.
            // ---------------------
            // See https://blog.identitydigest.com/azuread-federate-mi/
            /*
            // <aks_json>
            {
                "ClientCredentials": [
                {
                    "SourceType": "SignedAssertionFilePath",
                    "ManagedIdentityClientId": "c:/path.signedAssertion"
                }]
            }
            // </aks_json>
            */

            CredentialDescription credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.SignedAssertionFilePath,
                SignedAssertionFileDiskPath = "c:/path.signedAssertion"
            };
            Assert.Equal(CredentialType.SignedAssertion, credentialDescription.CredentialType);
        }

        [Fact]
        public void CertificateDirectly()
        {
            // Preloaded certificate.
            // ---------------------
#pragma warning disable SYSLIB0026 // Type or member is obsolete
            // <cert_csharp>
            CredentialDescription credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.Certificate,
                Certificate = new System.Security.Cryptography.X509Certificates.X509Certificate2()
            };
            // </cert_csharp>

            credentialDescription.CachedValue = credentialDescription.Certificate;
            Assert.NotNull(credentialDescription.CachedValue);
#pragma warning restore SYSLIB0026 // Type or member is obsolete
            Assert.Equal(CredentialType.Certificate, credentialDescription.CredentialType);
            Assert.NotNull(credentialDescription.Certificate);
        }

        [Fact]
        public void SignedAssertionFromVault()
        {
            // signed assertion from a vault (not KeyVault)
            // -------------------------------------------
            CredentialDescription credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.SignedAssertionFromVault,
                KeyVaultCertificateName = "somename"
            };

            Assert.Equal(CredentialType.SignedAssertion, credentialDescription.CredentialType);

            // Skip a cred in a list of creds (when they are invalid)
            credentialDescription.Skip = true;
            Assert.True(credentialDescription.Skip);
        }

        [Fact]
        public void AutomaticDecryptKeys()
        {
            // Automatic decrypt keys.
            /*
            // <autodecryp_json>
            {
                "TokenDecryptionCredentials": [
                {
                    "SourceType": "AutoDecryptKeys",
                    "DecryptKeysAuthenticationOptions" : {
                        "ProtocolScheme": "Bearer",
                        "AcquireTokenOptions": {
                            "Tenant": "mytenant.onmicrosoftonline.com"
                        }
                    }
                }]
            }
            // </autodecryp_json>
            */

            // <autodecryp_csharp>
            CredentialDescription credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.AutoDecryptKeys,
                DecryptKeysAuthenticationOptions = new AuthorizationHeaderProviderOptions
                {
                    ProtocolScheme = "Bearer",
                    AcquireTokenOptions = new AcquireTokenOptions
                    {
                        Tenant = "mytenant.onmicrosoftonline.com",
                    }
                }
            };
            // </autodecryp_csharp>

            Assert.Equal(CredentialType.DecryptKeys, credentialDescription.CredentialType);
            Assert.Equal("mytenant.onmicrosoftonline.com", credentialDescription.DecryptKeysAuthenticationOptions.AcquireTokenOptions.Tenant);
            Assert.Equal("Bearer", credentialDescription.DecryptKeysAuthenticationOptions.ProtocolScheme);
        }

        [Fact]
        public void CustomSignedAssertion()
        {
            // Signed assertion from a custom provider
            // -------------------------------------------
            // Arrange
            CredentialDescription credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.CustomSignedAssertion,
                CustomSignedAssertionProviderName = "MyCustomProvider",
                CustomSignedAssertionProviderData = new Dictionary<string, object>() { { "MyCustomProviderData_Key", "MyCustomProviderData_Data" } }
            };

            // Act
            var id = credentialDescription.Id;

            // Assert
            Assert.Equal(CredentialType.SignedAssertion, credentialDescription.CredentialType);
            Assert.Equal("MyCustomProvider", credentialDescription.CustomSignedAssertionProviderName);
        }

        [Fact]
        public void TokenExchangeUrl()
        {
            /*
            // <tokenExchangeUrl_json>
            {
                "ClientCredentials": [
                {
                    "SourceType": "SignedAssertionFromManagedIdentity",
                    "TokenExchangeUrl": "api://AzureADTokenExchangeChina"
                }]
            }
            // </tokenExchangeUrl_json>
            */

            // <tokenExchangeUrl_csharp>
            CredentialDescription credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.SignedAssertionFromManagedIdentity,
                TokenExchangeUrl = "api://AzureADTokenExchangeChina"
            };
            // </tokenExchangeUrl_csharp>

            Assert.Equal("api://AzureADTokenExchangeChina", credentialDescription.TokenExchangeUrl);
        }

        [Fact]
        public void UnknownCredentialSource()
        {
            CredentialDescription credentialDescription = new CredentialDescription
            {
                SourceType = (CredentialSource)(1000)
            };
            Assert.Equal(default(CredentialType), credentialDescription.CredentialType);

            credentialDescription.SourceType = (CredentialSource)(-1);
            Assert.Equal(default(CredentialType), credentialDescription.CredentialType);
            Assert.Throws<ArgumentException>(() => credentialDescription.Id);
        }

        [Fact]
        public void TestNullCertificate()
        {
            CredentialDescription credentialDescription = new();
            credentialDescription.Certificate = null;
            Assert.NotNull(credentialDescription.Id);
        }

        [Fact]
        public void TokenExchangeAuthority_SetAndGet_ShouldWork()
        {
            /*
            // <tokenExchangeAuthority_json>
            {
                "ClientCredentials": [
                {
                    "SourceType": "SignedAssertionFromManagedIdentity",
                    "ManagedIdentityClientId": "GUID",
                    "TokenExchangeUrl" : "api://AzureADTokenExchangeSomeCloud1",
                    "TokenExchangeAuthority": "https://login.microsoftonline.cloud2/33e01921-4d64-4f8c-a055-5bdaffd5e33d/v2.0"
                }]
            }
            // </tokenExchangeAuthority_json>
            */

            // <tokenExchangeAuthority_csharp>
            // Arrange
            var credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.SignedAssertionFromManagedIdentity,
                ManagedIdentityClientId = "GUID",
                TokenExchangeUrl = "api://AzureADTokenExchangeSomeCloud1",
                TokenExchangeAuthority = "https://login.microsoftonline.cloud2/33e01921-4d64-4f8c-a055-5bdaffd5e33d/v2.0"
            };

            // Act
            var actualTokenExchangeAuthority = credentialDescription.TokenExchangeAuthority;
            // </tokenExchangeAuthority_csharp>

            // Assert
            Assert.Equal("https://login.microsoftonline.cloud2/33e01921-4d64-4f8c-a055-5bdaffd5e33d/v2.0", actualTokenExchangeAuthority);
        }

        [Fact]
        public void CopyConstructor_ShouldCopyTokenExchangeAuthority()
        {
            // Arrange
            var original = new CredentialDescription
            {
                TokenExchangeAuthority = "https://login.microsoftonline.cloud2/33e01921-4d64-4f8c-a055-5bdaffd5e33d/v2.0"
            };

            // Act
            var copy = new CredentialDescription(original);

            // Assert
            Assert.Equal(original.TokenExchangeAuthority, copy.TokenExchangeAuthority);
        }

        [Fact]
        public void CopyConstructorFromNull_ShouldThrow()
        {
            // Arrange
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            CredentialDescription original = null;
            // Act & Assert
            _ = Assert.Throws<ArgumentNullException>(() => new CredentialDescription(original!));
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        }

        [Fact]
        public void Algorithm_SetAndGet_ShouldWork()
        {
            /*
            // <algorithm_json>
            {
                "ClientCredentials": [
                {
                    "Algorithm": "RS256",
                }]
            }
            // </algorithm_json>
            */

            // <algorithm_csharp>
            // Arrange
            var credentialDescription = new CredentialDescription
            {
                Algorithm = "RS256"
            };

            // Act
            var actualAlgorithm = credentialDescription.Algorithm;
            // </algorithm_csharp>

            // Assert
            Assert.Equal("RS256", actualAlgorithm);
        }

        [Fact]
        public void CopyConstructor_ShouldCopyAlgorithm()
        {
            // Arrange
            var original = new CredentialDescription
            {
                Algorithm = "RS256"
            };

            // Act
            var copy = new CredentialDescription(original);

            // Assert
            Assert.Equal(original.Algorithm, copy.Algorithm);
        }
    }
}
