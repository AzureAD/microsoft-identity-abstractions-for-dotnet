using Microsoft.Identity.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Xunit;

namespace Microsoft.Identity.Abstractions.Tests
{
    public class CredentialDescriptionIdTest
    {
        [Fact]
        public void NullSecretIdTest()
        {
            var credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.ClientSecret,
                ClientSecret = null
            };
            Assert.Equal("ClientSecret=", credentialDescription.Id);
        }

        [Fact]
        public void DisplayCredentialDescriptionIdsForAllSourceTypes()
        {
            // Create a credential description for each SourceType
            var credentialDescriptions = new Dictionary<CredentialSource, CredentialDescription>
                    {
                        // Certificate
                        {
                            CredentialSource.Certificate,
                            new CredentialDescription
                            {
                                SourceType = CredentialSource.Certificate,
                                Certificate = new X509Certificate2("C:\\Users\\jmprieur\\Downloads\\buildautomation-AzureADIdentityDivisionTestAgentCert-20230521.pfx"), // "path/to/certificate.pfx", "password"
                                CertificatePassword = "password"
                            }
                        },
                        
                        // KeyVault
                        {
                            CredentialSource.KeyVault,
                            new CredentialDescription
                            {
                                SourceType = CredentialSource.KeyVault,
                                KeyVaultUrl = "https://mykeyvault.vault.azure.net",
                                KeyVaultCertificateName = "MyCertificate"
                            }
                        },
                        
                        // Base64Encoded
                        {
                            CredentialSource.Base64Encoded,
                            new CredentialDescription
                            {
                                SourceType = CredentialSource.Base64Encoded,
                                Base64EncodedValue = "MIIDHzCgegA.....r1n8Ta0="
                            }
                        },
                        
                        // Path
                        {
                            CredentialSource.Path,
                            new CredentialDescription
                            {
                                SourceType = CredentialSource.Path,
                                CertificateDiskPath = "c:\\temp\\Certificate.pfx",
                                CertificatePassword = "password"
                            }
                        },
                        
                        // StoreWithThumbprint
                        {
                            CredentialSource.StoreWithThumbprint,
                            new CredentialDescription
                            {
                                SourceType = CredentialSource.StoreWithThumbprint,
                                CertificateStorePath = "CurrentUser/My",
                                CertificateThumbprint = "962D129A...D18EFEB6961684"
                            }
                        },
                        
                        // StoreWithDistinguishedName
                        {
                            CredentialSource.StoreWithDistinguishedName,
                            new CredentialDescription
                            {
                                SourceType = CredentialSource.StoreWithDistinguishedName,
                                CertificateStorePath = "CurrentUser/My",
                                CertificateDistinguishedName = "CN=WebAppCallingWebApiCert"
                            }
                        },
                        
                        // ClientSecret
                        {
                            CredentialSource.ClientSecret,
                            new CredentialDescription
                            {
                                SourceType = CredentialSource.ClientSecret,
                                ClientSecret = "MyClientSecret"
                            }
                        },
                        
                        // SignedAssertionFromManagedIdentity
                        {
                            CredentialSource.SignedAssertionFromManagedIdentity,
                            new CredentialDescription
                            {
                                SourceType = CredentialSource.SignedAssertionFromManagedIdentity,
                                ManagedIdentityClientId = "12345"
                            }
                        },
                        
                        // SignedAssertionFilePath
                        {
                            CredentialSource.SignedAssertionFilePath,
                            new CredentialDescription
                            {
                                SourceType = CredentialSource.SignedAssertionFilePath,
                                SignedAssertionFileDiskPath = "c:/path.signedAssertion"
                            }
                        },
                        
                        // SignedAssertionFromVault
                        {
                            CredentialSource.SignedAssertionFromVault,
                            new CredentialDescription
                            {
                                SourceType = CredentialSource.SignedAssertionFromVault,
                                KeyVaultCertificateName = "somename"
                            }
                        },
                        
                        // AutoDecryptKeys
                        {
                            CredentialSource.AutoDecryptKeys,
                            new CredentialDescription
                            {
                                SourceType = CredentialSource.AutoDecryptKeys,
                                DecryptKeysAuthenticationOptions = new AuthorizationHeaderProviderOptions
                                {
                                    ProtocolScheme = "Bearer",
                                    AcquireTokenOptions = new AcquireTokenOptions
                                    {
                                        Tenant = "mytenant.onmicrosoftonline.com"
                                    }
                                }
                            }
                        },
                        
                        // CustomSignedAssertion
                        {
                            CredentialSource.CustomSignedAssertion,
                            new CredentialDescription
                            {
                                SourceType = CredentialSource.CustomSignedAssertion,
                                CustomSignedAssertionProviderName = "MyCustomProvider",
                                CustomSignedAssertionProviderData = new Dictionary<string, object>
                                {
                                    { "MyCustomProviderData_Key", "MyCustomProviderData_Data" }
                                }
                            }
                        }
                    };

#if DEBUG
            StringBuilder sb = new StringBuilder();
            foreach (var kvp in credentialDescriptions)
            {
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, $"Id: {kvp.Value.Id}"));
            }
            string allDescriptions = sb.ToString();
#endif

            // Assertions to make sure each credential description has an Id
            foreach (var credentialDescription in credentialDescriptions.Values)
            {
                Assert.NotNull(credentialDescription.Id);
                Assert.Contains(credentialDescription.SourceType.ToString(), credentialDescription.Id, StringComparison.Ordinal);
            }
        }
    }
}
