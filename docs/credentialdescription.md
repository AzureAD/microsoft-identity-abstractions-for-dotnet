# CredentialDescription Documentation

The `CredentialDescription` class is used to describe credentials for Microsoft Identity applications. These credentials can be used for two purposes:
1. Client Credentials - to prove the identity of the application
2. Token Decryption Credentials - to decrypt tokens

## When to Use Which Credential Type

### Certificate Credentials
Best for production environments due to enhanced security. Choose between these options based on your scenario:

1. **Key Vault (`SourceType = KeyVault`)**
   - **What**: Certificate stored in Azure Key Vault
   - **When to use**:
     - Production environments
     - When you need centralized certificate management
     - When you require automatic certificate rotation
     - When you need to share certificates across multiple services
   - **Advantages**:
     - Secure storage and access control
     - Automatic certificate renewal
     - Audit logging
     - Managed backup and recovery

2. **Certificate Store (`SourceType = StoreWithThumbprint` or `StoreWithDistinguishedName`)**
   - **What**: Certificate stored in the Windows Certificate Store
   - **When to use**:
     - Production environments
     - When the application runs on Windows
     - When you want to use Windows certificate management tools
   - **Choose between**:
     - `StoreWithThumbprint`: When you need to target a specific certificate version
     - `StoreWithDistinguishedName`: When you want automatic certificate rollover
   - **Advantages**:
     - Native Windows management
     - Integration with Windows security features
     - Support for hardware security modules (HSM)

3. **File Path (`SourceType = Path`)**
   - **What**: Certificate stored as a PFX/P12 file on disk
   - **When to use**:
     - Development/testing environments
     - Simple deployment scenarios
     - When filesystem-based configuration is preferred
   - **Not recommended for production** due to:
     - Password needs to be in configuration
     - Manual certificate management
     - Less secure storage

4. **Base64 Encoded (`SourceType = Base64Encoded`)**
   - **What**: Certificate encoded as a base64 string
   - **When to use**:
     - Development/testing environments
     - When certificate needs to be embedded in configuration
   - **Not recommended for production** due to:
     - Certificate exposed in configuration
     - Manual certificate management
     - Less secure storage

### Client Secret (`SourceType = ClientSecret`)
- **What**: A simple shared secret string
- **When to use**:
  - Development/testing environments
  - Simple applications with basic security requirements
- **Not recommended for production** due to:
  - Less secure than certificates
  - No support for automatic rotation
  - Secrets can be easily copied/shared

### Federated Identity Credentials with Managed Identity (`SourceType = SignedAssertionFromManagedIdentity`)
- **What**: Azure Managed Identity integration
- **When to use**:
  - Production environments running on Azure
  - When you want zero secret/certificate management
  - Modern cloud-native applications
- **Choose between**:
  - System-assigned: When the identity is tied to the resource lifecycle
  - User-assigned: When you need to share identity across resources
- **Advantages**:
  - No secret/certificate management required
  - Automatic credential rotation
  - Tight integration with Azure services
  - Enhanced security with no stored credentials

### Auto Decrypt Keys (`SourceType = AutoDecryptKeys`)
- **What**: Automatic key retrieval for token decryption
- **When to use**:
  - When implementing encrypted token scenarios
  - When the application needs to decrypt tokens automatically
- **Advantages**:
  - Automatic key management
  - Support for key rotation
  - Simplified token decryption workflow

## JSON Configuration

### 1. Certificate-Based Credentials

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

### 2. Client Secret
```json
{
 "ClientCredentials": [
 {
  "SourceType": "ClientSecret",
  "ClientSecret": "your-secret-here"
 }]
}
```

### 3. Managed Identity Credentials
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

### 1. Certificate-Based Credentials

#### From Key Vault
```csharp
var credentialDescription = new CredentialDescription
{
    SourceType = CredentialSource.KeyVault,
    KeyVaultUrl = "https://msidentitywebsamples.vault.azure.net",
    KeyVaultCertificateName = "MicrosoftIdentitySamplesCert"
};
```

#### From Certificate Store (Using Thumbprint)
```csharp
var credentialDescription = new CredentialDescription
{
    SourceType = CredentialSource.StoreWithThumbprint,
    CertificateStorePath = "LocalMachine/My",
    CertificateThumbprint = "962D129A...D18EFEB6961684"
};
```

#### From Certificate Store (Using Distinguished Name)
```csharp
var credentialDescription = new CredentialDescription
{
    SourceType = CredentialSource.StoreWithDistinguishedName,
    CertificateStorePath = "LocalMachine/My",
    CertificateDistinguishedName = "CN=WebAppCallingWebApiCert"
};
```

#### From File Path
```csharp
var credentialDescription = new CredentialDescription
{
    SourceType = CredentialSource.Path,
    CertificateDiskPath = "c:\\temp\\WebAppCallingWebApiCert.pfx",
    CertificatePassword = "password"
};
```

#### Base64 Encoded
```csharp
var credentialDescription = new CredentialDescription
{
    SourceType = CredentialSource.Base64Encoded,
    Base64EncodedValue = "MIIDHzCgegA.....r1n8Ta0="
};
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
     1. Azure Key Vault (most secure, recommended)
     2. Managed Identities (when running on Azure)
     3. Certificate Store (for Windows environments)
   - For development/testing only:
     - Client Secrets
     - File-based certificates
     - Base64 encoded certificates

3. **Certificate Store Paths**:
   - Format: `{StoreLocation}/{StoreName}`
   - Common values:
     - `CurrentUser/My`: User certificates
     - `LocalMachine/My`: Computer certificates

4. **Managed Identity**:
   - For system-assigned managed identity, use `SignedAssertionFromManagedIdentity` source type without specifying `ManagedIdentityClientId`
   - For user-assigned managed identity, specify the `ManagedIdentityClientId`

5. **Custom Provider Extension**:
   - Use `CredentialSource.CustomSignedAssertion` with:
     - `CustomSignedAssertionProviderName`
     - `CustomSignedAssertionProviderData` (Dictionary<string, object>)

## Decision Flow for Choosing Credential Type

1. Are you running on Azure?
   - Yes: Consider Managed Identity first
   - No: Continue to next question

2. Is this for production?
   - Yes: Choose between:
     1. Key Vault (preferred)
     2. Certificate Store (Windows environments)
   - No: You can use any option, including client secrets and file-based certificates

3. Do you need certificate rotation?
   - Yes: Use Key Vault or Certificate Store with Distinguished Name
   - No: Any certificate option will work

4. Do you need to share credentials across services?
   - Yes: Use Key Vault or User-assigned Managed Identity
   - No: Any option suitable for your environment will work

5. Are you implementing token decryption?
   - Yes: Use AutoDecryptKeys or Certificate-based options
   - No: Any client credential option will work
