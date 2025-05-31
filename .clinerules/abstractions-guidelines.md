Microsoft.Identity.Abstractions Guidelines

## Overview

Microsoft.Identity.Abstractions is a core authentication and authorization library that provides essential interfaces and base classes for implementing identity and access management in .NET applications. The library specializes in:

- Abstractions and options for applications using Microsoft Entra ID
- Token acquisition options and interfaces
- Credential handling abstractions
- Protocol-agnostic identity concepts

Through its well-designed abstractions and interfaces, Microsoft.Identity.Abstractions enables consistent authentication patterns across different authentication providers and scenarios.

## Repository Structure

### Core Directories
- `/src` - Contains all source code for the Microsoft.Identity.Abstractions package
  - ApplicationOptions - Options decribing the authentication part of applications
  - Credentials - Option describing credentials (secrets, certificate, signed assertions)
  - TokenAcquisition - Options and interfaces to acquire tokens, and creating Authorization headers
- `/tests` - Unit tests
- `/build` - Build configuration and scripts

## Key Components

### Core Abstractions
- CredentialDescription - describes a credential
- MicrosoftIdentityApplicationOptions - describes an application
- ICredentialProvider - Interface to bring your own credential loaders
- ITokenAcquirer - Core interface for token acquisition
- ITokenAcquirerFactory - Factory of Token acquirers
- IAuthorizationHeaderProvider - creates authorization headers (getting tokens and building the protocol string)
- IDownstreamApi - call downstream APIs in an authenticated way.

## Development Guidelines

### Core Development Principles
- Follow .editorconfig rules strictly
- Design for extensibility and flexibility
- Maintain backward compatibility
- Keep abstractions clean and focused
- Minimize concrete implementations

### Interface Design Requirements
- Keep interfaces focused and cohesive
- Follow Interface Segregation Principle
- Document interface contracts thoroughly
- Consider extension points
- Maintain consistent naming patterns

### Abstraction Guidelines
- Create purpose-specific abstractions
- Avoid leaking implementation details
- Design for dependency injection
- Support multiple authentication scenarios
- Enable easy testing through abstractions

### Testing Requirements
- Test interface contracts thoroughly
- Include mocked implementations
- Verify extensibility points
- Test configuration patterns
- Validate integration scenarios

### Public API Changes
- The project uses Microsoft.CodeAnalysis.PublicApiAnalyzers
- For any public API changes:
  1. Update PublicAPI.Unshipped.txt in the package directory
  2. Include complete interface and abstract class signatures
  3. Consider backward compatibility impacts
  4. Document breaking changes clearly

Example format:
```diff
// Adding new API
+Microsoft.Identity.Abstractions.ITokenAcquirer.GetTokenAsync(TokenRequestContext context) -> System.Threading.Tasks.Task<string>
+Microsoft.Identity.Abstractions.AuthenticationClientOptions.Scopes.get -> System.Collections.Generic.IEnumerable<string>

// Removing API (requires careful consideration)
-Microsoft.Identity.Abstractions.ObsoleteAuthenticationMethod() -> void
```

The analyzer enforces documentation of all public API changes in PublicAPI.Unshipped.txt and will fail the build if changes are not properly reflected.
