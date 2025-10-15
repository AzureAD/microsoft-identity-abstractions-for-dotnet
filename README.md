[![CI](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/actions/workflows/dotnetcore.yml/badge.svg)](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/actions/workflows/dotnetcore.yml) ![Diagrams Synced](https://img.shields.io/badge/Diagrams%20Synced-2025--10--15-blue)

# Microsoft.Identity.Abstractions

Microsoft.Identity.Abstractions contain interfaces and POCO classes used in the Microsoft .NET authentication
libraries (Microsoft.IdentityModel, MSAL.NET and Microsoft.Identity.Web). It exposes concepts in three domains:
- Application and credentials
- Acquire tokens, get authorization headers and call downstream APIs
- Extensibility to bring your own credential providers

## NuGet Package

[![NuGet](https://img.shields.io/nuget/v/Microsoft.Identity.Abstractions.svg?style=flat-square&label=nuget&colorB=00b200)](https://www.nuget.org/packages/Microsoft.Identity.Abstractions/)

## Version Lifecycle and Support Matrix

See [Long Term Support policy](./supportPolicy.md) for details.

The following table lists Microsoft.Identity.Abstractions versions currently supported and receiving security fixes.

| Major Version | Last Release | Patch release date  | Support phase|End of support |
| --------------|--------------|--------|------------|--------|
| 9.x           | [![NuGet](https://img.shields.io/nuget/v/Microsoft.Identity.Abstractions.svg?style=flat-square&label=nuget&colorB=00b200)](https://www.nuget.org/packages/Microsoft.Identity.Abstractions/)    |Monthly| Active | Not planned.<br/>✅Supported versions: from 9.0.0 to [![NuGet](https://img.shields.io/nuget/v/Microsoft.Identity.Abstractions.svg?style=flat-square&label=nuget&colorB=00b200)](https://www.nuget.org/packages/Microsoft.Identity.Abstractions/) <br/>⚠️Unsupported versions `< 9.0.0`.|


## Concepts

### Overview of the data classes
the following diagram provides an overview of the data classes exposed by Microsoft.Identity.Abstractions

```mermaid
classDiagram
   namespace ApplicationOptions {
      class CredentialDescription {
            &gt;&gt;ro&gt;&gt; +string Id
            &gt;&gt;rw&gt;&gt; +CredentialSource SourceType
            &gt;&gt;rw&gt;&gt; +string KeyVaultUrl
            &gt;&gt;rw&gt;&gt; +string CertificateStorePath
            &gt;&gt;rw&gt;&gt; +string CertificateDistinguishedName
            &gt;&gt;rw&gt;&gt; +string KeyVaultCertificateName
            &gt;&gt;rw&gt;&gt; +string CertificateThumbprint
            &gt;&gt;rw&gt;&gt; +string CertificateDiskPath
            &gt;&gt;rw&gt;&gt; +string CertificatePassword
            &gt;&gt;rw&gt;&gt; +string Base64EncodedValue
            &gt;&gt;rw&gt;&gt; +string ClientSecret
            &gt;&gt;rw&gt;&gt; +string ManagedIdentityClientId
            &gt;&gt;rw&gt;&gt; +string SignedAssertionFileDiskPath
            &gt;&gt;rw&gt;&gt; +AuthorizationHeaderProviderOptions DecryptKeysAuthenticationOptions
            &gt;&gt;rw&gt;&gt; +string TokenExchangeAuthority
            &gt;&gt;rw&gt;&gt; +X509Certificate2 Certificate
            &gt;&gt;rw&gt;&gt; +object CachedValue
            &gt;&gt;rw&gt;&gt; +bool Skip
            &gt;&gt;ro&gt;&gt; +CredentialType CredentialType
            &gt;&gt;rw&gt;&gt; +string TokenExchangeUrl
            &gt;&gt;rw&gt;&gt; +string CustomSignedAssertionProviderName
            &gt;&gt;rw&gt;&gt; +Dictionary&lt;string, Object&gt; CustomSignedAssertionProviderData
            &gt;&gt;rw&gt;&gt; +string Algorithm
        }
    class CredentialSource { <<enum>>
    Certificate = 0
    KeyVault = 1
    Base64Encoded = 2
    Path = 3
    StoreWithThumbprint = 4
    StoreWithDistinguishedName = 5
    ClientSecret = 6
    SignedAssertionFromManagedIdentity = 7
    SignedAssertionFilePath = 8
    SignedAssertionFromVault = 9
    AutoDecryptKeys = 10
    CustomSignedAssertion = 11
    }
    class CredentialType { <<enum>>
    Certificate = 0
    Secret = 1
    SignedAssertion = 2
    DecryptKeys = 3
    }
    class IdentityApplicationOptions {
            &gt;&gt;rw&gt;&gt; +string Authority
            &gt;&gt;rw&gt;&gt; +string ClientId
            &gt;&gt;rw&gt;&gt; +bool EnablePiiLogging
            &gt;&gt;rw&gt;&gt; +IDictionary&lt;string, string&gt; ExtraQueryParameters
            &gt;&gt;rw&gt;&gt; +IEnumerable&lt;CredentialDescription&gt; ClientCredentials
            &gt;&gt;rw&gt;&gt; +string Audience
            &gt;&gt;rw&gt;&gt; +IEnumerable&lt;string&gt; Audiences
            &gt;&gt;rw&gt;&gt; +IEnumerable&lt;CredentialDescription&gt; TokenDecryptionCredentials
            &gt;&gt;rw&gt;&gt; +bool AllowWebApiToBeAuthorizedByACL
    }
    class MicrosoftEntraApplicationOptions {
            &gt;&gt;rw&gt;&gt; +string Name
            &gt;&gt;rw&gt;&gt; +string Instance
            &gt;&gt;rw&gt;&gt; +string TenantId
            &gt;&gt;rw&gt;&gt; +string Authority
            &gt;&gt;rw&gt;&gt; +string AppHomeTenantId
            &gt;&gt;rw&gt;&gt; +string AzureRegion
            &gt;&gt;rw&gt;&gt; +IEnumerable&lt;string&gt; ClientCapabilities
            &gt;&gt;rw&gt;&gt; +bool SendX5C
    }
    class MicrosoftIdentityApplicationOptions {
            &gt;&gt;rw&gt;&gt; +bool WithSpaAuthCode
            &gt;&gt;rw&gt;&gt; +string Domain
            &gt;&gt;rw&gt;&gt; +string EditProfilePolicyId
            &gt;&gt;rw&gt;&gt; +string SignUpSignInPolicyId
            &gt;&gt;rw&gt;&gt; +string ResetPasswordPolicyId
            &gt;&gt;ro&gt;&gt; +string DefaultUserFlow
            &gt;&gt;rw&gt;&gt; +string ResetPasswordPath
            &gt;&gt;rw&gt;&gt; +string ErrorPath
    }
 }

namespace TokenAcquisition {
    class AcquireTokenOptions {
           +AcquireTokenOptions Clone()
            &gt;&gt;rw&gt;&gt; +string AuthenticationOptionsName
            &gt;&gt;rw&gt;&gt; +Nullable&lt;Guid&gt; CorrelationId
            &gt;&gt;rw&gt;&gt; +IDictionary&lt;string, string&gt; ExtraQueryParameters
            &gt;&gt;rw&gt;&gt; +IDictionary&lt;string, Object&gt; ExtraParameters
            &gt;&gt;rw&gt;&gt; +IDictionary&lt;string, string&gt; ExtraHeaderParameters
            &gt;&gt;rw&gt;&gt; +string Claims
            &gt;&gt;rw&gt;&gt; +string FmiPath
            &gt;&gt;rw&gt;&gt; +bool ForceRefresh
            &gt;&gt;rw&gt;&gt; +string PopPublicKey
            &gt;&gt;rw&gt;&gt; +string PopClaim
            &gt;&gt;rw&gt;&gt; +ManagedIdentityOptions ManagedIdentity
            &gt;&gt;rw&gt;&gt; +string LongRunningWebApiSessionKey
            &gt;&gt;ro&gt;&gt; +string LongRunningWebApiSessionKeyAuto
            &gt;&gt;rw&gt;&gt; +string Tenant
            &gt;&gt;rw&gt;&gt; +string UserFlow
    }
    class AcquireTokenResult {
            &gt;&gt;rw&gt;&gt; +string AccessToken
            &gt;&gt;rw&gt;&gt; +DateTimeOffset ExpiresOn
            &gt;&gt;rw&gt;&gt; +string TenantId
            &gt;&gt;rw&gt;&gt; +string IdToken
            &gt;&gt;rw&gt;&gt; +IEnumerable&lt;string&gt; Scopes
            &gt;&gt;rw&gt;&gt; +Guid CorrelationId
            &gt;&gt;rw&gt;&gt; +string TokenType
            &gt;&gt;rw&gt;&gt; +IReadOnlyDictionary&lt;string, string&gt; AdditionalResponseParameters
            &gt;&gt;rw&gt;&gt; +X509Certificate2 BindingCertificate
    }
    class ITokenAcquirer { <<interface>>
    +Task&lt;AcquireTokenResult&gt; GetTokenForUserAsync(IEnumerable&lt;string&gt; scopes, AcquireTokenOptions tokenAcquisitionOptions, ClaimsPrincipal user, CancellationToken cancellationToken)
    +Task&lt;AcquireTokenResult&gt; GetTokenForAppAsync(string scope, AcquireTokenOptions tokenAcquisitionOptions, CancellationToken cancellationToken)
    }
    class ITokenAcquirerFactory { <<interface>>
    +ITokenAcquirer GetTokenAcquirer(IdentityApplicationOptions identityApplicationOptions)
    +ITokenAcquirer GetTokenAcquirer(string optionName)
    }
    class ManagedIdentityOptions {
    +ManagedIdentityOptions Clone()
    &gt;&gt;rw&gt;&gt; +string UserAssignedClientId
    }
 }

        namespace DownstreamApis {
    class AuthorizationHeaderProviderOptions {
    +AuthorizationHeaderProviderOptions Clone()
    #AuthorizationHeaderProviderOptions CloneInternal()
    +string GetApiUrl()
            &gt;&gt;rw&gt;&gt; +string BaseUrl
            &gt;&gt;rw&gt;&gt; +string RelativePath
            &gt;&gt;rw&gt;&gt; +string HttpMethod
            &gt;&gt;rw&gt;&gt; +Action&lt;HttpRequestMessage&gt; CustomizeHttpRequestMessage
            &gt;&gt;rw&gt;&gt; +AcquireTokenOptions AcquireTokenOptions
            &gt;&gt;rw&gt;&gt; +string ProtocolScheme
            &gt;&gt;rw&gt;&gt; +bool RequestAppToken
    }
    class DownstreamApiOptions {
    +DownstreamApiOptions Clone()
    #AuthorizationHeaderProviderOptions CloneInternal()
         &gt;&gt;rw&gt;&gt; +IEnumerable<string> Scopes
            &gt;&gt;rw&gt;&gt; +Func&lt;object?,HttpContent?&gt; Serializer
            &gt;&gt;rw&gt;&gt; +Func&lt;HttpContent?,object?&gt; Deserializer
            &gt;&gt;rw&gt;&gt; +string AcceptHeader
            &gt;&gt;rw&gt;&gt; +string ContentType
            &gt;&gt;rw&gt;&gt; +IDictionary&lt;string, string&gt; ExtraQueryParameters
            &gt;&gt;rw&gt;&gt; +IDictionary&lt;string, string&gt; ExtraHeaderParameters
    }
    class DownstreamApiOptionsReadOnlyHttpMethod {
    +DownstreamApiOptionsReadOnlyHttpMethod Clone()
    #AuthorizationHeaderProviderOptions CloneInternal()
    &gt;&gt;ro&gt;&gt; +string HttpMethod
    }

        class IAuthorizationHeaderProvider { <<interface>> }
        class IAuthorizationHeaderProvider~TResult~ { <<interface>> }
        class IDownstreamApi { <<interface>>
               +CallApiAsync(...)
               +CallApiForUserAsync(...)
               +CallApiForAppAsync(...)
               +Generic overloads() ... }
 }


    IdentityApplicationOptions <|-- MicrosoftEntraApplicationOptions : Inherits
    MicrosoftEntraApplicationOptions <|-- MicrosoftIdentityApplicationOptions : Inherits
    AuthorizationHeaderProviderOptions <|-- DownstreamApiOptions : Inherits
    DownstreamApiOptions <|-- DownstreamApiOptionsReadOnlyHttpMethod : Inherits

    CredentialDescription *-- "SourceType" CredentialSource : Has
    CredentialDescription --> "DecryptKeysAuthenticationOptions" AuthorizationHeaderProviderOptions : Has
    CredentialDescription *-- "CredentialType" CredentialType : Has

    IdentityApplicationOptions --> "ClientCredentials" CredentialDescription : Has many
    IdentityApplicationOptions --> "TokenDecryptionCredentials" CredentialDescription : Has many
    AuthorizationHeaderProviderOptions --> "AcquireTokenOptions" AcquireTokenOptions : Has
    AcquireTokenOptions --> "ManagedIdentity" ManagedIdentityOptions : Has

   ITokenAcquirerFactory --> ITokenAcquirer : produces
   ITokenAcquirer -->  AcquireTokenOptions : parametrized by
   AcquireTokenOptions --> "ManagedIdentity" ManagedIdentityOptions : Has
   ITokenAcquirer -->  AcquireTokenResult : returns
```

Note:
- for `AuthorizationHeaderProviderOptions` "Defaults: ProtocolScheme=Bearer, HttpMethod=Get"
-  for `DownstreamApiOptions` "Defaults: AcceptHeader=application/json, ContentType=application/json
-  for `IdentityApplicationOptions` "Effective audiences = Audience ∪ Audiences"

### Application options and credentials

   The application options are typically the options that you find in configuration files like the appsettings.json
   file. They describe the authentication aspects of your application. The library offers two layer. A standard
   layer, and a Microsoft Identity platform specialization.

  ```mermaid
  classDiagram
   class CredentialDescription {
    &lt;&lt;ro&gt;&gt; +string Id
    &gt;&gt;rw&gt;&gt; +CredentialSource SourceType
    &gt;&gt;rw&gt;&gt; +string KeyVaultUrl
    &gt;&gt;rw&gt;&gt; +string CertificateStorePath
    &gt;&gt;rw&gt;&gt; +string CertificateDistinguishedName
    &gt;&gt;rw&gt;&gt; +string KeyVaultCertificateName
    &gt;&gt;rw&gt;&gt; +string CertificateThumbprint
    &gt;&gt;rw&gt;&gt; +string CertificateDiskPath
    &gt;&gt;rw&gt;&gt; +string CertificatePassword
    &gt;&gt;rw&gt;&gt; +string Base64EncodedValue
    &gt;&gt;rw&gt;&gt; +string ClientSecret
    &gt;&gt;rw&gt;&gt; +string ManagedIdentityClientId
    &gt;&gt;rw&gt;&gt; +string SignedAssertionFileDiskPath
    &gt;&gt;rw&gt;&gt; +AuthorizationHeaderProviderOptions DecryptKeysAuthenticationOptions
    &gt;&gt;rw&gt;&gt; +string TokenExchangeAuthority
    &gt;&gt;rw&gt;&gt; +X509Certificate2 Certificate
    &gt;&gt;rw&gt;&gt; +Object CachedValue
    &gt;&gt;rw&gt;&gt; +bool Skip
    &lt;&lt;ro&gt;&gt; +CredentialType CredentialType
    &gt;&gt;rw&gt;&gt; +string TokenExchangeUrl
    &gt;&gt;rw&gt;&gt; +string CustomSignedAssertionProviderName
    &gt;&gt;rw&gt;&gt; +Dictionary&lt;string, Object&gt; CustomSignedAssertionProviderData
    &gt;&gt;rw&gt;&gt; +string Algorithm
    }
    class CredentialSource { <<enum>>
    Certificate = 0
    KeyVault = 1
    Base64Encoded = 2
    Path = 3
    StoreWithThumbprint = 4
    StoreWithDistinguishedName = 5
    ClientSecret = 6
    SignedAssertionFromManagedIdentity = 7
    SignedAssertionFilePath = 8
    SignedAssertionFromVault = 9
    AutoDecryptKeys = 10
    CustomSignedAssertion = 11
    }
    class CredentialType { <<enum>>
    Certificate = 0
    Secret = 1
    SignedAssertion = 2
    DecryptKeys = 3
    }
    class IdentityApplicationOptions {
    &gt;&gt;rw&gt;&gt; +string Authority
    &gt;&gt;rw&gt;&gt; +string ClientId
    &gt;&gt;rw&gt;&gt; +bool EnablePiiLogging
    &gt;&gt;rw&gt;&gt; +IDictionary&lt;string, string&gt; ExtraQueryParameters
    &gt;&gt;rw&gt;&gt; +IEnumerable&lt;CredentialDescription&gt; ClientCredentials
    &gt;&gt;rw&gt;&gt; +string Audience
    &gt;&gt;rw&gt;&gt; +IEnumerable&lt;string&gt; Audiences
    &gt;&gt;rw&gt;&gt; +IEnumerable&lt;CredentialDescription&gt; TokenDecryptionCredentials
    &gt;&gt;rw&gt;&gt; +bool AllowWebApiToBeAuthorizedByACL
    }
    class MicrosoftEntraApplicationOptions {
    &gt;&gt;rw&gt;&gt; +string Name
    &gt;&gt;rw&gt;&gt; +string Instance
    &gt;&gt;rw&gt;&gt; +string TenantId
    &gt;&gt;rw&gt;&gt; +string Authority
    &gt;&gt;rw&gt;&gt; +string AppHomeTenantId
    &gt;&gt;rw&gt;&gt; +string AzureRegion
    &gt;&gt;rw&gt;&gt; +IEnumerable&lt;string&gt; ClientCapabilities
    &gt;&gt;rw&gt;&gt; +bool SendX5C
    }
    class MicrosoftIdentityApplicationOptions {
    &gt;&gt;rw&gt;&gt; +bool WithSpaAuthCode
    &gt;&gt;rw&gt;&gt; +string Domain
    &gt;&gt;rw&gt;&gt; +string EditProfilePolicyId
    &gt;&gt;rw&gt;&gt; +string SignUpSignInPolicyId
    &gt;&gt;rw&gt;&gt; +string ResetPasswordPolicyId
    &lt;&lt;ro&gt;&gt; +string DefaultUserFlow
    &gt;&gt;rw&gt;&gt; +string ResetPasswordPath
    &gt;&gt;rw&gt;&gt; +string ErrorPath
    }

    IdentityApplicationOptions <|-- MicrosoftEntraApplicationOptions : Inherits
    MicrosoftEntraApplicationOptions <|-- MicrosoftIdentityApplicationOptions : Inherits

    CredentialDescription *-- "SourceType" CredentialSource : Has
    CredentialDescription --> "DecryptKeysAuthenticationOptions" AuthorizationHeaderProviderOptions : Has
    note for AuthorizationHeaderProviderOptions "see below"
    CredentialDescription *-- "CredentialType" CredentialType : Has
    IdentityApplicationOptions --> "ClientCredentials" CredentialDescription : Has many
    IdentityApplicationOptions --> "TokenDecryptionCredentials" CredentialDescription : Has many

   ```

For details about Credentials, see [CredentialDecription](./docs/credentialdescription.md)

### Credential loaders
   An important part of the application options are the credentials. In addition to the credential descriptions, the
   library offers extensibility mechanisms so that implementers can add their own credential source loaders.

   ```mermaid
   classDiagram
   class CredentialSourceLoaderParameters {
    &gt;&gt;rw&gt;&gt; +string ClientId
    &gt;&gt;rw&gt;&gt; +string Authority
    }
    class ICredentialsLoader { <<interface>>
    +Task LoadCredentialsIfNeededAsync(CredentialDescription credentialDescription, CredentialSourceLoaderParameters parameters)
    +Task&lt;CredentialDescription&gt; LoadFirstValidCredentialsAsync(IEnumerable&lt;CredentialDescription&gt; credentialDescriptions, CredentialSourceLoaderParameters parameters)
    +Void ResetCredentials(IEnumerable&lt;CredentialDescription&gt; credentialDescriptions)
    &lt;&lt;ro&gt;&gt; +IDictionary&lt;CredentialSource, ICredentialSourceLoader&gt; CredentialSourceLoaders
    }
    class ICredentialSourceLoader { <<interface>>
    +Task LoadIfNeededAsync(CredentialDescription credentialDescription, CredentialSourceLoaderParameters parameters)
    &lt;&lt;ro&gt;&gt; +CredentialSource CredentialSource
    }
    class ICustomSignedAssertionProvider { <<interface>>
    &lt;&lt;ro&gt;&gt; +string Name
    }

    ICredentialSourceLoader <|-- ICustomSignedAssertionProvider : Inherits
    ICredentialSourceLoader *-- "CredentialSource" CredentialSource : Has
    ICredentialsLoader --> ICredentialSourceLoader : Loads
    ICredentialSourceLoader --> CredentialSourceLoaderParameters : Uses
    note for CredentialSource "see above"
   ```

   There can be several application options with different names (for instance in ASP.NET Core these would be different
   authentication schemes)

### Token acquisition

Once configured, an application can acquire tokens from the Identity provider. This is a low level API, in the sense that
you would probably prefer to call downstream web APIs without having to be preoccupied about the authentication aspects. If you
really want to use the lower level API, you should:
- get hold of a ITokenAcquirerFactory. Implementations can provide a TokenAcquirerFactory for instance, with a singleton.
- get a ITokenAcquirer (by its name, for instance). This corresponds to the application options
- From the token acquirer get a token for on behalf of the user, or the app. If you don't specify any AcquireTokenOptions,
  the implementation should do its best effort. The AcquireTokenOptions enable you to override the defaults.

```mermaid
classDiagram
   class AcquireTokenOptions {
    +AcquireTokenOptions Clone()
    &gt;&gt;rw&gt;&gt; +string AuthenticationOptionsName
    &gt;&gt;rw&gt;&gt; +Nullable&lt;Guid&gt; CorrelationId
    &gt;&gt;rw&gt;&gt; +IDictionary&lt;string, string&gt; ExtraQueryParameters
    &gt;&gt;rw&gt;&gt; +IDictionary&lt;string, Object&gt; ExtraParameters
    &gt;&gt;rw&gt;&gt; +IDictionary&lt;string, string&gt; ExtraHeaderParameters
    &gt;&gt;rw&gt;&gt; +string Claims
    &gt;&gt;rw&gt;&gt; +string FmiPath
    &gt;&gt;rw&gt;&gt; +bool ForceRefresh
    &gt;&gt;rw&gt;&gt; +string PopPublicKey
    &gt;&gt;rw&gt;&gt; +string PopClaim
    &gt;&gt;rw&gt;&gt; +ManagedIdentityOptions ManagedIdentity
    &gt;&gt;rw&gt;&gt; +string LongRunningWebApiSessionKey
    &lt;&lt;ro&gt;&gt; +string LongRunningWebApiSessionKeyAuto
    &gt;&gt;rw&gt;&gt; +string Tenant
    &gt;&gt;rw&gt;&gt; +string UserFlow
    }
    class AcquireTokenResult {
    &gt;&gt;rw&gt;&gt; +string AccessToken
    &gt;&gt;rw&gt;&gt; +DateTimeOffset ExpiresOn
    &gt;&gt;rw&gt;&gt; +string TenantId
    &gt;&gt;rw&gt;&gt; +string IdToken
    &gt;&gt;rw&gt;&gt; +IEnumerable&lt;string&gt; Scopes
    &gt;&gt;rw&gt;&gt; +Guid CorrelationId
    &gt;&gt;rw&gt;&gt; +string TokenType
    &gt;&gt;rw&gt;&gt; +IReadOnlyDictionary&lt;string, string&gt; AdditionalResponseParameters
    &gt;&gt;rw&gt;&gt; +X509Certificate2 BindingCertificate
    }
    class ITokenAcquirer { <<interface>>
    +Task&lt;AcquireTokenResult&gt; GetTokenForUserAsync(IEnumerable&lt;string&gt; scopes, AcquireTokenOptions tokenAcquisitionOptions, ClaimsPrincipal user, CancellationToken cancellationToken)
    +Task&lt;AcquireTokenResult&gt; GetTokenForAppAsync(string scope, AcquireTokenOptions tokenAcquisitionOptions, CancellationToken cancellationToken)
    }
    class ITokenAcquirerFactory { <<interface>>
    +ITokenAcquirer GetTokenAcquirer(IdentityApplicationOptions identityApplicationOptions)
    +ITokenAcquirer GetTokenAcquirer(string optionName)
    }
    class ManagedIdentityOptions {
    +ManagedIdentityOptions Clone()
    &gt;&gt;rw&gt;&gt; +string UserAssignedClientId
    }

   ITokenAcquirerFactory ..> ITokenAcquirer : produces
   ITokenAcquirer -->  AcquireTokenOptions : parametrized by
   AcquireTokenOptions --> "ManagedIdentity" ManagedIdentityOptions : Has
   ITokenAcquirer ..>  AcquireTokenResult : returns
```

## Call downstream APIs

It's also possible (and recommended) to use higher level APIs:
- IDownstreamApi enables you to call a downstream web API and let the implementation handle the serialization of the
  input parameter (if any), handling the getting the authorization header and attaching it to the HttpClient, call
  the downstream web API, handle errors, deserialize the answer and return it as a strongly typed object. You can
  use customize all these steps, for instance by providing your own serializer / deserializer.
- IAuthorizationHeaderProvider is the component that provides the authorization header, delegating to the ITokenAcquirer.
  Whereas ITokenAcquirer only knows about tokens, IAuthorizationHeaderProvider knows about protocols (for instance bearer,
  Pop, etc ...)

 ```mermaid
 classDiagram
    class AuthorizationHeaderProviderOptions {
    +AuthorizationHeaderProviderOptions Clone()
    #AuthorizationHeaderProviderOptions CloneInternal()
    +string GetApiUrl()
    &gt;&gt;rw&gt;&gt; +string BaseUrl
    &gt;&gt;rw&gt;&gt; +string RelativePath
    &gt;&gt;rw&gt;&gt; +string HttpMethod
    &gt;&gt;rw&gt;&gt; +Action&lt;HttpRequestMessage&gt; CustomizeHttpRequestMessage
    &gt;&gt;rw&gt;&gt; +AcquireTokenOptions AcquireTokenOptions
    &gt;&gt;rw&gt;&gt; +string ProtocolScheme
    &gt;&gt;rw&gt;&gt; +bool RequestAppToken
    }
    class DownstreamApiOptions {
    +DownstreamApiOptions Clone()
    #AuthorizationHeaderProviderOptions CloneInternal()
    &gt;&gt;rw&gt;&gt; +IEnumerable&lt;string&gt; Scopes
    &gt;&gt;rw&gt;&gt; +Func&lt;Object, HttpContent&gt; Serializer
    &gt;&gt;rw&gt;&gt; +Func&lt;HttpContent, Object&gt; Deserializer
    &gt;&gt;rw&gt;&gt; +string AcceptHeader
    &gt;&gt;rw&gt;&gt; +string ContentType
    &gt;&gt;rw&gt;&gt; +IDictionary&lt;string, string&gt; ExtraQueryParameters
    &gt;&gt;rw&gt;&gt; +IDictionary&lt;string, string&gt; ExtraHeaderParameters
    }
    class DownstreamApiOptionsReadOnlyHttpMethod {
    +DownstreamApiOptionsReadOnlyHttpMethod Clone()
    #AuthorizationHeaderProviderOptions CloneInternal()
    &gt;&gt;ro&gt;&gt; +string HttpMethod
    }
    class IAuthorizationHeaderProvider { <<interface>>
    +Task&lt;string&gt; CreateAuthorizationHeaderForUserAsync(IEnumerable&lt;string&gt; scopes, AuthorizationHeaderProviderOptions authorizationHeaderProviderOptions, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
    +Task&lt;string&gt; CreateAuthorizationHeaderForAppAsync(string scopes, AuthorizationHeaderProviderOptions downstreamApiOptions, CancellationToken cancellationToken)
    +Task&lt;string&gt; CreateAuthorizationHeaderAsync(IEnumerable&lt;string&gt; scopes, AuthorizationHeaderProviderOptions options, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
    }
    class IDownstreamApi { <<interface>>
    +Task&lt;HttpResponseMessage&gt; CallApiAsync(DownstreamApiOptions downstreamApiOptions, ClaimsPrincipal user, HttpContent content, CancellationToken cancellationToken)
    +Task&lt;HttpResponseMessage&gt; CallApiAsync(string serviceName, Action&lt;DownstreamApiOptions&gt; downstreamApiOptionsOverride, ClaimsPrincipal user, HttpContent content, CancellationToken cancellationToken)
    +Task&lt;HttpResponseMessage&gt; CallApiForUserAsync(string serviceName, Action&lt;DownstreamApiOptions&gt; downstreamApiOptionsOverride, ClaimsPrincipal user, HttpContent content, CancellationToken cancellationToken)
    +Task&lt;HttpResponseMessage&gt; CallApiForAppAsync(string serviceName, Action&lt;DownstreamApiOptions&gt; downstreamApiOptionsOverride, HttpContent content, CancellationToken cancellationToken)
    +Task&lt;IDownstreamApi.TOutput&gt; CallApiForUserAsync(string serviceName, IDownstreamApi.TInput input, Action&lt;DownstreamApiOptions&gt; downstreamApiOptionsOverride, ClaimsPrincipal user, CancellationToken cancellationToken)
    +Task&lt;IDownstreamApi.TOutput&gt; CallApiForUserAsync(string serviceName, Action&lt;DownstreamApiOptions&gt; downstreamApiOptionsOverride, ClaimsPrincipal user, CancellationToken cancellationToken)
    +Task&lt;IDownstreamApi.TOutput&gt; CallApiForAppAsync(string serviceName, IDownstreamApi.TInput input, Action&lt;DownstreamApiOptions&gt; downstreamApiOptionsOverride, CancellationToken cancellationToken)
    +Task&lt;IDownstreamApi.TOutput&gt; CallApiForAppAsync(string serviceName, Action&lt;DownstreamApiOptions&gt; downstreamApiOptionsOverride, CancellationToken cancellationToken)
    +Task&lt;IDownstreamApi.TOutput&gt; CallApiForUserAsync(string serviceName, IDownstreamApi.TInput input, JsonTypeInfo&lt;IDownstreamApi.TInput&gt; inputJsonTypeInfo, JsonTypeInfo&lt;IDownstreamApi.TOutput&gt; outputJsonTypeInfo, Action&lt;DownstreamApiOptions&gt; downstreamApiOptionsOverride, ClaimsPrincipal user, CancellationToken cancellationToken)
    +Task&lt;IDownstreamApi.TOutput&gt; CallApiForUserAsync(string serviceName, JsonTypeInfo&lt;IDownstreamApi.TOutput&gt; outputJsonTypeInfo, Action&lt;DownstreamApiOptions&gt; downstreamApiOptionsOverride, ClaimsPrincipal user, CancellationToken cancellationToken)
    +Task&lt;IDownstreamApi.TOutput&gt; CallApiForAppAsync(string serviceName, IDownstreamApi.TInput input, JsonTypeInfo&lt;IDownstreamApi.TInput&gt; inputJsonTypeInfo, JsonTypeInfo&lt;IDownstreamApi.TOutput&gt; outputJsonTypeInfo, Action&lt;DownstreamApiOptions&gt; downstreamApiOptionsOverride, CancellationToken cancellationToken)
    +Task&lt;IDownstreamApi.TOutput&gt; CallApiForAppAsync(string serviceName, JsonTypeInfo&lt;IDownstreamApi.TOutput&gt; outputJsonTypeInfo, Action&lt;DownstreamApiOptions&gt; downstreamApiOptionsOverride, CancellationToken cancellationToken)
    +Task&lt;IDownstreamApi.TOutput&gt; GetForUserAsync(string serviceName, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, ClaimsPrincipal user, CancellationToken cancellationToken)
    +Task&lt;IDownstreamApi.TOutput&gt; GetForUserAsync(string serviceName, IDownstreamApi.TInput input, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, ClaimsPrincipal user, CancellationToken cancellationToken)
    +Task&lt;IDownstreamApi.TOutput&gt; GetForAppAsync(string serviceName, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, CancellationToken cancellationToken)
    +Task&lt;IDownstreamApi.TOutput&gt; GetForAppAsync(string serviceName, IDownstreamApi.TInput input, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, CancellationToken cancellationToken)
    +Task PostForUserAsync(string serviceName, IDownstreamApi.TInput input, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, ClaimsPrincipal user, CancellationToken cancellationToken)
    +Task&lt;IDownstreamApi.TOutput&gt; PostForUserAsync(string serviceName, IDownstreamApi.TInput input, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, ClaimsPrincipal user, CancellationToken cancellationToken)
    +Task PostForAppAsync(string serviceName, IDownstreamApi.TInput input, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, CancellationToken cancellationToken)
    +Task&lt;IDownstreamApi.TOutput&gt; PostForAppAsync(string serviceName, IDownstreamApi.TInput input, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, CancellationToken cancellationToken)
    +Task PutForUserAsync(string serviceName, IDownstreamApi.TInput input, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, ClaimsPrincipal user, CancellationToken cancellationToken)
    +Task&lt;IDownstreamApi.TOutput&gt; PutForUserAsync(string serviceName, IDownstreamApi.TInput input, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, ClaimsPrincipal user, CancellationToken cancellationToken)
    +Task PutForAppAsync(string serviceName, IDownstreamApi.TInput input, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, CancellationToken cancellationToken)
    +Task&lt;IDownstreamApi.TOutput&gt; PutForAppAsync(string serviceName, IDownstreamApi.TInput input, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, CancellationToken cancellationToken)
    +Task PatchForUserAsync(string serviceName, IDownstreamApi.TInput input, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, ClaimsPrincipal user, CancellationToken cancellationToken)
    +Task&lt;IDownstreamApi.TOutput&gt; PatchForUserAsync(string serviceName, IDownstreamApi.TInput input, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, ClaimsPrincipal user, CancellationToken cancellationToken)
    +Task PatchForAppAsync(string serviceName, IDownstreamApi.TInput input, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, CancellationToken cancellationToken)
    +Task&lt;IDownstreamApi.TOutput&gt; PatchForAppAsync(string serviceName, IDownstreamApi.TInput input, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, CancellationToken cancellationToken)
    +Task DeleteForUserAsync(string serviceName, IDownstreamApi.TInput input, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, ClaimsPrincipal user, CancellationToken cancellationToken)
    +Task&lt;IDownstreamApi.TOutput&gt; DeleteForUserAsync(string serviceName, IDownstreamApi.TInput input, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, ClaimsPrincipal user, CancellationToken cancellationToken)
    +Task DeleteForAppAsync(string serviceName, IDownstreamApi.TInput input, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, CancellationToken cancellationToken)
    +Task&lt;IDownstreamApi.TOutput&gt; DeleteForAppAsync(string serviceName, IDownstreamApi.TInput input, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, CancellationToken cancellationToken)
    +Task&lt;IDownstreamApi.TOutput&gt; GetForUserAsync(string serviceName, JsonTypeInfo&lt;IDownstreamApi.TOutput&gt; outputJsonTypeInfo, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, ClaimsPrincipal user, CancellationToken cancellationToken)
    +Task&lt;IDownstreamApi.TOutput&gt; GetForUserAsync(string serviceName, IDownstreamApi.TInput input, JsonTypeInfo&lt;IDownstreamApi.TInput&gt; inputJsonTypeInfo, JsonTypeInfo&lt;IDownstreamApi.TOutput&gt; outputJsonTypeInfo, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, ClaimsPrincipal user, CancellationToken cancellationToken)
    +Task&lt;IDownstreamApi.TOutput&gt; GetForAppAsync(string serviceName, JsonTypeInfo&lt;IDownstreamApi.TOutput&gt; outputJsonTypeInfo, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, CancellationToken cancellationToken)
    +Task&lt;IDownstreamApi.TOutput&gt; GetForAppAsync(string serviceName, IDownstreamApi.TInput input, JsonTypeInfo&lt;IDownstreamApi.TInput&gt; inputJsonTypeInfo, JsonTypeInfo&lt;IDownstreamApi.TOutput&gt; outputJsonTypeInfo, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, CancellationToken cancellationToken)
    +Task PostForUserAsync(string serviceName, IDownstreamApi.TInput input, JsonTypeInfo&lt;IDownstreamApi.TInput&gt; inputJsonTypeInfo, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, ClaimsPrincipal user, CancellationToken cancellationToken)
    +Task&lt;IDownstreamApi.TOutput&gt; PostForUserAsync(string serviceName, IDownstreamApi.TInput input, JsonTypeInfo&lt;IDownstreamApi.TInput&gt; inputJsonTypeInfo, JsonTypeInfo&lt;IDownstreamApi.TOutput&gt; outputJsonTypeInfo, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, ClaimsPrincipal user, CancellationToken cancellationToken)
    +Task PostForAppAsync(string serviceName, IDownstreamApi.TInput input, JsonTypeInfo&lt;IDownstreamApi.TInput&gt; inputJsonTypeInfo, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, CancellationToken cancellationToken)
    +Task&lt;IDownstreamApi.TOutput&gt; PostForAppAsync(string serviceName, IDownstreamApi.TInput input, JsonTypeInfo&lt;IDownstreamApi.TInput&gt; inputJsonTypeInfo, JsonTypeInfo&lt;IDownstreamApi.TOutput&gt; outputJsonTypeInfo, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, CancellationToken cancellationToken)
    +Task PutForUserAsync(string serviceName, IDownstreamApi.TInput input, JsonTypeInfo&lt;IDownstreamApi.TInput&gt; inputJsonTypeInfo, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, ClaimsPrincipal user, CancellationToken cancellationToken)
    +Task&lt;IDownstreamApi.TOutput&gt; PutForUserAsync(string serviceName, IDownstreamApi.TInput input, JsonTypeInfo&lt;IDownstreamApi.TInput&gt; inputJsonTypeInfo, JsonTypeInfo&lt;IDownstreamApi.TOutput&gt; outputJsonTypeInfo, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, ClaimsPrincipal user, CancellationToken cancellationToken)
    +Task PutForAppAsync(string serviceName, IDownstreamApi.TInput input, JsonTypeInfo&lt;IDownstreamApi.TInput&gt; inputJsonTypeInfo, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, CancellationToken cancellationToken)
    +Task&lt;IDownstreamApi.TOutput&gt; PutForAppAsync(string serviceName, IDownstreamApi.TInput input, JsonTypeInfo&lt;IDownstreamApi.TInput&gt; inputJsonTypeInfo, JsonTypeInfo&lt;IDownstreamApi.TOutput&gt; outputJsonTypeInfo, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, CancellationToken cancellationToken)
    +Task PatchForUserAsync(string serviceName, IDownstreamApi.TInput input, JsonTypeInfo&lt;IDownstreamApi.TInput&gt; inputJsonTypeInfo, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, ClaimsPrincipal user, CancellationToken cancellationToken)
    +Task&lt;IDownstreamApi.TOutput&gt; PatchForUserAsync(string serviceName, IDownstreamApi.TInput input, JsonTypeInfo&lt;IDownstreamApi.TInput&gt; inputJsonTypeInfo, JsonTypeInfo&lt;IDownstreamApi.TOutput&gt; outputJsonTypeInfo, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, ClaimsPrincipal user, CancellationToken cancellationToken)
    +Task PatchForAppAsync(string serviceName, IDownstreamApi.TInput input, JsonTypeInfo&lt;IDownstreamApi.TInput&gt; inputJsonTypeInfo, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, CancellationToken cancellationToken)
    +Task&lt;IDownstreamApi.TOutput&gt; PatchForAppAsync(string serviceName, IDownstreamApi.TInput input, JsonTypeInfo&lt;IDownstreamApi.TInput&gt; inputJsonTypeInfo, JsonTypeInfo&lt;IDownstreamApi.TOutput&gt; outputJsonTypeInfo, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, CancellationToken cancellationToken)
    +Task DeleteForUserAsync(string serviceName, IDownstreamApi.TInput input, JsonTypeInfo&lt;IDownstreamApi.TInput&gt; inputJsonTypeInfo, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, ClaimsPrincipal user, CancellationToken cancellationToken)
    +Task&lt;IDownstreamApi.TOutput&gt; DeleteForUserAsync(string serviceName, IDownstreamApi.TInput input, JsonTypeInfo&lt;IDownstreamApi.TInput&gt; inputJsonTypeInfo, JsonTypeInfo&lt;IDownstreamApi.TOutput&gt; outputJsonTypeInfo, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, ClaimsPrincipal user, CancellationToken cancellationToken)
    +Task DeleteForAppAsync(string serviceName, IDownstreamApi.TInput input, JsonTypeInfo&lt;IDownstreamApi.TInput&gt; inputJsonTypeInfo, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, CancellationToken cancellationToken)
    +Task&lt;IDownstreamApi.TOutput&gt; DeleteForAppAsync(string serviceName, IDownstreamApi.TInput input, JsonTypeInfo&lt;IDownstreamApi.TInput&gt; inputJsonTypeInfo, JsonTypeInfo&lt;IDownstreamApi.TOutput&gt; outputJsonTypeInfo, Action&lt;DownstreamApiOptionsReadOnlyHttpMethod&gt; downstreamApiOptionsOverride, CancellationToken cancellationToken)
    }

    AuthorizationHeaderProviderOptions <|-- DownstreamApiOptions : Inherits
    DownstreamApiOptions <|-- DownstreamApiOptionsReadOnlyHttpMethod : Inherits
    CredentialDescription --> "DecryptKeysAuthenticationOptions" AuthorizationHeaderProviderOptions : Has
    AuthorizationHeaderProviderOptions --> "AcquireTokenOptions" AcquireTokenOptions : Has
    AcquireTokenOptions --> "ManagedIdentity" ManagedIdentityOptions : Has
    IDownstreamApi ..> DownstreamApiOptions : Uses
    IAuthorizationHeaderProvider ..> AuthorizationHeaderProviderOptions : Uses
    IAuthorizationHeaderProvider~TResult~ { <<interface>> }

```


### Extensibility
Credential loading extensibility points.

```mermaid
classDiagram
    class CredentialSourceLoaderParameters {
        +string ClientId
        +string Authority
    }
    class ICredentialSourceLoader { <<interface>>
        +Task LoadIfNeededAsync(CredentialDescription, CredentialSourceLoaderParameters?)
        +CredentialSource CredentialSource
    }
    class ICustomSignedAssertionProvider { <<interface>>
        +string Name
    }
    class ICredentialsLoader { <<interface>>
        +IDictionary&lt;CredentialSource, ICredentialSourceLoader&gt; CredentialSourceLoaders
        +Task LoadCredentialsIfNeededAsync(CredentialDescription, CredentialSourceLoaderParameters?)
        +Task<CredentialDescription?> LoadFirstValidCredentialsAsync(IEnumerable&lt;CredentialDescription&gt;, CredentialSourceLoaderParameters?)
        +void ResetCredentials(IEnumerable<CredentialDescription>)
    }
    ICredentialSourceLoader <|-- ICustomSignedAssertionProvider : Inherits
    ICredentialsLoader --> ICredentialSourceLoader : Uses
    ICredentialSourceLoader --> CredentialSourceLoaderParameters : Uses
```

## Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Trademarks

This project may contain trademarks or logos for projects, products, or services. Authorized use of Microsoft
trademarks or logos is subject to and must follow
[Microsoft's Trademark & Brand Guidelines](https://www.microsoft.com/en-us/legal/intellectualproperty/trademarks/usage/general).
Use of Microsoft trademarks or logos in modified versions of this project must not cause confusion or imply Microsoft sponsorship.
Any use of third-party trademarks or logos are subject to those third-party's policies.
