1.0.1-preview
==========
# Bug fix:
- Revert back CorrelationId to a GUID, as MSAL.NET requires a GUID.

1.0.0-preview
==========
# Bug fix:
- CorrelationId should be a string and not a GUID. See [issue](https://github.com/AzureAD/microsoft-identity-abstractions-for-dotnet/issues/20) for details.
- Rename `AuthenticationOptions` to `ApplicationAuthenticationOptions`.

2.0.2-preview
==========
# Bug fix:
- Remove the default region.

2.0.0
==========
Initial release of Microsoft.Identity.Abstractions which brings interfaces and POCO classes used in all the Microsoft .NET authentication libraries provided by Identity and Network Access (IDNA) see ReadME.md for details.
