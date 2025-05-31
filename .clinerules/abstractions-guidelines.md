Microsoft.Identity.Abstractions Guidelines

## Overview

Microsoft.Identity.Abstractions is a core authentication and authorization library that provides essential interfaces and base classes for implementing identity and access management in .NET applications. The library specializes in:

- Authentication and authorization abstractions
- Token acquisition and management interfaces
- Credential handling abstractions
- Client configuration interfaces
- Protocol-agnostic identity concepts
- High-performance authentication middleware support

Through its well-designed abstractions and interfaces, Microsoft.Identity.Abstractions enables consistent authentication patterns across different authentication providers and scenarios.

## Repository Structure

### Core Directories
- `/src` - Contains all source code for the Microsoft.Identity.Abstractions package
  - Authentication - Core authentication interfaces
  - Authorization - Authorization abstractions
  - Credentials - Credential management interfaces
  - Configuration - Authentication configuration abstractions
  - TokenAcquisition - Token handling interfaces
- `/tests` - Unit tests, integration tests, and test utilities
- `/samples` - Example implementations and usage patterns
- `/build` - Build configuration and scripts

## Key Components

### Core Abstractions
- IAuthenticationClientFactory - Factory for creating authentication clients
- ICredentialProvider - Interface for credential management
- ITokenAcquirer - Core interface for token acquisition
- IAuthenticationConfiguration - Authentication configuration interface

### Authentication Support
- AuthenticationOptionsBase - Base class for authentication options
- AuthenticationClientBase - Base implementation for authentication clients
- TokenRequestContext - Context for token requests
- CredentialDescription - Credential information abstraction

### Configuration and Integration
- AuthenticationClientOptions - Client configuration options
- CredentialSourceOptions - Credential source configuration
- TokenAcquirerOptions - Token acquisition settings
- AuthenticationProtocolOptions - Protocol configuration abstractions

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
