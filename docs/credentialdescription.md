# CredentialDescription Documentation

The `CredentialDescription` class is used to describe credentials for Microsoft Identity applications. These credentials support both traditional certificate-based and certificateless authentication approaches, and can be used for two purposes:
1. Client Credentials - to prove the identity of the application
2. Token Decryption Credentials - to decrypt tokens

## App authentication Approaches

### Client secrets

Uses an app password shared between the Identity Provider (Entra ID) and the code of the app. This is simple, but not usable in production. Certain organizations even prevent its usage.

### Traditional Certificate-Based Authentication
Uses X.509 certificates for strong cryptographic identity verification. This is the traditional approach that relies on certificate management.

### Certificateless Authentication
Provides authentication without the overhead of certificate management. This modern approach is particularly useful for:
- Cloud-native applications
- Scenarios where certificate management is challenging
- Environments with limited PKI infrastructure
- Rapid deployment requirements

This is the recommended approach

## When to Use Which Credential Type

| Credential Type | What Is It | When to Use | Advantages | Considerations |
|----------------|------------|-------------|------------|----------------|
| **Federation identity credential with Managed Identity (FIC+MSI)** <br> (`SignedAssertionFromManagedIdentity`) | Azure Managed Identity | • Production on Azure<br>• Zero cert management<br>• Cloud-native apps<br><br>_System-assigned_: Resource lifecycle<br>_User-assigned_: Share across resources | • No secret management<br>• Automatic rotation<br>• Azure integration<br>• Enhanced security | Best choice for Azure workloads |
| **Key Vault** <br> (`SourceType = KeyVault`) | Certificate stored in Azure Key Vault | • Production environments<br>• Need for centralized management<br>• Require automatic rotation<br>• Share across services | • Secure storage & access control<br>• Automatic renewal<br>• Audit logging<br>• Managed backup/recovery | Best choice for production workloads not hosted on an Azure compute supporting managed identity|
| **Certificate Store** <br> (`StoreWithThumbprint` or `StoreWithDistinguishedName`) | Certificate in Windows Certificate Store | • Production on Windows<br>• Using Windows cert management<br><br>_Thumbprint_: Target specific version<br>_DistinguishedName_: Auto rollover | • Native Windows management<br>• Windows security integration<br>• HSM support | Ideal for Windows production environments |
| **File Path** <br> (`SourceType = Path`) | PFX/P12 file on disk | • Development/testing<br>• Simple deployment<br>• File-based config preferred | • Simple setup<br>• Easy deployment<br>• Direct file access | **Not for production:**<br>• Password in config<br>• Manual management<br>• Less secure storage |
| **Base64 Encoded** <br> (`SourceType = Base64Encoded`) | Certificate as base64 string | • Development/testing<br>• Config-embedded certificates | • Simple configuration<br>• No file system dependency | **Not for production:**<br>• Exposed in config<br>• Manual management<br>• Less secure |
| **Client Secret** <br> (`SourceType = ClientSecret`) | Simple shared secret string | • Development/testing<br>• Basic security requirements | • Simple to use<br>• Easy to configure | **Not for production:**<br>• Less secure<br>• No auto-rotation<br>• Easy to expose |
| **Auto Decrypt Keys** <br> (`SourceType = AutoDecryptKeys`) | Automatic key retrieval | • Encrypted token scenarios<br>• Automatic token decryption | • Automatic key management<br>• Key rotation support<br>• Simplified workflow | Specific to token decryption scenarios |

## JSON Configuration


### 1. FIC+MSI
```json
{
 "ClientCredentials": [
 {
  "SourceType": "SignedAssertionFromManagedIdentity",
  "ManagedIdentityClientId": "12345",  // Optional for system-assigned managed identity
  "TokenExchangeUrl": "api://AzureADTokenExchangeChina",  // Optional
  "TokenExchangeAuthority": "https://login.microsoftonline.cloud2/33e01921-4d64-4f8c-a055-5bdaffd5e33d/v2.0"  // Optional
 }]
}
```

### 2. Certificate Credentials

This is the case where the client credentials are a certificate.

#### From Key Vault
```json
{
 "ClientCredentials": [
  {
   "SourceType": "KeyVault",
   "KeyVaultUrl": "https://msidentitywebsamples.vault.azure.net",
   "KeyVaultCertificateName": "MicrosoftIdentitySamplesCert"
  }
 ]
}
```

#### From Certificate Store (Using Thumbprint)
```json
{
 "ClientCredentials": [
 {
  "SourceType": "StoreWithThumbprint",
  "CertificateStorePath": "CurrentUser/My",
  "CertificateThumbprint": "962D129A...D18EFEB6961684"
 }]
}
```

#### From Certificate Store (Using Distinguished Name)
```json
{
 "ClientCredentials": [
 {
  "SourceType": "StoreWithDistinguishedName",
  "CertificateStorePath": "CurrentUser/My",
  "CertificateDistinguishedName": "CN=WebAppCallingWebApiCert"
 }]
}
```

#### From File Path
```json
{
 "ClientCredentials": [
 {
  "SourceType": "Path",
  "CertificateDiskPath": "c:\\temp\\WebAppCallingWebApiCert.pfx",
  "CertificatePassword": "password"
 }]
}
```

#### Base64 Encoded
```json
{
 "ClientCredentials": [
 {
  "SourceType": "Base64Encoded",
  "Base64EncodedValue": "MIIDHzCgegA.....r1n8Ta0="
 }]
}
```

### 3. Client Secret
```json
{
 "ClientCredentials": [
 {
  "SourceType": "ClientSecret",
  "ClientSecret": "your-secret-here"
 }]
}
```


### 4. Auto Decrypt Keys (for Token Decryption)
```json
{
 "TokenDecryptionCredentials": [
 {
  "SourceType": "AutoDecryptKeys",
  "DecryptKeysAuthenticationOptions": {
   "ProtocolScheme": "Bearer",
   "AcquireTokenOptions": {
    "Tenant": "mytenant.onmicrosoftonline.com"
   }
  }
 }]
}
```

## C# Code Usage

### 1. Certificate  Credentials

#### From Key Vault
```csharp
// Using property initialization
var credentialDescription = new CredentialDescription
{
    SourceType = CredentialSource.KeyVault,
    KeyVaultUrl = "https://msidentitywebsamples.vault.azure.net",
    KeyVaultCertificateName = "MicrosoftIdentitySamplesCert"
};

// Using FromKeyVault method
var credentialDescription = CredentialDescription.FromKeyVault(
    "https://msidentitywebsamples.vault.azure.net",
    "MicrosoftIdentitySamplesCert");
```

#### From Certificate Store (Using Thumbprint)
```csharp
// Using property initialization
var credentialDescription = new CredentialDescription
{
    SourceType = CredentialSource.StoreWithThumbprint,
    CertificateStorePath = "LocalMachine/My",
    CertificateThumbprint = "962D129A...D18EFEB6961684"
};

// Using FromCertificateStore method
var credentialDescription = CredentialDescription.FromCertificateStore(
    "LocalMachine/My",
    thumbprint: "962D129A...D18EFEB6961684");
```

#### From Certificate Store (Using Distinguished Name)
```csharp
// Using property initialization
var credentialDescription = new CredentialDescription
{
    SourceType = CredentialSource.StoreWithDistinguishedName,
    CertificateStorePath = "LocalMachine/My",
    CertificateDistinguishedName = "CN=WebAppCallingWebApiCert"
};

// Using FromCertificateStore method
var credentialDescription = CredentialDescription.FromCertificateStore(
    "LocalMachine/My",
    distinguishedName: "CN=WebAppCallingWebApiCert");
```

#### From File Path
```csharp
// Using property initialization
var credentialDescription = new CredentialDescription
{
    SourceType = CredentialSource.Path,
    CertificateDiskPath = "c:\\temp\\WebAppCallingWebApiCert.pfx",
    CertificatePassword = "password"
};

// Using FromCertificatePath method
var credentialDescription = CredentialDescription.FromCertificatePath(
    "c:\\temp\\WebAppCallingWebApiCert.pfx",
    "password");
```

#### Base64 Encoded
```csharp
// Using property initialization
var credentialDescription = new CredentialDescription
{
    SourceType = CredentialSource.Base64Encoded,
    Base64EncodedValue = "MIIDHzCgegA.....r1n8Ta0="
};

// Using FromBase64String method
var credentialDescription = CredentialDescription.FromBase64String(
    "MIIDHzCgegA.....r1n8Ta0=");
```

### 2. Client Secret
```csharp
var credentialDescription = new CredentialDescription
{
    SourceType = CredentialSource.ClientSecret,
    ClientSecret = "your-secret-here"
};
```

### 3. Managed Identity Credentials
```csharp
var credentialDescription = new CredentialDescription
{
    SourceType = CredentialSource.SignedAssertionFromManagedIdentity,
    ManagedIdentityClientId = "GUID",  // Optional for system-assigned managed identity
    TokenExchangeUrl = "api://AzureADTokenExchangeSomeCloud1",  // Optional
    TokenExchangeAuthority = "https://login.microsoftonline.cloud2/33e01921-4d64-4f8c-a055-5bdaffd5e33d/v2.0"  // Optional
};
```

### 4. Auto Decrypt Keys (for Token Decryption)
```csharp
var credentialDescription = new CredentialDescription
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
```

## Important Notes

1. **Credential Types**:
   - `Certificate`: Can be used for both client credentials and token decryption
   - `Secret`: Only for client credentials
   - `SignedAssertion`: Only for client credentials
   - `DecryptKeys`: Only for token decryption

2. **Security Best Practices**:
    - For production environments:
      A. Certificateless Authentication:
         1. Managed Identities (when running on Azure)
         2. Other certificateless authentication methods
         - Benefits:
           * No certificate management overhead
           * Automatic credential rotation
           * Reduced operational complexity
           * Lower risk of credential exposure

      B. Certificate-based Authentication:
         1. Azure Key Vault (most secure, recommended)
         2. Certificate Store (for Windows environments)
         - Benefits:
           * Strong cryptographic identity proof
           * Compliance with PKI requirements
           * Suitable for air-gapped environments
           * Hardware security module (HSM) support

    - For development/testing only:
      - Client Secrets
      - File-based certificates
      - Base64 encoded certificates

    Key considerations:
    - Choose certificateless authentication when possible for simplified operations
    - Use certificate-based authentication when:
      * Regulatory compliance requires it
      * You need cryptographic proof of identity
      * Your environment has established PKI infrastructure
      * You're working in air-gapped scenarios

3. **Certificate Store Paths**:
   - Format: `{StoreLocation}/{StoreName}`
   - Common values:
     - `CurrentUser/My`: User certificates
     - `LocalMachine/My`: Computer certificates

4. **Federation Identity Credential with Managed Identity**:
   - For system-assigned managed identity, use `SignedAssertionFromManagedIdentity` source type without specifying `ManagedIdentityClientId`
   - For user-assigned managed identity, specify the `ManagedIdentityClientId`

5. **Custom Provider Extension**:
   - Use `CredentialSource.CustomSignedAssertion` with:
     - `CustomSignedAssertionProviderName`
     - `CustomSignedAssertionProviderData` (Dictionary<string, object>)

## Decision Flow for Choosing Authentication Approach

1. Do you want to avoid certificate management complexity?
    - Yes: Consider certificateless authentication
      * Ideal for cloud-native applications
      * Simpler deployment and maintenance
      * No certificate lifecycle management
    - No: Continue with traditional certificate-based approach

2. Are you running on Azure?
    - Yes: Consider these options in order:
      1. Federation Identity Credential with  Managed Identity (certificateless)
      2. Key Vault (certificate-based)
    - No: Continue to next question

3. Is this for production?
    - Yes: Choose between:
      1. Certificateless authentication (if supported in your environment)
      2. Key Vault (preferred for certificate-based)
      3. Certificate Store (Windows environments)
    - No: You can use any option, including client secrets and file-based certificates, but keep your secrets safe.

4. Do you need credential rotation?
    - Yes: Consider these options:
      * Certificateless authentication (automatic rotation)
      * Key Vault with managed certificates
      * Certificate Store with Distinguished Name
    - No: Any authentication option will work

5. Do you need to share credentials across services?
    - Yes: Use:
      * User-assigned Managed Identity (certificateless)
      * Key Vault (certificate-based)
    - No: Any option suitable for your environment will work

6. Are you implementing token decryption?
    - Yes: Use:
      * AutoDecryptKeys (preferred). This requires  client credentials
      * decrypt certificate using the Certificate-based options
    - Yes: No

7. Do you have compliance requirements for cryptographic proof of identity?
    - Yes: Use traditional certificate-based authentication
    - No: Consider certificateless authentication for simplified operations
