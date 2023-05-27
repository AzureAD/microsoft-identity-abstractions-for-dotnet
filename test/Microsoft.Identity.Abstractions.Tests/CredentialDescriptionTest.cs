// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Security.AccessControl;
using Xunit;

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
            {
             "ClientCredentials": [
             {
              "SourceType": "Base64Encoded",
              "Base64EncodedValue": "MIIDHzCgegA.....r1n8Ta0="
             }]
            }
            */
            CredentialDescription credentialDescription = new()
            {
                SourceType = CredentialSource.Base64Encoded,
                Base64EncodedValue = "MIIDHzCgegA.....r1n8Ta0="
            };

            Assert.Equal(CredentialType.Certificate, credentialDescription.CredentialType);
            Assert.Null(credentialDescription.Container);
            Assert.Equal(credentialDescription.Base64EncodedValue, credentialDescription.ReferenceOrValue);
        }

        [Fact]
        public void CertificateFromPath()
        {
            // Certificate from path 
            // ---------------------
            /*
            {
             "ClientCredentials": [
             {
              "SourceType": "Path",
              "CertificateDiskPath": "c:\\temp\\WebAppCallingWebApiCert.pfx",
              "CertificatePassword": "password"
             }]
            }
            */
            CredentialDescription credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.Path,
                CertificateDiskPath = "c:\\temp\\WebAppCallingWebApiCert.pfx",
                CertificatePassword = "password"
            };

            Assert.Equal(CredentialType.Certificate, credentialDescription.CredentialType);
            Assert.Equal(credentialDescription.CertificateDiskPath, credentialDescription.Container);
            Assert.Equal(credentialDescription.CertificatePassword, credentialDescription.ReferenceOrValue);
        }

        [Fact]
        public void CertificateFromStoreByThumbprint()
        {
            // Certificate from credential store by thumbprint
            // -------------------------------------------------
            /*
            {
             "ClientCertificates": [
             {
              "SourceType": "StoreWithThumbprint",
              "CertificateStorePath": "CurrentUser/My",
              "CertificateThumbprint": "962D129A...D18EFEB6961684"
             }]
            }
            */
            CredentialDescription credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.StoreWithThumbprint,
                CertificateStorePath = "CurrentUser/My",
                CertificateThumbprint = "962D129A...D18EFEB6961684"

            };

            Assert.Equal(CredentialType.Certificate, credentialDescription.CredentialType);
            Assert.Equal(credentialDescription.CertificateStorePath, credentialDescription.Container);
            Assert.Equal(credentialDescription.CertificateThumbprint, credentialDescription.ReferenceOrValue);
        }

        [Fact]
        public void CertificateFromStoreByDistinguishedName()
        {
            // Certificate from credential store by distinguished name
            // -------------------------------------------------------
            /*
            {
             "ClientCredentials": [
             {
              "SourceType": "StoreWithDistinguishedName",
              "CertificateStorePath": "CurrentUser/My",
              "CertificateDistinguishedName": "CN=WebAppCallingWebApiCert"
             }]
            }
            */
            CredentialDescription credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.StoreWithDistinguishedName,
                CertificateStorePath = "CurrentUser/My",
                CertificateDistinguishedName = "CN=WebAppCallingWebApiCert"

            };

            Assert.Equal(CredentialType.Certificate, credentialDescription.CredentialType);
            Assert.Equal(credentialDescription.CertificateStorePath, credentialDescription.Container);
            Assert.Equal(credentialDescription.CertificateDistinguishedName, credentialDescription.ReferenceOrValue);
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
            Assert.Equal(credentialDescription.KeyVaultUrl, credentialDescription.Container);
            Assert.Equal(credentialDescription.KeyVaultCertificateName, credentialDescription.ReferenceOrValue);
        }

        [Fact]
        public void Secret()
        {
            // Secret
            // ------
            /*

            {
             "ClientCredentials": [
             {
              "SourceType": "ClientSecret",
              "ClientSecret": "blah"
             }]
            }
             */
            CredentialDescription credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.ClientSecret,
                ClientSecret = "blah"
            };

            Assert.Equal(CredentialType.Secret, credentialDescription.CredentialType);
            Assert.Null(credentialDescription.Container);
            Assert.Equal(credentialDescription.ClientSecret, credentialDescription.ReferenceOrValue);
        }

        [Fact]
        public void SignedAssertionFromMSI()
        {
            // Signed assertion from Managed identity federation
            // -------------------------------------------------
            // https://learn.microsoft.com/azure/active-directory/develop/workload-identity-federation
            /*
            {
                "ClientCredentials": [
                {
                    "SourceType": "SignedAssertionFromManagedIdentity",
                    "ManagedIdentityClientId": "12345"
                }]
            }
            */

            CredentialDescription credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.SignedAssertionFromManagedIdentity,
                ManagedIdentityClientId = "12345" // optional
            };
            Assert.Equal(CredentialType.SignedAssertion, credentialDescription.CredentialType);
            Assert.Null(credentialDescription.Container);
            Assert.Equal(credentialDescription.ManagedIdentityClientId, credentialDescription.ReferenceOrValue);
        }


        [Fact]
        public void SignedAssertionFromFilePath()
        {
            // Signed assertion from a file (managed identities with workload identity federation
            // from Kubernates pods.
            // ---------------------
            // See https://blog.identitydigest.com/azuread-federate-mi/
            /*
            {
                "ClientCredentials": [
                {
                    "SourceType": "SignedAssertionFilePath",
                    "ManagedIdentityClientId": "c:/path.signedAssertion"
                }]
            }
            */

            CredentialDescription credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.SignedAssertionFilePath,
                SignedAssertionFileDiskPath = "c:/path.signedAssertion"
            };
            Assert.Equal(CredentialType.SignedAssertion, credentialDescription.CredentialType);
            Assert.Null(credentialDescription.ReferenceOrValue);
            Assert.Equal(credentialDescription.SignedAssertionFileDiskPath, credentialDescription.Container);
        }

        [Fact]
        public void CertificateDirectly()
        {
            // Preloaded certificate.
            // ---------------------
#pragma warning disable SYSLIB0026 // Type or member is obsolete
            CredentialDescription credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.Certificate,
                Certificate = new System.Security.Cryptography.X509Certificates.X509Certificate2()
            };

            credentialDescription.CachedValue = credentialDescription.Certificate;
            Assert.NotNull(credentialDescription.CachedValue);
#pragma warning restore SYSLIB0026 // Type or member is obsolete
            Assert.Equal(CredentialType.Certificate, credentialDescription.CredentialType);
            Assert.Null(credentialDescription.Container);
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
                KeyVaultUrl = "someurl"
            };

            Assert.Equal(CredentialType.SignedAssertion, credentialDescription.CredentialType);

            // Skip a cred in a list of creds (when they are invalid)
            credentialDescription.Skip= true;
            Assert.True(credentialDescription.Skip);
        }

        [Fact]
        public void AutomaticDecryptKeys()
        {
            // Automatic decrypt keys.
            /*
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
            */

            CredentialDescription credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.AutoDecryptKeys,
                DecryptKeysAuthenticationOptions = new AuthorizationHeaderProviderOptions
                {
                     ProtocolScheme = "Bearer",
                    AcquireTokenOptions = new AcquireTokenOptions {
                         Tenant = "mytenant.onmicrosoftonline.com",
                    }
                }
            };
            Assert.Equal(CredentialType.DecryptKeys, credentialDescription.CredentialType);
            Assert.Equal("mytenant.onmicrosoftonline.com", credentialDescription.DecryptKeysAuthenticationOptions.AcquireTokenOptions.Tenant);
            Assert.Equal("Bearer", credentialDescription.DecryptKeysAuthenticationOptions.ProtocolScheme);
            Assert.Null(credentialDescription.ReferenceOrValue);
            Assert.Null(credentialDescription.Container);
        }

        [Fact]
        public void UnknownCredentialSource()
        {
            CredentialDescription credentialDescription = new CredentialDescription
            {
                SourceType = (CredentialSource)(1000)
            };
            Assert.Equal(default(CredentialType), credentialDescription.CredentialType);
            Assert.Null(credentialDescription.ReferenceOrValue);
            Assert.Null(credentialDescription.Container);

            credentialDescription.SourceType = (CredentialSource)(-1);
            credentialDescription.ReferenceOrValue = "referenceOrValue";
            credentialDescription.Container = "container";
            Assert.Equal(default(CredentialType), credentialDescription.CredentialType);
            Assert.Null(credentialDescription.ReferenceOrValue);
            Assert.Null(credentialDescription.Container);
        }

            // Both container and reference or value.
            [Theory]
        [InlineData(CredentialSource.Base64Encoded)]
        [InlineData(CredentialSource.StoreWithThumbprint)]
        [InlineData(CredentialSource.StoreWithDistinguishedName)]
        [InlineData(CredentialSource.SignedAssertionFromVault)]
        [InlineData(CredentialSource.KeyVault)]
        [InlineData(CredentialSource.Path)]
        public void TestContainerAndValueOrReference(CredentialSource credentialSource)
        {
            CredentialDescription credentialDescription = new CredentialDescription { SourceType = credentialSource };
            credentialDescription.Container = "container";
            Assert.Equal("container", credentialDescription.Container);
            credentialDescription.ReferenceOrValue = "referenceOrValue";
            Assert.Equal("referenceOrValue", credentialDescription.ReferenceOrValue);
        }

        [Fact]
        public void TestContainerAndValueOrReferenceForCertificate()
        {
            CredentialDescription credentialDescription = new();
            credentialDescription.Certificate = null;
            credentialDescription.Container = "container";
            credentialDescription.ReferenceOrValue = "referenceOrValue";
            Assert.Null(credentialDescription.Container);
            Assert.Null(credentialDescription.ReferenceOrValue);
        }

        // Container only
        [Theory]
        [InlineData(CredentialSource.SignedAssertionFilePath)]
        public void TestContainer(CredentialSource credentialSource)
        {
            CredentialDescription credentialDescription = new CredentialDescription{SourceType = credentialSource};
            credentialDescription.Container = "container";
            Assert.Equal("container", credentialDescription.Container);
        }

        // Ref/Value only
        [Theory]
        [InlineData(CredentialSource.ClientSecret)]
        [InlineData(CredentialSource.SignedAssertionFromManagedIdentity)]
        public void TestValueOrReference(CredentialSource credentialSource)
        {
            CredentialDescription credentialDescription = new CredentialDescription { SourceType = credentialSource };
            credentialDescription.ReferenceOrValue = "referenceOrValue";
            Assert.Equal("referenceOrValue", credentialDescription.ReferenceOrValue);
        }


    }
}
