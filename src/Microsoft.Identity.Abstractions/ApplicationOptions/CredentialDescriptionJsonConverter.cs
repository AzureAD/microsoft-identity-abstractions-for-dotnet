// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

#if NET8_0_OR_GREATER
using System;
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
                    case "sourcetype":
                        string sourceType = reader.GetString()!;
                        credentialDescription.SourceType = Enum.Parse<CredentialSource>(sourceType);
                        break;
                    case "base64encodedvalue":
                        credentialDescription.Base64EncodedValue = reader.GetString();
                        break;
                    case "certificatestorepath":
                        credentialDescription.CertificateStorePath = reader.GetString();
                        break;
                    case "certificatedistinguishedname":
                        credentialDescription.CertificateDistinguishedName = reader.GetString();
                        break;
                    case "certificatethumbprint":
                        credentialDescription.CertificateThumbprint = reader.GetString();
                        break;
                    case "certificatediskpath":
                        credentialDescription.CertificateDiskPath = reader.GetString();
                        break;
                    case "certificatepassword":
                        credentialDescription.CertificatePassword = reader.GetString();
                        break;
                    case "clientsecret":
                        credentialDescription.ClientSecret = reader.GetString();
                        break;
                    case "managedidentityclientid":
                        credentialDescription.ManagedIdentityClientId = reader.GetString();
                        break;
                    case "signedassertionfilediskpath":
                        credentialDescription.SignedAssertionFileDiskPath = reader.GetString();
                        break;
                    case "keyvaulturl":
                        credentialDescription.KeyVaultUrl = reader.GetString();
                        break;
                    case "keyvaultcertificatename":
                        credentialDescription.KeyVaultCertificateName = reader.GetString();
                        break;
                    case "tokenexchangeurl":
                        credentialDescription.TokenExchangeUrl = reader.GetString();
                        break;
                    case "tokenexchangeauthority":
                        credentialDescription.TokenExchangeAuthority = reader.GetString();
                        break;
                    case "skip":
                        credentialDescription.Skip = reader.GetBoolean();
                        break;
                    case "customsignedassertionprovidername":
                        credentialDescription.CustomSignedAssertionProviderName = reader.GetString();
                        break;
                    case "customsignedassertionproviderdata":
                        credentialDescription.CustomSignedAssertionProviderData =
                            JsonSerializer.Deserialize<System.Collections.Generic.Dictionary<string, object>>(ref reader, options);
                        break;
                    case "decryptkeysauthenticationoptions":
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
                        writer.WriteString("Base64EncodedValue", value.Base64EncodedValue);
                    break;

                case CredentialSource.Path:
                    if (!string.IsNullOrEmpty(value.CertificateDiskPath))
                        writer.WriteString("CertificateDiskPath", value.CertificateDiskPath);
                    if (!string.IsNullOrEmpty(value.CertificatePassword))
                        writer.WriteString("CertificatePassword", value.CertificatePassword);
                    break;

                case CredentialSource.StoreWithThumbprint:
                    if (!string.IsNullOrEmpty(value.CertificateStorePath))
                        writer.WriteString("CertificateStorePath", value.CertificateStorePath);
                    if (!string.IsNullOrEmpty(value.CertificateThumbprint))
                        writer.WriteString("CertificateThumbprint", value.CertificateThumbprint);
                    break;

                case CredentialSource.StoreWithDistinguishedName:
                    if (!string.IsNullOrEmpty(value.CertificateStorePath))
                        writer.WriteString("CertificateStorePath", value.CertificateStorePath);
                    if (!string.IsNullOrEmpty(value.CertificateDistinguishedName))
                        writer.WriteString("CertificateDistinguishedName", value.CertificateDistinguishedName);
                    break;

                case CredentialSource.KeyVault:
                    if (!string.IsNullOrEmpty(value.KeyVaultUrl))
                        writer.WriteString("KeyVaultUrl", value.KeyVaultUrl);
                    if (!string.IsNullOrEmpty(value.KeyVaultCertificateName))
                        writer.WriteString("KeyVaultCertificateName", value.KeyVaultCertificateName);
                    break;

                case CredentialSource.ClientSecret:
                    if (!string.IsNullOrEmpty(value.ClientSecret))
                        writer.WriteString("ClientSecret", value.ClientSecret);
                    break;

                case CredentialSource.SignedAssertionFromManagedIdentity:
                    if (!string.IsNullOrEmpty(value.ManagedIdentityClientId))
                        writer.WriteString("ManagedIdentityClientId", value.ManagedIdentityClientId);
                    if (!string.IsNullOrEmpty(value.TokenExchangeUrl))
                        writer.WriteString("TokenExchangeUrl", value.TokenExchangeUrl);
                    if (!string.IsNullOrEmpty(value.TokenExchangeAuthority))
                        writer.WriteString("TokenExchangeAuthority", value.TokenExchangeAuthority);
                    break;

                case CredentialSource.SignedAssertionFilePath:
                    if (!string.IsNullOrEmpty(value.SignedAssertionFileDiskPath))
                        writer.WriteString("SignedAssertionFileDiskPath", value.SignedAssertionFileDiskPath);
                    break;

                case CredentialSource.AutoDecryptKeys:
                    if (value.DecryptKeysAuthenticationOptions != null)
                    {
                        writer.WritePropertyName("DecryptKeysAuthenticationOptions");
                        JsonSerializer.Serialize(writer, value.DecryptKeysAuthenticationOptions, options);
                    }
                    break;

                case CredentialSource.CustomSignedAssertion:
                    if (!string.IsNullOrEmpty(value.CustomSignedAssertionProviderName))
                        writer.WriteString("CustomSignedAssertionProviderName", value.CustomSignedAssertionProviderName);
                    if (value.CustomSignedAssertionProviderData != null)
                    {
                        writer.WritePropertyName("CustomSignedAssertionProviderData");
                        JsonSerializer.Serialize(writer, value.CustomSignedAssertionProviderData, options);
                    }
                    break;
            }

            writer.WriteEndObject();
        }
    }
}
#endif
