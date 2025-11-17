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
    }
}
