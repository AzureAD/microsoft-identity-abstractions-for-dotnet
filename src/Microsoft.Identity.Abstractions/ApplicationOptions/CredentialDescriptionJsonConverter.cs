// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

#if NET8_0_OR_GREATER
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Custom JSON converter for CredentialDescription to handle proper serialization/deserialization
    /// of credential information based on SourceType.
    /// </summary>
    /// <remarks>BE CAREFUL when you serialize a credential description. The secrets it contains
    /// will be serialized too depending on the CredentialSource (ClientSecret, Base64Encoded, and Password)</remarks>
    [RequiresDynamicCode("Uses JsonSerializer which may require dynamic code generation for certain types.")]
    [RequiresUnreferencedCode("Uses JsonSerializer which may require dynamic code generation for certain types.")]
    public class CredentialDescriptionJsonConverter : JsonConverter<CredentialDescription>
    {
        /// <inheritdoc/>
        public override CredentialDescription Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected start of object");
            }

            var credentialDescription = new CredentialDescription();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return credentialDescription;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException("Expected property name");
                }

                string propertyName = reader.GetString()!;
                reader.Read();

                switch (propertyName.ToLowerInvariant())
                {
                    case string _ when propertyName.Equals(nameof(CredentialDescription.SourceType), StringComparison.OrdinalIgnoreCase):
                        string sourceType = reader.GetString()!;
                        credentialDescription.SourceType = Enum.Parse<CredentialSource>(sourceType);
                        break;
                    case string _ when propertyName.Equals(nameof(CredentialDescription.Base64EncodedValue), StringComparison.OrdinalIgnoreCase):
                        credentialDescription.Base64EncodedValue = reader.GetString();
                        break;
                    case string _ when propertyName.Equals(nameof(CredentialDescription.CertificateStorePath), StringComparison.OrdinalIgnoreCase):
                        credentialDescription.CertificateStorePath = reader.GetString();
                        break;
                    case string _ when propertyName.Equals(nameof(CredentialDescription.CertificateDistinguishedName), StringComparison.OrdinalIgnoreCase):
                        credentialDescription.CertificateDistinguishedName = reader.GetString();
                        break;
                    case string _ when propertyName.Equals(nameof(CredentialDescription.CertificateThumbprint), StringComparison.OrdinalIgnoreCase):
                        credentialDescription.CertificateThumbprint = reader.GetString();
                        break;
                    case string _ when propertyName.Equals(nameof(CredentialDescription.CertificateDiskPath), StringComparison.OrdinalIgnoreCase):
                        credentialDescription.CertificateDiskPath = reader.GetString();
                        break;
                    case string _ when propertyName.Equals(nameof(CredentialDescription.CertificatePassword), StringComparison.OrdinalIgnoreCase):
                        credentialDescription.CertificatePassword = reader.GetString();
                        break;
                    case string _ when propertyName.Equals(nameof(CredentialDescription.ClientSecret), StringComparison.OrdinalIgnoreCase):
                        credentialDescription.ClientSecret = reader.GetString();
                        break;
                    case string _ when propertyName.Equals(nameof(CredentialDescription.ManagedIdentityClientId), StringComparison.OrdinalIgnoreCase):
                        credentialDescription.ManagedIdentityClientId = reader.GetString();
                        break;
                    case string _ when propertyName.Equals(nameof(CredentialDescription.SignedAssertionFileDiskPath), StringComparison.OrdinalIgnoreCase):
                        credentialDescription.SignedAssertionFileDiskPath = reader.GetString();
                        break;
                    case string _ when propertyName.Equals(nameof(CredentialDescription.KeyVaultUrl), StringComparison.OrdinalIgnoreCase):
                        credentialDescription.KeyVaultUrl = reader.GetString();
                        break;
                    case string _ when propertyName.Equals(nameof(CredentialDescription.KeyVaultCertificateName), StringComparison.OrdinalIgnoreCase):
                        credentialDescription.KeyVaultCertificateName = reader.GetString();
                        break;
                    case string _ when propertyName.Equals(nameof(CredentialDescription.TokenExchangeUrl), StringComparison.OrdinalIgnoreCase):
                        credentialDescription.TokenExchangeUrl = reader.GetString();
                        break;
                    case string _ when propertyName.Equals(nameof(CredentialDescription.TokenExchangeAuthority), StringComparison.OrdinalIgnoreCase):
                        credentialDescription.TokenExchangeAuthority = reader.GetString();
                        break;
                    case string _ when propertyName.Equals(nameof(CredentialDescription.Skip), StringComparison.OrdinalIgnoreCase):
                        credentialDescription.Skip = reader.GetBoolean();
                        break;
                    case string _ when propertyName.Equals(nameof(CredentialDescription.CustomSignedAssertionProviderName), StringComparison.OrdinalIgnoreCase):
                        credentialDescription.CustomSignedAssertionProviderName = reader.GetString();
                        break;
                    case string _ when propertyName.Equals(nameof(CredentialDescription.CustomSignedAssertionProviderData), StringComparison.OrdinalIgnoreCase):
                        credentialDescription.CustomSignedAssertionProviderData =
                            JsonSerializer.Deserialize<System.Collections.Generic.Dictionary<string, object>>(ref reader, options);
                        break;
                    case string _ when propertyName.Equals(nameof(CredentialDescription.DecryptKeysAuthenticationOptions), StringComparison.OrdinalIgnoreCase):
                        credentialDescription.DecryptKeysAuthenticationOptions =
                            JsonSerializer.Deserialize<AuthorizationHeaderProviderOptions>(ref reader, options);
                        break;
                }
            }

            throw new JsonException("Expected end of object");
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, CredentialDescription value, JsonSerializerOptions options)
        {
            ArgumentNullException.ThrowIfNull(writer);
            ArgumentNullException.ThrowIfNull(value);
            writer.WriteStartObject();

            writer.WritePropertyName("SourceType");
            JsonSerializer.Serialize(writer, Enum.GetName(value.SourceType), options);

            switch (value.SourceType)
            {
                case CredentialSource.Base64Encoded:
                    if (!string.IsNullOrEmpty(value.Base64EncodedValue))
                        writer.WriteString(nameof(CredentialDescription.Base64EncodedValue), value.Base64EncodedValue);
                    break;

                case CredentialSource.Path:
                    if (!string.IsNullOrEmpty(value.CertificateDiskPath))
                        writer.WriteString(nameof(CredentialDescription.CertificateDiskPath), value.CertificateDiskPath);
                    if (!string.IsNullOrEmpty(value.CertificatePassword))
                        writer.WriteString(nameof(CredentialDescription.CertificatePassword), value.CertificatePassword);
                    break;

                case CredentialSource.StoreWithThumbprint:
                    if (!string.IsNullOrEmpty(value.CertificateStorePath))
                        writer.WriteString(nameof(CredentialDescription.CertificateStorePath), value.CertificateStorePath);
                    if (!string.IsNullOrEmpty(value.CertificateThumbprint))
                        writer.WriteString(nameof(CredentialDescription.CertificateThumbprint), value.CertificateThumbprint);
                    break;

                case CredentialSource.StoreWithDistinguishedName:
                    if (!string.IsNullOrEmpty(value.CertificateStorePath))
                        writer.WriteString(nameof(CredentialDescription.CertificateStorePath), value.CertificateStorePath);
                    if (!string.IsNullOrEmpty(value.CertificateDistinguishedName))
                        writer.WriteString(nameof(CredentialDescription.CertificateDistinguishedName), value.CertificateDistinguishedName);
                    break;

                case CredentialSource.KeyVault:
                    if (!string.IsNullOrEmpty(value.KeyVaultUrl))
                        writer.WriteString(nameof(CredentialDescription.KeyVaultUrl), value.KeyVaultUrl);
                    if (!string.IsNullOrEmpty(value.KeyVaultCertificateName))
                        writer.WriteString(nameof(CredentialDescription.KeyVaultCertificateName), value.KeyVaultCertificateName);
                    break;

                case CredentialSource.ClientSecret:
                    if (!string.IsNullOrEmpty(value.ClientSecret))
                        writer.WriteString(nameof(CredentialDescription.ClientSecret), value.ClientSecret);
                    break;

                case CredentialSource.SignedAssertionFromManagedIdentity:
                    if (!string.IsNullOrEmpty(value.ManagedIdentityClientId))
                        writer.WriteString(nameof(CredentialDescription.ManagedIdentityClientId), value.ManagedIdentityClientId);
                    if (!string.IsNullOrEmpty(value.TokenExchangeUrl))
                        writer.WriteString(nameof(CredentialDescription.TokenExchangeUrl), value.TokenExchangeUrl);
                    if (!string.IsNullOrEmpty(value.TokenExchangeAuthority))
                        writer.WriteString(nameof(CredentialDescription.TokenExchangeAuthority), value.TokenExchangeAuthority);
                    break;

                case CredentialSource.SignedAssertionFilePath:
                    if (!string.IsNullOrEmpty(value.SignedAssertionFileDiskPath))
                        writer.WriteString(nameof(CredentialDescription.SignedAssertionFileDiskPath), value.SignedAssertionFileDiskPath);
                    break;

                case CredentialSource.AutoDecryptKeys:
                    if (value.DecryptKeysAuthenticationOptions != null)
                    {
                        writer.WritePropertyName(nameof(CredentialDescription.DecryptKeysAuthenticationOptions));
                        JsonSerializer.Serialize(writer, value.DecryptKeysAuthenticationOptions, options);
                    }
                    break;

                case CredentialSource.CustomSignedAssertion:
                    if (!string.IsNullOrEmpty(value.CustomSignedAssertionProviderName))
                        writer.WriteString(nameof(CredentialDescription.CustomSignedAssertionProviderName), value.CustomSignedAssertionProviderName);
                    if (value.CustomSignedAssertionProviderData != null)
                    {
                        writer.WritePropertyName(nameof(CredentialDescription.CustomSignedAssertionProviderData));
                        JsonSerializer.Serialize(writer, value.CustomSignedAssertionProviderData, options);
                    }
                    break;
            }

            writer.WriteEndObject();
        }
    }
}
#endif
