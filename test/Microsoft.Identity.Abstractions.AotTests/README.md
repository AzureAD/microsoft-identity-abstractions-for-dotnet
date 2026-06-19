# Microsoft.Identity.Abstractions AOT Tests

This project tests AOT (Ahead-of-Time) compilation compatibility for Microsoft.Identity.Abstractions, specifically for configuration binding scenarios on .NET 10+.

## Purpose

The AOT test project verifies that:
1. `CredentialDescription` and related types can be bound from configuration without reflection
2. Extension properties work correctly with configuration binders
3. No AOT warnings are generated during publish

## Running the Tests

### On Windows

```batch
cd test/Microsoft.Identity.Abstractions.AotTests
Publish.bat
```

### On Linux/macOS

```bash
cd test/Microsoft.Identity.Abstractions.AotTests
dotnet publish --runtime linux-x64 -f net10.0
./bin/Release/net10.0/linux-x64/publish/Microsoft.Identity.Abstractions.AotTests
```

### Manual Publish

```bash
dotnet publish --runtime <RID> -f net10.0
```

Where `<RID>` is one of:
- `win-x64` (Windows)
- `linux-x64` (Linux)
- `osx-x64` (macOS Intel)
- `osx-arm64` (macOS Apple Silicon)

## What It Tests

The test application:
1. Loads configuration from `appsettings.json`
2. Binds `MicrosoftIdentityApplicationOptions` with `CredentialDescription` objects
3. Binds `DownstreamApisOptions`
4. Verifies all properties are correctly bound
5. Outputs success messages if binding worked

## Expected Output

```
ClientId: 6c4e1e3e-9b3e-4b3e-8e3e-1e3e9b3e4b3e
ClientCredentials: CertificateFromKeyVault=https://mykeyvault.vault.azure.net/myCertName;Thumbprint=null
DownstreamApis count: 2
DownstreamApi: Api1
DownstreamApi: https://api1.com
DownstreamApi: Api2
DownstreamApi: https://api2.com
AOT test completed successfully!
```

## Key Features

- Uses `PublishAot=true` to enable AOT compilation
- Uses `EnableConfigurationBindingGenerator=true` for source-generated configuration binding
- Tests that Certificate and CachedValue properties (implemented as extension properties on .NET 10+) work with configuration binding
- Verifies no reflection warnings during publish

## Related

This test project validates the AOT compatibility changes made in version 11.0.0 where `Certificate` and `CachedValue` were converted to C# 14 extension properties to hide them from configuration binders.
