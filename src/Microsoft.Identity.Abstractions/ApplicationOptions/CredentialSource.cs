// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Source for a credential.
    /// Credentials are used to prove the identity of the
    /// application (See <see cref="IdentityApplicationOptions.ClientCredentials"/>), or
    /// to decrypt tokens (See <see cref="IdentityApplicationOptions.TokenDecryptionCredentials"/>). Credentials can be
    /// secrets (client secrets), certificates, or signed assertions. They can be stored or provided in a variety of ways, 
    /// and this enumeration describes these ways. It's used in the <see cref="CredentialDescription.SourceType"/> property.
    /// </summary>
    public enum CredentialSource
    {
        /// <summary>
        /// Use this value if you provide a certificate yourself. When setting the <see cref="CredentialDescription.SourceType"/> property to this value,
        /// you will also provide the <see cref="CredentialDescription.Certificate"/>.
        /// </summary>
        Certificate = 0,

        /// <summary>
        /// Use this value when the certificate is stored in Azure Key Vault. When setting the <see cref="CredentialDescription.SourceType"/> property to this value,
        /// you'll also provide the <see cref="CredentialDescription.KeyVaultUrl"/> and <see cref="CredentialDescription.KeyVaultCertificateName"/>
        /// properties.
        /// </summary>
        /// <example>
        /// <format type="text/markdown">
        /// <![CDATA[
        /// The Json fragment below describes a certificate stored in Key Vault used as a client credential in a confidential client application:
        /// :::code language="json" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="keyvault_json":::
        /// 
        /// The code below describes programmatically in C#, the same certificate stored in Key Vault.
        /// :::code language="csharp" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="keyvault_csharp":::
        /// ]]></format>
        /// </example>
        KeyVault = 1,

        /// <summary>
        /// Use this value when you provide a Base64 encoded string. When setting the <see cref="CredentialDescription.SourceType"/> property to this value,
        /// you'll also provide the <see cref="CredentialDescription.Base64EncodedValue"/> property and optionally, the <see cref="CredentialDescription.CertificatePassword"/>.
        /// </summary>
        /// <example>
        /// <format type="text/markdown">
        /// <![CDATA[
        /// The Json fragment below describes a certificate by its base64 encoded value, to be used as a client credential in a confidential client application:
        /// :::code language="json" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="base64_json":::
        /// 
        /// The code below describes programmatically in C#, the same certificate.
        /// :::code language="csharp" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="base64_csharp":::
        /// ]]></format>
        /// </example>
        /// <remarks>Using the base64 encoded representation of a certificate is not recommended in production.</remarks>
        Base64Encoded = 2,

        /// <summary>
        /// Use this value when you provide a path to a file containing the certificate on disk. When setting the <see cref="CredentialDescription.SourceType"/> property to this value,
        /// you'll also provide the <see cref="CredentialDescription.CertificateDiskPath"/> property, and optionally, the <see cref="CredentialDescription.CertificatePassword"/>
        /// </summary>
        /// <example>
        /// <format type="text/markdown">
        /// <![CDATA[
        /// The Json fragment below describes a certificate retrieved by its path and a password to be used as a client credential in a confidential client application:
        /// :::code language="json" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="path_json":::
        /// 
        /// The code below describes programmatically in C#, a the same certificate.
        /// :::code language="csharp" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="path_csharp":::
        /// ]]></format>
        /// </example>
        /// <remarks>Using a certificate from a local path is not recommended in production.</remarks>
        Path = 3,

        /// <summary>
        /// Use this value when you provide a certificate from the certificate store, described by its thumbprint.
        /// When setting the <see cref="CredentialDescription.SourceType"/> property to this value, you'll also provide the <see cref="CredentialDescription.CertificateThumbprint"/>
        /// and <see cref="CredentialDescription.CertificateStorePath"/> properties.
        /// </summary>
        /// <example>
        /// <format type="text/markdown">
        /// <![CDATA[
        /// The Json fragment below describes a user certificate stored in the personal certificates folder (<b>CurrentUser/My</b>) and specified by its thumbprint, used as a client credential in a confidential client application:
        /// :::code language="json" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="thumbprint_json":::
        /// 
        /// The code below describes programmatically in C#, a computer certificate in the personal certificates folder (<b>LocalMachine/My<b>) accessed by its thumbprint.
        /// :::code language="csharp" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="thumbprint_csharp":::
        /// ]]></format>
        /// </example>
        StoreWithThumbprint = 4,

        /// <summary>
        /// Use this value when you provide a certificate from the certificate store, described by its distinguished name.
        /// When setting the <see cref="CredentialDescription.SourceType"/> property to this value, you'll also provide the <see cref="CredentialDescription.CertificateDistinguishedName"/>
        /// and <see cref="CredentialDescription.CertificateStorePath"/> properties.
        /// </summary>
        ///         /// <example>
        /// <format type="text/markdown">
        /// <![CDATA[
        /// The Json fragment below describes a user certificate stored in the personal certificates folder (<b>CurrentUser/My</b>) and specified by its distinguised name, used as a client credential in a confidential client application:
        /// :::code language="json" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="distinguishedname_json":::
        /// 
        /// The code below describes programmatically in C#, a computer certificate in the personal certificates folder (<b>LocalMachine/My<b>).
        /// :::code language="csharp" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="distinguishedname_csharp":::
        /// ]]></format>
        /// </example>
        StoreWithDistinguishedName = 5,

        /// <summary>
        /// Use this value when you provide a client secret.
        /// When setting the <see cref="CredentialDescription.SourceType"/> property to this value, you'll also provide the <see cref="CredentialDescription.ClientSecret"/>.
        /// </summary>
        /// <example>
        /// <format type="text/markdown">
        /// <![CDATA[
        /// The Json fragment below describes a client secret used as a client credential in a confidential client application:
        /// :::code language="json" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="secret_json":::
        /// 
        /// The code below describes programmatically in C#, the same client secret.
        /// :::code language="csharp" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="secret_csharp":::
        /// ]]></format>
        /// </example>
        /// <remarks>Using a client secret recommended in production.</remarks>
        ClientSecret = 6,

        /// <summary>
        /// Use this value for a Certificateless client credentials using workload identity federation with managed identity.
        /// When setting the <see cref="CredentialDescription.SourceType"/> property to this value, you can also provide a user assigned managed identity using the <see cref="CredentialDescription.ManagedIdentityClientId"/>.
        /// If you don't the client credential will be based on the system assigned managed identity.
        /// </summary>
        /// <example>
        /// <format type="text/markdown">
        /// <![CDATA[
        /// The Json fragment below describes a workload identity federation with a user assigned managed identity:
        /// :::code language="json" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="msi_json":::
        /// 
        /// The code below describes programmatically in C#, the same workload identity federation with a user assigned managed identity.
        /// :::code language="csharp" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="msi_csharp":::
        /// ]]></format>
        /// </example> 
        SignedAssertionFromManagedIdentity = 7,

        /// <summary>
        /// Use this value for a Certificateless client credentials using workload identity federation with Azure Kubernetes Services (AKS).
        /// When setting the <see cref="CredentialDescription.SourceType"/> property to this value, you can also optionally provide a path containing the signed assertion.
        /// If you don't the credential will be searched in files contained in the following environment variables: <b>AZURE_FEDERATED_TOKEN_FILE</b> and <b>AZURE_ACCESS_TOKEN_FILE</b>.
        /// </summary>
        /// <example>
        /// <format type="text/markdown">
        /// <![CDATA[
        /// The Json fragment below describes a signed assertion acquired with workload identity federation with Azure Kubernetes Services (AKS):
        /// :::code language="json" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="aks_json":::
        /// 
        /// The code below describes programmatically in C#, the same workload identity federation with with Azure Kubernetes Services (AKS) signed assertion.
        /// :::code language="csharp" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="aks_csharp":::
        /// ]]></format>
        /// </example>
        SignedAssertionFilePath = 8,

        /// <summary>
        /// Use this value for a certificateless client credentials generated from another vault.
        /// When setting the <see cref="CredentialDescription.SourceType"/> property to this value, you can also optionally provide the name of a certificate used to compute
        /// the signed assertion using the <see cref="CredentialDescription.KeyVaultCertificateName"/> property.
        /// </summary>
        SignedAssertionFromVault = 9,

        /// <summary>
        /// Use this value for automatic decrypt keys used by a web API to decrypt an encrypted token. When setting the <see cref="CredentialDescription.SourceType"/> property to this value,
        /// also use the <see cref="CredentialDescription.DecryptKeysAuthenticationOptions"/> to provide the tenant used by the web API to get a token to get the decrypt keys. This value
        /// only applies to <see cref="IdentityApplicationOptions.TokenDecryptionCredentials"/>, but the client credentials are used to get the token 
        /// to acquire the decrypt keys.
        /// </summary>
        /// <example>
        /// <format type="text/markdown">
        /// <![CDATA[
        /// The Json fragment below describes a decrypt credential to get the decrypt keys automatically:
        /// :::code language="json" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="autodecryp_json":::
        /// 
        /// The code below describes the same, programmatically in C#.
        /// :::code language="csharp" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/CredentialDescriptionTest.cs" id="autodecryp_csharp":::
        /// ]]></format>
        /// </example>
        AutoDecryptKeys = 10,

        /// <summary>
        /// Use this value in order to utilize a credential provider that is not part of the Microsoft.Identity.Abstractions library.
        /// This is an extension point, which goes along with <see cref = "CredentialDescription.CustomSignedAssertionProviderName" />
        /// </summary>
        CustomSignedAssertion = 11
    }
}
