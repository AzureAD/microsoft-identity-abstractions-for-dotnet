// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

#if NET8_0_OR_GREATER
using System.Text.Json;
using Xunit;

namespace Microsoft.Identity.Abstractions.ApplicationOptions.Tests
{
    public class CredentialDescriptionJsonConverterTest
    {
        private readonly JsonSerializerOptions _options;

        public CredentialDescriptionJsonConverterTest()
        {
            _options = new JsonSerializerOptions
            {
                Converters = { new CredentialDescriptionJsonConverter() }
            };
        }

        [Fact]
        public void SerializeDeserialize_Base64Encoded()
        {
            var original = new CredentialDescription
            {
                SourceType = CredentialSource.Base64Encoded,
                Base64EncodedValue = "MIIDHzCgegA.....r1n8Ta0="
            };

            string json = JsonSerializer.Serialize(original, _options);
            var deserialized = JsonSerializer.Deserialize<CredentialDescription>(json, _options);
        
            Assert.NotNull(deserialized);
            Assert.Equal(original.SourceType, deserialized.SourceType);
            Assert.Equal(original.Base64EncodedValue, deserialized.Base64EncodedValue);
            Assert.Equal(CredentialType.Certificate, deserialized.CredentialType);
        }

        [Fact]
        public void SerializeDeserialize_CertificateFromPath()
        {
            var original = new CredentialDescription
            {
                SourceType = CredentialSource.Path,
                CertificateDiskPath = @"c:\temp\WebAppCallingWebApiCert.pfx",
                CertificatePassword = "password"
            };

            string json = JsonSerializer.Serialize(original, _options);
            var deserialized = JsonSerializer.Deserialize<CredentialDescription>(json, _options);
            Assert.NotNull(deserialized);
            Assert.Equal(original.SourceType, deserialized.SourceType);
            Assert.Equal(original.CertificateDiskPath, deserialized.CertificateDiskPath);
            Assert.Equal(original.CertificatePassword, deserialized.CertificatePassword);
            Assert.Equal(CredentialType.Certificate, deserialized.CredentialType);
        }

        [Fact]
        public void SerializeDeserialize_CertificateFromStoreByThumbprint()
        {
            var original = new CredentialDescription
            {
                SourceType = CredentialSource.StoreWithThumbprint,
                CertificateStorePath = "LocalMachine/My",
                CertificateThumbprint = "962D129A...D18EFEB6961684"
            };

            string json = JsonSerializer.Serialize(original, _options);
            var deserialized = JsonSerializer.Deserialize<CredentialDescription>(json, _options);

            Assert.NotNull(deserialized);
            Assert.Equal(original.SourceType, deserialized.SourceType);
            Assert.Equal(original.CertificateStorePath, deserialized.CertificateStorePath);
            Assert.Equal(original.CertificateThumbprint, deserialized.CertificateThumbprint);
            Assert.Equal(CredentialType.Certificate, deserialized.CredentialType);
        }

        [Fact]
        public void SerializeDeserialize_CertificateFromKeyVault()
        {
            var original = new CredentialDescription
            {
                SourceType = CredentialSource.KeyVault,
                KeyVaultUrl = "https://msidentitywebsamples.vault.azure.net",
                KeyVaultCertificateName = "MicrosoftIdentitySamplesCert"
            };

            string json = JsonSerializer.Serialize(original, _options);
            var deserialized = JsonSerializer.Deserialize<CredentialDescription>(json, _options);

            Assert.NotNull(deserialized);
            Assert.Equal(original.SourceType, deserialized.SourceType);
            Assert.Equal(original.KeyVaultUrl, deserialized.KeyVaultUrl);
            Assert.Equal(original.KeyVaultCertificateName, deserialized.KeyVaultCertificateName);
            Assert.Equal(CredentialType.Certificate, deserialized.CredentialType);
        }

        [Fact]
        public void SerializeDeserialize_Secret()
        {
            var original = new CredentialDescription
            {
                SourceType = CredentialSource.ClientSecret,
                ClientSecret = "blah"
            };

            string json = JsonSerializer.Serialize(original, _options);
            var deserialized = JsonSerializer.Deserialize<CredentialDescription>(json, _options);

            Assert.NotNull(deserialized);
            Assert.Equal(original.SourceType, deserialized.SourceType);
            Assert.Equal(original.ClientSecret, deserialized.ClientSecret);
            Assert.Equal(CredentialType.Secret, deserialized.CredentialType);
        }

        [Fact]
        public void SerializeDeserialize_SignedAssertionFromMSI()
        {
            var original = new CredentialDescription
            {
                SourceType = CredentialSource.SignedAssertionFromManagedIdentity,
                ManagedIdentityClientId = "12345"
            };

            string json = JsonSerializer.Serialize(original, _options);
            var deserialized = JsonSerializer.Deserialize<CredentialDescription>(json, _options);

            Assert.NotNull(deserialized);
            Assert.Equal(original.SourceType, deserialized.SourceType);
            Assert.Equal(original.ManagedIdentityClientId, deserialized.ManagedIdentityClientId);
            Assert.Equal(CredentialType.SignedAssertion, deserialized.CredentialType);
        }

        [Fact]
        public void SerializeDeserialize_AutoDecryptKeys()
        {
            var original = new CredentialDescription
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
            };

            string json = JsonSerializer.Serialize(original, _options);
            var deserialized = JsonSerializer.Deserialize<CredentialDescription>(json, _options);

            Assert.NotNull(deserialized);
            Assert.Equal(original.SourceType, deserialized.SourceType);
            Assert.Equal(original.DecryptKeysAuthenticationOptions.ProtocolScheme, deserialized.DecryptKeysAuthenticationOptions?.ProtocolScheme);
            Assert.Equal(original.DecryptKeysAuthenticationOptions.AcquireTokenOptions.Tenant, deserialized.DecryptKeysAuthenticationOptions?.AcquireTokenOptions.Tenant);
            Assert.Equal(CredentialType.DecryptKeys, deserialized.CredentialType);
        }

        [Fact]
        public void SerializeDeserialize_CustomSignedAssertion()
        {
            var original = new CredentialDescription
            {
                SourceType = CredentialSource.CustomSignedAssertion,
                CustomSignedAssertionProviderName = "MyCustomProvider",
                CustomSignedAssertionProviderData = new System.Collections.Generic.Dictionary<string, object>
                {
                    { "MyCustomProviderData_Key", "MyCustomProviderData_Data" }
                }
            };

            string json = JsonSerializer.Serialize(original, _options);
            var deserialized = JsonSerializer.Deserialize<CredentialDescription>(json, _options);

            Assert.NotNull(deserialized);
            Assert.Equal(original.SourceType, deserialized.SourceType);
            Assert.Equal(original.CustomSignedAssertionProviderName, deserialized.CustomSignedAssertionProviderName);
            Assert.Equal(original.CustomSignedAssertionProviderData["MyCustomProviderData_Key"].ToString(),
                deserialized.CustomSignedAssertionProviderData?["MyCustomProviderData_Key"].ToString());
            Assert.Equal(CredentialType.SignedAssertion, deserialized.CredentialType);
        }
    }
}
#endif
