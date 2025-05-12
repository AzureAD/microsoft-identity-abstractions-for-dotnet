# CredentialDescription Documentation

The `CredentialDescription` class is used to describe credentials for Microsoft Identity applications. These credentials can be used for two purposes:
1. Client Credentials - to prove the identity of the application
2. Token Decryption Credentials - to decrypt tokens

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

2. **Security Recommendations**:
   - Using client secrets is not recommended in production
   - Using certificates from file paths or base64 encoded values is not recommended in production
   - For production scenarios, prefer:
     - Key Vault certificates
     - Certificate store
     - Managed identities

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
