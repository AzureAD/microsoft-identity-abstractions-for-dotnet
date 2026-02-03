// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Xunit;

namespace Microsoft.Identity.Abstractions.Tests
{
    public class CredentialDescriptionIdTest
    {
        const string base64encoded = "MIIDDjCCAfagAwIBAgIQIo6M1+XYDbtDgY/Z3VcWOjANBgkqhkiG9w0BAQsFADAaMRgwFgYDVQQDDA9UZXN0Q2VydGlmaWNhdGUwHhcNMjUwMzI1MDExMTExWhcNMjYwMzI1MDEzMTExWjAaMRgwFgYDVQQDDA9UZXN0Q2VydGlmaWNhdGUwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQClI8k4uhi7yfdHHYq17MbWmGl2sdgTZ+yRserSQFLcAoVLkVppfwEeccuj6DQraQXhoXXLVk+DXgTx5+/pJTekNpv4arexGwTG2GztPDaiku188minJVDHB7sD6MqTBXqDrFMfEu4uZZmDRMTC3q+sNhiDpMG09NjH/OZnhab0ORu0lXDxoFKtZbYc1yK56eMsYqz2T+K8JkJejvMtuOq3ZERQSvhn+Ud400wUYTyi45D/5zKHrqw0Xyg9Q7Hl4g7GnBcWPWG8JGIseaLJFWyl5yXEQrR1s7S1RNmX+9/YbSV/JzSzicmM66aytwjI8VTLuoXEXXYdzZo0ZO0oxheRAgMBAAGjUDBOMA4GA1UdDwEB/wQEAwIFoDAdBgNVHSUEFjAUBggrBgEFBQcDAgYIKwYBBQUHAwEwHQYDVR0OBBYEFNQtVk+KmmYHqcdsH/jf305p/dh8MA0GCSqGSIb3DQEBCwUAA4IBAQBmUdRF8R3i3WJiEnxYSpgsdMZox1WrEpQugnLYEywmXqNify/M4qNvBpKVNOVVFmGg94V57D8SHbr77nIhyiwPUF3161MixJ+EO0bl+Iepu3GAQ6nbgnRtsbPhhmFIR5MWCQ7ekVaoxIbvWV/Jxha4ZoVyxLOcshRQaKfUZuzjZorhY0P4oXM7zVAw80btbP5nC5FucH0Z2kzIVwXDJsTKWeBXDSXm67hZ2wA4+B9NDDtRX6hxTT9wWmo6vlogfwGKUJJp+X6rs4jt59whms6himga0exf5k9A32+o8O8RJr0h/1gN6VWOcZeu+aBWH/ozuB/eG6hZMV0/oKvhbUki";

        [Fact]
        public void NullSecretIdTest()
        {
            var credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.ClientSecret,
                ClientSecret = null
            };
            Assert.Equal("RedactedClientSecret=", credentialDescription.Id);
        }

        [Fact]
        public void DisplayCredentialDescriptionIdsForAllSourceTypes()
        {
                // Create a credential description for each SourceType
#pragma warning disable SYSLIB0026 // Type or member is obsolete
                var credentialDescriptions = new Dictionary<CredentialSource, CredentialDescription>
                    {
                        // Certificate
                        {
                            CredentialSource.Certificate,
                            new CredentialDescription
                            {
                                SourceType = CredentialSource.Certificate,
#if NET10_0_OR_GREATER
                                Certificate = X509CertificateLoader.LoadCertificate(Convert.FromBase64String(base64encoded)), // "path/to/certificate.pfx"
#else
                                Certificate = new X509Certificate2(Convert.FromBase64String(base64encoded)), // "path/to/certificate.pfx"
#endif
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
#pragma warning restore SYSLIB0026 // Type or member is obsolete

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

        [Fact]
        public void CachedId_InvalidatedWhen_KeyVaultUrl_Changes()
        {
            var credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.KeyVault,
                KeyVaultUrl = "https://vault1.vault.azure.net",
                KeyVaultCertificateName = "MyCert"
            };

            var initialId = credentialDescription.Id;
            Assert.Contains("vault1", initialId, StringComparison.Ordinal);

            credentialDescription.KeyVaultUrl = "https://vault2.vault.azure.net";
            var updatedId = credentialDescription.Id;

            Assert.NotEqual(initialId, updatedId);
            Assert.Contains("vault2", updatedId, StringComparison.Ordinal);
        }

        [Fact]
        public void CachedId_InvalidatedWhen_KeyVaultCertificateName_Changes()
        {
            var credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.KeyVault,
                KeyVaultUrl = "https://myvault.vault.azure.net",
                KeyVaultCertificateName = "Cert1"
            };

            var initialId = credentialDescription.Id;
            Assert.Contains("Cert1", initialId, StringComparison.Ordinal);

            credentialDescription.KeyVaultCertificateName = "Cert2";
            var updatedId = credentialDescription.Id;

            Assert.NotEqual(initialId, updatedId);
            Assert.Contains("Cert2", updatedId, StringComparison.Ordinal);
        }

        [Fact]
        public void CachedId_InvalidatedWhen_Base64EncodedValue_Changes()
        {
            var credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.Base64Encoded,
                Base64EncodedValue = "Value1"
            };

            var initialId = credentialDescription.Id;

            credentialDescription.Base64EncodedValue = "Value2";
            var updatedId = credentialDescription.Id;

            Assert.NotEqual(initialId, updatedId);
        }

        [Fact]
        public void CachedId_InvalidatedWhen_CertificateDiskPath_Changes()
        {
            var credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.Path,
                CertificateDiskPath = "c:\\path1\\cert.pfx"
            };

            var initialId = credentialDescription.Id;
            Assert.Contains("path1", initialId, StringComparison.Ordinal);

            credentialDescription.CertificateDiskPath = "c:\\path2\\cert.pfx";
            var updatedId = credentialDescription.Id;

            Assert.NotEqual(initialId, updatedId);
            Assert.Contains("path2", updatedId, StringComparison.Ordinal);
        }

        [Fact]
        public void CachedId_InvalidatedWhen_CertificateStorePath_Changes()
        {
            var credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.StoreWithThumbprint,
                CertificateStorePath = "CurrentUser/My",
                CertificateThumbprint = "123456"
            };

            var initialId = credentialDescription.Id;
            Assert.Contains("CurrentUser", initialId, StringComparison.Ordinal);

            credentialDescription.CertificateStorePath = "LocalMachine/My";
            var updatedId = credentialDescription.Id;

            Assert.NotEqual(initialId, updatedId);
            Assert.Contains("LocalMachine", updatedId, StringComparison.Ordinal);
        }

        [Fact]
        public void CachedId_InvalidatedWhen_CertificateThumbprint_Changes()
        {
            var credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.StoreWithThumbprint,
                CertificateStorePath = "CurrentUser/My",
                CertificateThumbprint = "Thumbprint1"
            };

            var initialId = credentialDescription.Id;
            Assert.Contains("Thumbprint1", initialId, StringComparison.Ordinal);

            credentialDescription.CertificateThumbprint = "Thumbprint2";
            var updatedId = credentialDescription.Id;

            Assert.NotEqual(initialId, updatedId);
            Assert.Contains("Thumbprint2", updatedId, StringComparison.Ordinal);
        }

        [Fact]
        public void CachedId_InvalidatedWhen_CertificateDistinguishedName_Changes()
        {
            var credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.StoreWithDistinguishedName,
                CertificateStorePath = "CurrentUser/My",
                CertificateDistinguishedName = "CN=Cert1"
            };

            var initialId = credentialDescription.Id;
            Assert.Contains("CN=Cert1", initialId, StringComparison.Ordinal);

            credentialDescription.CertificateDistinguishedName = "CN=Cert2";
            var updatedId = credentialDescription.Id;

            Assert.NotEqual(initialId, updatedId);
            Assert.Contains("CN=Cert2", updatedId, StringComparison.Ordinal);
        }

        [Fact]
        public void CachedId_InvalidatedWhen_ManagedIdentityClientId_Changes()
        {
            var credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.SignedAssertionFromManagedIdentity,
                ManagedIdentityClientId = "ClientId1"
            };

            var initialId = credentialDescription.Id;
            Assert.Contains("ClientId1", initialId, StringComparison.Ordinal);

            credentialDescription.ManagedIdentityClientId = "ClientId2";
            var updatedId = credentialDescription.Id;

            Assert.NotEqual(initialId, updatedId);
            Assert.Contains("ClientId2", updatedId, StringComparison.Ordinal);
        }

        [Fact]
        public void CachedId_InvalidatedWhen_SignedAssertionFileDiskPath_Changes()
        {
            var credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.SignedAssertionFilePath,
                SignedAssertionFileDiskPath = "c:/path1.jwt"
            };

            var initialId = credentialDescription.Id;
            Assert.Contains("path1", initialId, StringComparison.Ordinal);

            credentialDescription.SignedAssertionFileDiskPath = "c:/path2.jwt";
            var updatedId = credentialDescription.Id;

            Assert.NotEqual(initialId, updatedId);
            Assert.Contains("path2", updatedId, StringComparison.Ordinal);
        }

        [Fact]
        public void CachedId_InvalidatedWhen_CustomSignedAssertionProviderName_Changes()
        {
            var credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.CustomSignedAssertion,
                CustomSignedAssertionProviderName = "Provider1"
            };

            var initialId = credentialDescription.Id;
            Assert.Contains("Provider1", initialId, StringComparison.Ordinal);

            credentialDescription.CustomSignedAssertionProviderName = "Provider2";
            var updatedId = credentialDescription.Id;

            Assert.NotEqual(initialId, updatedId);
            Assert.Contains("Provider2", updatedId, StringComparison.Ordinal);
        }

        [Fact]
        public void CachedId_InvalidatedWhen_CustomSignedAssertionProviderData_Changes()
        {
            var credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.CustomSignedAssertion,
                CustomSignedAssertionProviderName = "MyProvider",
                CustomSignedAssertionProviderData = new Dictionary<string, object>
                {
                    { "Key1", "Value1" }
                }
            };

            var initialId = credentialDescription.Id;
            Assert.Contains("Key1", initialId, StringComparison.Ordinal);

            credentialDescription.CustomSignedAssertionProviderData = new Dictionary<string, object>
            {
                { "Key2", "Value2" }
            };
            var updatedId = credentialDescription.Id;

            Assert.NotEqual(initialId, updatedId);
            Assert.Contains("Key2", updatedId, StringComparison.Ordinal);
        }

        [Fact]
        public void CachedId_InvalidatedWhen_ClientSecret_Changes()
        {
            var credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.ClientSecret,
                ClientSecret = "Secret1"
            };

            var initialId = credentialDescription.Id;

            credentialDescription.ClientSecret = "Secret2";
            var updatedId = credentialDescription.Id;

            Assert.NotEqual(initialId, updatedId);
        }

        [Fact]
        public void CachedId_InvalidatedWhen_Certificate_Changes()
        {
#pragma warning disable SYSLIB0026 // Type or member is obsolete
            var cert1 = new X509Certificate2(Convert.FromBase64String(base64encoded));
            var cert2 = new X509Certificate2(Convert.FromBase64String(base64encoded));

            var credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.Certificate,
                Certificate = cert1
            };

            var initialId = credentialDescription.Id;
            Assert.Contains(cert1.Thumbprint, initialId, StringComparison.Ordinal);

            credentialDescription.Certificate = cert2;
            var updatedId = credentialDescription.Id;

            // Even though thumbprints are the same, the cached ID should be recomputed
            Assert.Equal(initialId, updatedId); // Same cert, same thumbprint
            Assert.Contains(cert2.Thumbprint, updatedId, StringComparison.Ordinal);
#pragma warning restore SYSLIB0026 // Type or member is obsolete
        }

        [Fact]
        public void CachedId_InvalidatedWhen_CachedValue_Changes()
        {
            var credentialDescription = new CredentialDescription
            {
                SourceType = CredentialSource.ManagedCertificate,
                CachedValue = "Value1"
            };

            var initialId = credentialDescription.Id;
            Assert.Contains("Value1", initialId, StringComparison.Ordinal);

            credentialDescription.CachedValue = "Value2";
            var updatedId = credentialDescription.Id;

            Assert.NotEqual(initialId, updatedId);
            Assert.Contains("Value2", updatedId, StringComparison.Ordinal);
        }
    }
}
