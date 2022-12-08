using Microsoft.Identity.Abstractions;
using System.Runtime.ConstrainedExecution;
using Xunit;

namespace UnitTests
{
    public class CredentialDescriptionTest
    {
        [Fact]
        public void CredentialDescriptionProperties()
        {
            CredentialDescription credentialDescription;

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
            credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.Base64Encoded,
                Base64EncodedValue = "MIIDHzCgegA.....r1n8Ta0="
            };
            Assert.Equal(CredentialType.Certificate, credentialDescription.CredentialType);
            Assert.Null(credentialDescription.Container);
            Assert.Equal(credentialDescription.Base64EncodedValue, credentialDescription.ReferenceOrValue);

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
            credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.Path,
                CertificateDiskPath = "c:\\temp\\WebAppCallingWebApiCert.pfx",
                CertificatePassword = "password"
            };
            Assert.Equal(CredentialType.Certificate, credentialDescription.CredentialType);
            Assert.Equal(credentialDescription.CertificateDiskPath, credentialDescription.Container);
            Assert.Equal(credentialDescription.CertificatePassword, credentialDescription.ReferenceOrValue);

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
            credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.StoreWithThumbprint,
                CertificateStorePath = "CurrentUser/My",
                CertificateThumbprint = "962D129A...D18EFEB6961684"

            };
            Assert.Equal(CredentialType.Certificate, credentialDescription.CredentialType);
            Assert.Equal(credentialDescription.CertificateStorePath, credentialDescription.Container);
            Assert.Equal(credentialDescription.CertificateThumbprint, credentialDescription.ReferenceOrValue);

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
            credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.StoreWithDistinguishedName,
                CertificateStorePath = "CurrentUser/My",
                CertificateDistinguishedName = "CN=WebAppCallingWebApiCert"

            };
            Assert.Equal(CredentialType.Certificate, credentialDescription.CredentialType);
            Assert.Equal(credentialDescription.CertificateStorePath, credentialDescription.Container);
            Assert.Equal(credentialDescription.CertificateDistinguishedName, credentialDescription.ReferenceOrValue);

            // Certificate from KeyVault
            // -------------------------
            /*
                {
                 "ClientCredentials": [
                  {
                  "SourceType": "KeyVault",
                  "KeyVaultUrl": "https://msidentitywebsamples.vault.azure.net",
                  "KeyVaultCertificateName": "MicrosoftIdentitySamplesCert"
                  }
                 ]
                }
             */
            credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.KeyVault,
                KeyVaultUrl = "https://msidentitywebsamples.vault.azure.net",
                KeyVaultCertificateName = "MicrosoftIdentitySamplesCert"

            };
            Assert.Equal(CredentialType.Certificate, credentialDescription.CredentialType);
            Assert.Equal(credentialDescription.KeyVaultUrl, credentialDescription.Container);
            Assert.Equal(credentialDescription.KeyVaultCertificateName, credentialDescription.ReferenceOrValue);

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
            credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.ClientSecret,
                ClientSecret = "blah"
            };
            Assert.Equal(CredentialType.Secret, credentialDescription.CredentialType);
            Assert.Null(credentialDescription.Container);
            Assert.Equal(credentialDescription.ClientSecret, credentialDescription.ReferenceOrValue);


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
            credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.SignedAssertionFromManagedIdentity,
                ManagedIdentityClientId = "12345" // optional
            };
            Assert.Equal(CredentialType.SignedAssertion, credentialDescription.CredentialType);
            Assert.Null(credentialDescription.Container);
            Assert.Equal(credentialDescription.ManagedIdentityClientId, credentialDescription.ReferenceOrValue);


            // Signed assertion from a file (managed identities with workload identity federation
            // from Kubernates pods.
            // ---------------------
            // See https://blog.identitydigest.com/azuread-federate-mi/
            /*
            {
                "ClientCredentials": [
                {
                    "SourceType": "SignedAssertionFromManagedIdentity",
                    "ManagedIdentityClientId": "12345"
                }]
            }
            */
            credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.SignedAssertionFilePath,
                SignedAssertionFileDiskPath = "c:/path.signedAssertion"
            };
            Assert.Equal(CredentialType.SignedAssertion, credentialDescription.CredentialType);
            Assert.Null(credentialDescription.Container);
            Assert.Equal(credentialDescription.SignedAssertionFileDiskPath, credentialDescription.ReferenceOrValue);

            // Preloaded certificate.
            // ---------------------
#pragma warning disable SYSLIB0026 // Type or member is obsolete
            credentialDescription = new CredentialDescription
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

            // signed assertion from a vault (not KeyVault)
            // -------------------------------------------
            credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.SignedAssertionFromVault,
                KeyVaultUrl = "someurl"
            };
            Assert.Equal(CredentialType.SignedAssertion, credentialDescription.CredentialType);

            // Skip a cred in a list of creds (when they are invalid)
            credentialDescription.Skip= true;
            Assert.True(credentialDescription.Skip);
        }
    }
}
