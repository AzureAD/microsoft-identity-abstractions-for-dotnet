// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Description of a credential. Credentials are used to prove the identity of the
    /// application (See <see cref="IdentityApplicationOptions.ClientCredentials"/>), or
    /// to decrypt tokens (See <see cref="IdentityApplicationOptions.TokenDecryptionCredentials"/>).
    /// </summary>
    public class CredentialDescription
    {
        /// <summary>
        /// Type of the source of the credential.
        /// </summary>
        public CredentialSource SourceType { get; set; }

        /// <summary>
        /// Container in which to find the credential.
        /// <list type="bullet">
        /// <item>If <see cref="SourceType"/> equals <see cref="CredentialSource.KeyVault"/>, then
        /// the container is the Key Vault base URL.</item>
        /// <item>If <see cref="SourceType"/> equals <see cref="CredentialSource.Base64Encoded"/>, then
        /// this value is not used.</item>
        /// <item>If <see cref="SourceType"/> equals <see cref="CredentialSource.Path"/>, then
        /// this value is the path on disk where to find the credential.</item>
        /// <item>If <see cref="SourceType"/> equals <see cref="CredentialSource.StoreWithDistinguishedName"/>,
        /// or <see cref="CredentialSource.StoreWithThumbprint"/>, then
        /// this value is the path to the credential in the cert store, for instance <c>CurrentUser/My</c>.</item>
        /// </list>
        /// </summary>
        public string? Container
        {
            get
            {
                return SourceType switch
                {
                    CredentialSource.Certificate => null,
                    CredentialSource.KeyVault => KeyVaultUrl,
                    CredentialSource.Base64Encoded => CertificatePassword,
                    CredentialSource.Path => CertificateDiskPath,
                    CredentialSource.StoreWithThumbprint or CredentialSource.StoreWithDistinguishedName => CertificateStorePath,
                    CredentialSource.SignedAssertionFilePath => SignedAssertionFileDiskPath,
                    CredentialSource.SignedAssertionFromVault => KeyVaultUrl,
                    _ => null,
                };
            }
            set
            {
                switch (SourceType)
                {
                    case CredentialSource.Certificate:
                        break;
                    case CredentialSource.KeyVault:
                    case CredentialSource.SignedAssertionFromVault:
                        KeyVaultUrl = value;
                        break;
                    case CredentialSource.Base64Encoded:
                        // This is to avoid a breaking change (in v1.0 there was no password for
                        // base 64 encoded assertion, which was therefore the reference or value.
                        // We consider the password as a kind of enveloppe, hence being the container
                        CertificatePassword = value;
                        break;
                    case CredentialSource.Path:
                        CertificateDiskPath = value;
                        break;
                    case CredentialSource.StoreWithDistinguishedName:
                    case CredentialSource.StoreWithThumbprint:
                        CertificateStorePath = value;
                        break;
                    case CredentialSource.SignedAssertionFilePath:
                        SignedAssertionFileDiskPath = value;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// When <see cref="SourceType"/> is <see cref="CredentialSource.KeyVault"/>, use this property to specify the
        /// URL of the Key Vault containing the certificate, in conjunction with <see cref="KeyVaultCertificateName"/>.
        /// </summary>
        /// <example>
        /// <format type="text/markdown">
        /// <![CDATA[
        /// The Json fragment below describes a certificate stored in Key Vault used as a client credential in a confidential client application:
        /// :::code language="json" source="~/ /test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="keyvault_json":::
        /// 
        /// The code below describes programmatically in C#, the same certificate stored in Key Vault.
        /// :::code language="csharp" source="~/abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="keyvault_csharp":::
        /// ]]></format>
        /// </example>
        public string? KeyVaultUrl { get; set; }

        /// <summary>
        /// Certificate store path, for instance "CurrentUser/My".
        /// </summary>
        /// <remarks>This property should only be used in conjunction with DistinguishedName or Thumbprint.</remarks>
        public string? CertificateStorePath { get; set; }

        /// <summary>
        /// Certificate distinguished name.
        /// </summary>
        public string? CertificateDistinguishedName { get; set; }

        /// <summary>
        /// Name of the certificate in Key Vault.
        /// </summary>
        public string? KeyVaultCertificateName { get; set; }

        /// <summary>
        /// Certificate thumbprint.
        /// </summary>
        public string? CertificateThumbprint { get; set; }

        /// <summary>
        /// Path on disk to the certificate.
        /// </summary>
        public string? CertificateDiskPath { get; set; }

        /// <summary>
        /// Password with which the certificate on disk is protected.
        /// </summary>
        public string? CertificatePassword { get; set; }

        /// <summary>
        /// Base64 encoded certificate value.
        /// </summary>
        public string? Base64EncodedValue { get; set; }

        /// <summary>
        /// Client Secret.
        /// </summary>
        public string? ClientSecret { get; set; }

        /// <summary>
        /// ClientId of the Azure managed identity used to access the certificates from KeyVault
        /// (in the case of User assigned managed identity).
        /// </summary>
        public string? ManagedIdentityClientId { get; set; }

        /// <summary>
        /// Path on disk to the signed assertion (for Kubernetes).
        /// </summary>
        public string? SignedAssertionFileDiskPath { get; set; }

        /// <summary>
        /// Authentication options used to produce an authorization header to access the decrypt keys.
        /// This property is only used when <see cref="SourceType"/> is <see cref="CredentialSource.AutoDecryptKeys"/>, in order
        /// to determine the authority to use to get a token to get the decrypt the keys. The cloud
        /// instance will be the same as the application, but the application can be a multi-tenant
        /// application (tenant = <b>common</b> or <b>organizations</b>), and in this case to get a token, the
        /// credential type needs to provide a tenant. More generally you might want to specify 
        /// authentication options, including protocol, PopKey, etc ...
        /// </summary>
        public AuthorizationHeaderProviderOptions? DecryptKeysAuthenticationOptions { get; set; }

        /// <summary>
        /// Reference to the certificate or value.
        /// </summary>
        /// <list type="bullet">
        /// <item>If <see cref="SourceType"/> equals <see cref="CredentialSource.KeyVault"/>, then
        /// the reference is the name of the certificate in Key Vault (maybe the version?).</item>
        /// <item>If <see cref="SourceType"/> equals <see cref="CredentialSource.Base64Encoded"/>, then
        /// this value is the base 64 encoded certificate itself.</item>
        /// <item>If <see cref="SourceType"/> equals <see cref="CredentialSource.Path"/>, then
        /// this value is the password to access the certificate (if needed).</item>
        /// <item>If <see cref="SourceType"/> equals <see cref="CredentialSource.StoreWithDistinguishedName"/>,
        /// this value is the distinguished name.</item>
        /// <item>If <see cref="SourceType"/> equals <see cref="CredentialSource.StoreWithThumbprint"/>,
        /// this value is the thumbprint.</item>
        /// </list>
        public string? ReferenceOrValue
        {
            get
            {
                return SourceType switch
                {
                    CredentialSource.KeyVault => KeyVaultCertificateName,
                    CredentialSource.SignedAssertionFromVault => KeyVaultCertificateName,
                    CredentialSource.Path => CertificatePassword,
                    CredentialSource.StoreWithThumbprint => CertificateThumbprint,
                    CredentialSource.StoreWithDistinguishedName => CertificateDistinguishedName,
                    CredentialSource.Certificate or CredentialSource.Base64Encoded => Base64EncodedValue,
                    CredentialSource.SignedAssertionFromManagedIdentity => ManagedIdentityClientId,
                    CredentialSource.ClientSecret => ClientSecret,
                    _ => null,
                };
            }
            set
            {
                switch (SourceType)
                {
                    case CredentialSource.Certificate:
                        break;
                    case CredentialSource.KeyVault:
                        KeyVaultCertificateName = value;
                        break;
                    case CredentialSource.SignedAssertionFromVault:
                        KeyVaultCertificateName = value;
                        break;
                    case CredentialSource.Base64Encoded:
                        Base64EncodedValue = value;
                        break;
                    case CredentialSource.Path:
                        CertificatePassword = value;
                        break;
                    case CredentialSource.StoreWithThumbprint:
                        CertificateThumbprint = value;
                        break;
                    case CredentialSource.StoreWithDistinguishedName:
                        CertificateDistinguishedName = value;
                        break;
                    case CredentialSource.ClientSecret:
                        ClientSecret = value;
                        break;
                    case CredentialSource.SignedAssertionFromManagedIdentity:
                        ManagedIdentityClientId = value;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// The certificate, either provided directly in code
        /// or loaded from the description.
        /// </summary>
        public X509Certificate2? Certificate { get; set; }

        /// <summary>
        /// Cached value for the credential
        /// </summary>
        public virtual object? CachedValue { get; set; }

        /// <summary>
        /// Skip this credential. This is useful when, you specify a list of
        /// credentials, some of which don't apply in a particular deployment.
        /// </summary>
        public bool Skip { get; set; }

        /// <summary>
        /// Describes the type of credentials
        /// </summary>
        public CredentialType CredentialType
        {
            get
            {
                return SourceType switch
                {
                    CredentialSource.KeyVault 
                    or CredentialSource.Path 
                    or CredentialSource.StoreWithThumbprint 
                    or CredentialSource.StoreWithDistinguishedName 
                    or CredentialSource.Certificate 
                    or CredentialSource.Base64Encoded => CredentialType.Certificate,
                    
                    CredentialSource.ClientSecret => CredentialType.Secret,
                    
                    CredentialSource.SignedAssertionFromManagedIdentity 
                    or CredentialSource.SignedAssertionFilePath 
                    or CredentialSource.SignedAssertionFromVault => CredentialType.SignedAssertion,

                    CredentialSource.AutoDecryptKeys => CredentialType.DecryptKeys,

                    _ => default,
                };
            }
        }
    }
}
