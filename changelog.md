2.0.0
========
- Rename DownstreamRestApi to DownstreamApi. 

1.2.0
========
- Fixes [54](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/issues/54)

1.1.0
========
- Releasing non-preview version

1.0.6-preview
========
- [#48](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/issues/48)
- [#45](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/issues/45)

1.0.5-preview
========
## API Changes to improve the developer experience
- New property `TokenType` on AcquireTokenResult.
- ApplicationAuthenticationOptions renamed to IdentityApplicationOptions, and MicrosoftAuthenticationOptions to MicrosoftIdentityApplicationOptions
- Removed ITokenAcquirerFactory.GetTokenAcquirer(string authority, string clientId, System.Collections.Generic.IEnumerable<CredentialDescription> clientCredentials, string? region), as the same is doable with GetTokenAcquirer(IdentityApplicationOptions identityApplicationOptions)
- Added helpers to IDownstreamRestApi for each of the Http methods.
- Split DownstreamRestApiOptions into AuthorizationHeaderProviderOptions (now used in IAuthorizationHeaderProvider), and DownstreamRestApiOptions, which adds the scopes. A new derived class DownstreamRestApiOptionsReadOnlyHttpMethod enables a better developer experience in the IDownstreamWebApi methods which names starts with an HttpMethod (no confusion and risk to change the HTTP method in the delegate)

1.0.4-preview
========
## Feature
- Adding extensibility for credentials: see [#30](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/pull/30/files)

1.0.3-preview
==========
## Bug fix:
- Remove param from Interface.

1.0.0-preview
==========
## Bug fix:
- CorrelationId should be a string and not a GUID. See [issue](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/issues/20) for details.
- Rename `AuthenticationOptions` to `ApplicationAuthenticationOptions`.

2.0.2-preview
==========
## Bug fix:
- Remove the default region.

2.0.0
==========
Initial release of Microsoft.Identity.Abstractions which brings interfaces and POCO classes used in all the Microsoft .NET authentication libraries provided by Identity and Network Access (IDNA) see ReadME.md for details.
