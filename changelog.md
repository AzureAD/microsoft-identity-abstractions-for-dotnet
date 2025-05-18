9.0.0
======
## New features
* Add a new generic IAuthorizationHeaderProvider<TResult> to have the possiblity of returning authorization header and metadata or error instead of throwing. For details see [#172](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/pull/172)
* Add Algorithm property to CredentialDescription to describe signing credentials. For details see [#182](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/issues/182)
* Adding serializer for CredentialDescription in .NET 8+. See [#176](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/pull/176)

## Foundamentals
* Add dev container to work in Code Spaces. See PR [#175](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/pull/175)
* Adding a doc about CredentialDescription. See PR [#181](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/pull/181)
* Fixing AoT warnings: part 1 - non breaking. See PR [#187](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/pull/187)
* update Readme.md to explain the support policy for the library and the notion of LTS. See PRs [171](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/pull/171), [183](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/pull/184), , [185](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/pull/185) 

9.0.0
======
## New features

- Added a new class named `MicrosoftEntraApplicationOptions` inheriting from `IdentityApplicationOptions` and from which `MicrosoftIdentityApplicationOptions` inherits. Moved the EntraID specific 
  properties related to web APIs from `MicrosoftIdentityApplicationOptions` to `MicrosoftEntraApplicationOptions`. `MicrosoftIdentityApplicationOptions` now only contains the
  properties related to web apps and B2C. See [#165](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/issues/165) for details.
- Added a `Name` property in `MicrosoftEntraApplicationOptions` to allow for dynamic discovery of ASP.NET Core authentication schemes / named options. See [#168](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/issues/168) for details.
- Changed the way the ID property is computed in ClientCredentials. All sensitive data is also now replaced by a hash. See [#163](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/issues/163) for details.
- Added XML comments with recommendations on which CredentialSource not to use in production. See [#167](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/pull/167) for details.

8.2.0
======
- To support Federated Managed Identities a new parameter `FmiPath` was added to `AcquireTokenOptions`. See [#161](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/pull/161) for details.

8.1.1
======
**

8.1.0
======
## New features:
- To support certain Federation identity cases, you need to add an additional parameter called `TokenExchangeAuthority`. This parameter is necessary when the issuer (the entity that issues the token) for the token exchange URL is different from the application's issuer. See [#155](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/issues/155) for details.
- Added a new interface `ICustomSignedAssertionProvider` for implementing custom signed assertion providers. This interface includes a `Name` property for configuration-friendly naming. See issue [#153](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/issues/153) for details.
- Added extensibility to the `CredentialDescription` class to support custom signed assertion providers. This includes new properties `CustomSignedAssertionProviderName` and `CustomSignedAssertionProviderData`. See issue [#146](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/issues/146) for details.

8.0.0
========
## Fundamentals:

- Removed the Container and ValueOrReference from the public API of CredentialDescription. They were technical debt used for compatibility
  with Microsoft.Identity.Web 1.x, no longer necessary. See [PR #151](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/pull/151)
  for details.

7.2.1
========
## Bug fix:
- `Id` property in `CredentialDescription` was derived from secret values, primarily affecting logging (information level) of credential attempts in `Microsoft.Identity.Web`, it doesn't affect higher log levels because if the failure occurs, it indicates that a credential description has both a credential source that can fail (e.g., certificate) and the `ClientSecret` property set, which is not a typical scenario. See issue [#147](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/pull/147) for details.

7.2.0
========
## New features:
- Add `AppHomeTenantId` to `MicrosoftIdentityApplicationOptions` to allow multi-tenant applications to specify the `AppHomeTenantId` to be used for client credentials. See PR [#142](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/pull/142) and [Id Web](https://github.com/AzureAD/microsoft-identity-web/issues/3121) for details.

7.1.0
========
## New features:
- Add support for internal Microsoft services for token acquisition extensibility. See issue [#135](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/pull/135) for details.

## Engineering excellence
- Add publicAPI, bannedAPI and Async analyzers. See issue [#136](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/pull/136) for details.
- Fix compiler warnings. See issue [#137](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/pull/137) for details.

7.0.0
========
## Breaking changes:
- Extends the 'IDownstreamApi' interface to include overrides with `JsonTypeInfo<T>` parameters for source generated JSON serialization. See  [PR](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/pull/131) for details.

6.0.0
========
## Breaking changes:
- Updates the 'IAuthorizationHeaderProvider' interface to include a new method 'GetAuthorizationHeaderAsync'. See issue [#130](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/issues/130) for details.

5.3.0
========
## New features:
- Added two new properties `AcceptHeader` and `ContentType` to `DownstreamApiOptions` class. See issue [#123](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/issues/123) for details.

## Bug fix:
- Fix file path for xml comment. See issue [#117](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/issues/117) for details.

5.2.0
========
- Added a `TokenExchangeUrl` to the [CredentialDescription](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/blob/main/src/Microsoft.Identity.Abstractions/ApplicationOptions/CredentialDescription.cs#L480) class.

5.1.0
========
## API additions to enable support for managed identities.
- Created a new [ManagedIdentityOptions class](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/blob/main/src/Microsoft.Identity.Abstractions/TokenAcquisition/ManagedIdentityOptions.cs).
- Added a 'ManagedIdentity' property to the [AcquireTokenOptions class](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/blob/main/src/Microsoft.Identity.Abstractions/TokenAcquisition/AcquireTokenOptions.cs). See [#115](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/issues/115) for details.

5.0.0
========
- Introduce a unique identifier for a CredentialDescription object. See [PR](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/pull/94) for details.

- Change `AuthorizationHeaderProviderOptions` to use a `string` instead of `HttpMethod`. See [PR](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/pull/99) for details. This is a breaking change, but shouldn't affect you if you are using the configuration.

- Add integrated API compatibility. See [PR](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/pull/99) for details.


4.1.0
========
- New `Id` property on CredentialDescription. See [PR](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/pull/94) for details

4.0.0
========
- Use Assembly Reference instead of PackageReference. See [PR](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/pull/91) for details.

3.2.1
========
- Improve the XML documentation (See [#85](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/pull/85) and [#86](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/pull/86))
- Add unit tests for a 100% code coverage

3.2.0
========
- Add `RequiresUnreferencedCode` attribute to `IDownstreamApi` and `IDownstreamApiHttpMethods`. See [#82](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/pull/82) for details.

3.1.0
========
- Add `ExtraQueryParameters` to `AcquireTokenOptions`. See [pr](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/pull/79) for details.

3.0.1
========
- Re-add support for net462.

3.0.0
========
- Rename `JwtClaim` to `PopClaim` in `AcquireTokenOptions`. See issue [#74](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/issues/74) for details.
- Remove support for net462.

2.1.0
========
- Support a credential description for auto decrypt keys. See issue [#65](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/issues/65) for details.
- Add `JwtClaim` to `AquireTokenOptions`. See issue [#67]([Support a credential description for auto decrypt keys](https://github.com/AzureAD)/microsoft-identity-abstractions-for-dotnet/issues/67) for details.

2.0.1
========
- Rename `CallAsync` to `CallApiAsync`

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
