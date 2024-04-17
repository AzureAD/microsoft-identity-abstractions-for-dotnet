// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Description of a credential. Credentials are used to prove the identity of the
    /// application (See <see cref="IdentityApplicationOptions.ClientCredentials"/>), or
    /// to decrypt tokens (See <see cref="IdentityApplicationOptions.TokenDecryptionCredentials"/>). Credentials can be
    /// secrets (client secrets), certificates, or signed assertions. They can be stored or provided in a variety of ways, 
    /// and this class provides a way to describe them. The description is then used by Microsoft.Identity.Web to retrieve the credential.
    /// (See the DefaultCredentialProvider class)
    /// </summary>
    public class CredentialDescription
    {
        private string? _cachedId;

        /// <summary>
        /// Gets a unique identifier for a CredentialDescription based on <see cref="SourceType"/> and <see cref="ReferenceOrValue"/>.
        /// </summary>
        public string Id
        {
            get
            {
                if (_cachedId == null)
                    _cachedId = $"{SourceType}_{Container}_{ReferenceOrValue}";

                return _cachedId;
            }
        }

        /// <summary>
        /// Type of the source of the credential. This property is used to determine which other properties need
        /// to be provided to describe the credential.
        /// </summary>
        public CredentialSource SourceType { get; set; }

        /// <summary>
        /// Container in which to find the credential. You will normally not use this property directly. It could be used
        /// by property editors in tools or IDEs. Instead, use the properties that are specific to the <see cref="SourceType"/>.
        /// </summary>
        /// <remarks>
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
        /// </remarks>
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
                    _ => null
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
        /// The JSON fragment below describes a certificate stored in Key Vault used as a client credential in a confidential client application:
        /// :::code language="json" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="keyvault_json":::
        /// 
        /// The code below describes programmatically in C#, the same certificate stored in Key Vault.
        /// :::code language="csharp" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="keyvault_csharp":::
        /// ]]></format>
        /// </example>
        /// <seealso cref="SourceType"/>
        /// <seealso cref="KeyVaultCertificateName"/>
        public string? KeyVaultUrl { get; set; }

        /// <summary>
        /// When <see cref="SourceType"/> is <see cref="CredentialSource.StoreWithDistinguishedName"/> or 
        /// <see cref="CredentialSource.StoreWithThumbprint"/>, specifies the certificate store from which to extract
        /// the certificate. The format is the concatenation of a value of <see cref="StoreLocation"/> and a value of <see cref="StoreName"/>
        /// separated by a slash. For instance, use <c>CurrentUser/My</c> for a user certificate, and <c>LocalMachine/My</c> for a computer certificate.
        /// </summary>
        /// <remarks>Use this property in conjunction with <see cref="CertificateDistinguishedName"/> or <see cref="CertificateThumbprint"/>.</remarks>
        /// <seealso cref="SourceType"/>
        /// <seealso cref="CertificateStorePath"/>
        /// <seealso cref="CertificateDistinguishedName"/>
        /// <seealso cref="CertificateThumbprint"/>
        public string? CertificateStorePath { get; set; }

        /// <summary>
        /// When <see cref="SourceType"/> is <see cref="CredentialSource.StoreWithDistinguishedName"/>, specifies the distinguished name of
        /// the certificate in the store specified by <see cref="CertificateStorePath"/>.
        /// </summary>
        /// <example>
        /// <format type="text/markdown">
        /// <![CDATA[
        /// The JSON fragment below describes a user certificate stored in the personal certificates folder (<b>CurrentUser/My</b>) and specified by its distinguised name, used as a client credential in a confidential client application:
        /// :::code language="json" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="distinguishedname_json":::
        /// 
        /// The code below describes programmatically in C#, a computer certificate in the personal certificates folder (<b>LocalMachine/My<b>).
        /// :::code language="csharp" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="distinguishedname_csharp":::
        /// ]]></format>
        /// </example>
        /// <seealso cref="SourceType"/>
        /// <seealso cref="CertificateStorePath"/>
        /// <seealso cref="CertificateThumbprint"/>
        public string? CertificateDistinguishedName { get; set; }

        /// <summary>
        /// When <see cref="SourceType"/> is <see cref="CredentialSource.KeyVault"/>, use this property to specify the
        /// the name of the certificate in Key Vault in conjunction with <see cref="KeyVaultUrl"/>.
        /// </summary>
        /// <example>
        /// <format type="text/markdown">
        /// <![CDATA[
        /// The JSON fragment below describes a certificate stored in Key Vault used as a client credential in a confidential client application:
        /// :::code language="json" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="keyvault_json":::
        /// 
        /// The code below describes programmatically in C#, the same certificate stored in Key Vault.
        /// :::code language="csharp" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="keyvault_csharp":::
        /// ]]></format>
        /// </example>
        public string? KeyVaultCertificateName { get; set; }

        /// <summary>
        /// When <see cref="SourceType"/> is <see cref="CredentialSource.StoreWithThumbprint"/> specifies the thumbprint of the certificate to extract from
        /// the certificate store specified by <see cref="CertificateStorePath"/>.
        /// </summary>
        /// <example>
        /// <format type="text/markdown">
        /// <![CDATA[
        /// The JSON fragment below describes a user certificate stored in the personal certificates folder (<b>CurrentUser/My</b>) and specified by its thumbprint, used as a client credential in a confidential client application:
        /// :::code language="json" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="thumbprint_json":::
        /// 
        /// The code below describes programmatically in C#, a computer certificate in the personal certificates folder (<b>LocalMachine/My<b>) accessed by its thumbprint.
        /// :::code language="csharp" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="thumbprint_csharp":::
        /// ]]></format>
        /// </example>
        /// <remarks>Use this property in conjunction with <see cref="CertificateStorePath"/>.</remarks>
        /// <seealso cref="SourceType"/>
        /// <seealso cref="CertificateDistinguishedName"/>
        /// <seealso cref="CertificateStorePath"/>
        public string? CertificateThumbprint { get; set; }

        /// <summary>
        /// When <see cref="SourceType"/> is <see cref="CredentialSource.Path"/>, specifies the path to the certificate on disk. You can
        /// use this property to specify the path to a PFX file containing the certificate and its private key. If a password is needed, 
        /// use <see cref="CertificatePassword"/>.
        /// </summary>
        /// <example>
        /// <format type="text/markdown">
        /// <![CDATA[
        /// The JSON fragment below describes a certificate retrieved by its path and a password to be used as a client credential in a confidential client application:
        /// :::code language="json" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="path_json":::
        /// 
        /// The code below describes programmatically in C#, a the same certificate.
        /// :::code language="csharp" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="path_csharp":::
        /// ]]></format>
        /// </example>
        /// <seealso cref="SourceType"/>
        /// <seealso cref="CertificatePassword"/>
        public string? CertificateDiskPath { get; set; }

        /// <summary>
        /// When <see cref="SourceType"/> is <see cref="CredentialSource.Path"/>, specifies the password to use to access the certificate which
        /// path is specified by <see cref="CertificateDiskPath"/>. Only use this property if the certificate is protected by a password.
        /// </summary>
        /// <example>
        /// <format type="text/markdown">
        /// <![CDATA[
        /// The JSON fragment below describes a certificate retrieved by its path and a password to be used as a client credential in a confidential client application:
        /// :::code language="json" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="path_json":::
        /// 
        /// The code below describes programmatically in C#, the same certificate.
        /// :::code language="csharp" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="path_csharp":::
        /// ]]></format>
        /// </example>
        /// <seealso cref="SourceType"/>
        /// <seealso cref="CertificateDiskPath"/>
        public string? CertificatePassword { get; set; }

        /// <summary>
        /// When <see cref="SourceType"/> is <see cref="CredentialSource.Base64Encoded"/>, specifies the base64 encoded value of the certificate.
        /// </summary>
        /// <example>
        /// <format type="text/markdown">
        /// <![CDATA[
        /// The JSON fragment below describes a certificate by its base64 encoded value, to be used as a client credential in a confidential client application:
        /// :::code language="json" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="base64_json":::
        /// 
        /// The code below describes programmatically in C#, the same certificate.
        /// :::code language="csharp" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="base64_csharp":::
        /// ]]></format>
        /// </example>
        public string? Base64EncodedValue { get; set; }

        /// <summary>
        /// When <see cref="SourceType"/> is <see cref="CredentialSource.ClientSecret"/>, describes the client secret to use as a client credential in a confidential client application.
        /// The client secret is a string known only to the application and the identity provider. It needs to match the value configured during the application registration.
        /// </summary>
        /// <example>
        /// <format type="text/markdown">
        /// <![CDATA[
        /// The JSON fragment below describes a client secret used as a client credential in a confidential client application:
        /// :::code language="json" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="secret_json":::
        /// 
        /// The code below describes programmatically in C#, the same client secret.
        /// :::code language="csharp" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="secret_csharp":::
        /// ]]></format>
        /// </example>
        public string? ClientSecret { get; set; }

        /// <summary>
        /// When <see cref="SourceType"/> is <see cref="CredentialSource.SignedAssertionFromManagedIdentity"/>, it specifies the client ID of the Azure user-assigned managed identity 
        /// used to provide a signed assertion to act as a client credential for the application. This requires that the application is deployed on Azure, that the managed identity is configured, 
        /// and that workload identity federation with the managed identity is declared in the application registration. For details, see https://learn.microsoft.com/azure/active-directory/workload-identities/workload-identity-federation.
        /// </summary>
        /// <example>
        /// <format type="text/markdown">
        /// <![CDATA[
        /// The JSON fragment below describes a workload identity federation with a user assigned managed identity:
        /// :::code language="json" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="msi_json":::
        /// 
        /// The code below describes programmatically in C#, the same workload identity federation with a user assigned managed identity.
        /// :::code language="csharp" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="msi_csharp":::
        /// ]]></format>
        /// </example>        
        /// <remarks>If you want to use the system-assigned managed identity, just use <see cref="SourceType"/> = <see cref="CredentialSource.SignedAssertionFromManagedIdentity"/> and
        /// don't provide a managed identity client ID.</remarks>
        public string? ManagedIdentityClientId { get; set; }

        /// <summary>
        /// When <see cref="SourceType"/> is <see cref="CredentialSource.SignedAssertionFilePath"/>, optionally specifies the path on disk of a file 
        /// containing a signed assertion used as a client assertion for the confidential client application. 
        /// The signed assertion file is a file containing a signed JWT assertion that is used as a client credential. You will usually use this option when you want to integrate
        /// with workload identity federation with Azure Kubernetes Service (AKS). 
        /// For details, see https://learn.microsoft.com/azure/active-directory/workload-identities/workload-identity-federation.
        /// </summary>
        /// <example>
        /// <format type="text/markdown">
        /// <![CDATA[
        /// The JSON fragment below describes a signed assertion acquired with workload identity federation with Azure Kubernetes Services (AKS):
        /// :::code language="json" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="aks_json":::
        /// 
        /// The code below describes programmatically in C#, the same workload identity federation with with Azure Kubernetes Services (AKS) signed assertion.
        /// :::code language="csharp" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="aks_csharp":::
        /// ]]></format>
        /// </example>
        /// <remarks>When deployed to AKS, if you specify <see cref="SourceType"/> = <see cref="CredentialSource.SignedAssertionFilePath"/> but don't provide
        /// the signed assertion file disk path, the file will be searched based on the content of two environment variables: 
        /// <b>AZURE_FEDERATED_TOKEN_FILE</b> and <b>AZURE_ACCESS_TOKEN_FILE</b>.</remarks>
        public string? SignedAssertionFileDiskPath { get; set; }

        /// <summary>
        /// When <see cref="SourceType"/> is <see cref="CredentialSource.AutoDecryptKeys"/>, this property describes the authority to use
        /// to get a token for a web API to get the keys used to decrypt an encrypted token. The cloud instance will be the same as the application, but the application can be a multi-tenant
        /// application (tenant = <b>common</b> or <b>organizations</b>), and in this case to get a token on behalf of itself, the
        /// credential type needs to provide a tenant. More generally you might want to specify authentication options, including protocol, PopKey, etc ...
        /// This credential description is only used for decrypt credentials, not for client credentials.
        /// </summary>
        /// <example>
        /// <format type="text/markdown">
        /// <![CDATA[
        /// The JSON fragment below describes a decrypt credential to get the decrypt keys automatically:
        /// :::code language="json" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="autodecryp_json":::
        /// 
        /// The code below describes the same, programmatically in C#.
        /// :::code language="csharp" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="autodecryp_csharp":::
        /// ]]></format>
        /// </example>
        public AuthorizationHeaderProviderOptions? DecryptKeysAuthenticationOptions { get; set; }

        /// <summary>
        /// Reference to the certificate or value. You will normally not use this property directly. It could be used
        /// by property editors in tools or IDEs. Instead, use the properties that are specific to the <see cref="SourceType"/>.
        /// </summary>
        /// <remarks>
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
        /// </remarks>
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
        /// When <see cref="SourceType"/> is <see cref="CredentialSource.Certificate"/>, you will use this property to provide the certificate yourself. 
        /// When <see cref="SourceType"/> is <see cref="CredentialSource.Base64Encoded"/> or <see cref="CredentialSource.KeyVault"/>
        /// or <see cref="CredentialSource.Path"/> or <see cref="CredentialSource.StoreWithDistinguishedName"/> or <see cref="CredentialSource.StoreWithThumbprint"/>
        /// after the certificate is retrieved by a <see cref="ICredentialsLoader"/>, it will be stored in this property and also in the <see cref="CachedValue"/>.
        /// </summary>
        public X509Certificate2? Certificate { get; set; }

        /// <summary>
        /// When the credential is retrieved by a <see cref="ICredentialsLoader"/>, it will be stored in this property, where you can retrieve it. If the credential is a certificate,
        /// it will also be stored in the <see cref="Certificate"/> property.
        /// </summary>
        public virtual object? CachedValue { get; set; }

        /// <summary>
        /// Skip this credential description. This is useful when, you specify a list of
        /// credentials, some of which don't apply in a particular deployment.
        /// It will also be used by the <see cref="ICredentialsLoader"/> if it cannot find or load the credential.
        /// </summary>
        public bool Skip { get; set; }

        /// <summary>
        /// Describes the type of credentials, based on the <see cref="SourceType"/>.
        /// </summary>
        /// <remarks>
        /// Returns:
        /// <list type="bullet">
        /// <item>
        /// <term><see cref="CredentialType.Certificate"/></term>
        /// <description>when <see cref="SourceType"/> is <see cref="CredentialSource.Certificate"/>, you will use this property to provide the certificate yourself. 
        /// When <see cref="SourceType"/> is <see cref="CredentialSource.Base64Encoded"/> or <see cref="CredentialSource.KeyVault"/>
        /// or <see cref="CredentialSource.Path"/> or <see cref="CredentialSource.StoreWithDistinguishedName"/> or <see cref="CredentialSource.StoreWithThumbprint"/></description>
        /// </item>
        /// <item>
        /// <term><see cref="CredentialType.Secret"/></term>
        /// <description>when <see cref="SourceType"/> is <see cref="CredentialSource.ClientSecret"/>.</description>
        /// </item>
        /// <item>
        /// <term><see cref="CredentialType.SignedAssertion"/></term>
        /// <description>when <see cref="SourceType"/> is <see cref="CredentialSource.SignedAssertionFilePath"/> or <see cref="CredentialSource.SignedAssertionFromManagedIdentity"/>
        /// or <see cref="CredentialSource.SignedAssertionFromVault"/>.</description>
        /// </item>
        /// <item>
        /// <term><see cref="CredentialType.DecryptKeys"/></term>
        /// <description>when <see cref="SourceType"/> is <see cref="CredentialSource.AutoDecryptKeys"/>.</description>
        /// </item>
        /// </list>
        /// </remarks>
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

        /// <summary>
        /// (Microsoft Entra specific)
        /// Value that can be used to configure the token exchange resource url.
        /// </summary>
        /// /// <example>
        /// <format type="text/markdown">
        /// <![CDATA[
        /// The JSON fragment below describes a workload identity federation with a user assigned managed identity:
        /// :::code language="json" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="tokenExchangeUrl_json":::
        /// 
        /// The code below describes programmatically in C#, the same workload identity federation with a user assigned managed identity.
        /// :::code language="csharp" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="tokenExchangeUrl_csharp":::
        /// ]]></format>
        /// </example> 
        /// <remarks>If you want to use the default token exchange resource "api://AzureADTokenExchange", don't provide a token exchange url.</remarks>
        public string? TokenExchangeUrl { get; set; }
    }
}
