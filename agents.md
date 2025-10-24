# Agent Automation and Development Guidelines

This document contains comprehensive guidelines for AI agents, development standards, and automation workflows for the Microsoft.Identity.Abstractions repository. These guidelines help ensure consistent code quality, development practices, and effective AI-assisted development.

---

## Table of Contents

1. [Repository Overview](#repository-overview)
2. [AI Assistant Guidelines](#ai-assistant-guidelines)
3. [C# Development Standards](#c-development-standards)
4. [Microsoft.Identity.Abstractions Guidelines](#microsoftidentityabstractions-guidelines)
5. [Agent Configuration](#agent-configuration)

---

## Repository Overview

Microsoft.Identity.Abstractions is a core authentication and authorization library that provides essential interfaces and base classes for implementing identity and access management in .NET applications. This repository follows specific development and automation standards to maintain high code quality and contributor experience.

### Key Features
- Abstractions and options for applications using Microsoft Entra ID
- Token acquisition options and interfaces
- Credential handling abstractions
- Protocol-agnostic identity concepts

---

## AI Assistant Guidelines

### Core Principles

* Make changes incrementally and verify each step
* Always analyze existing code patterns before making changes
* Prioritize built-in tools over shell commands
* Follow existing project patterns and conventions
* Maintain comprehensive test coverage

### Tool Usage

#### File Operations
* Use `read_file` for examining file contents instead of shell commands like `cat`
* Use `replace_in_file` for targeted, specific changes to existing files
* Use `write_to_file` only for new files or complete file rewrites
* Use `list_files` to explore directory structures
* Use `search_files` with precise regex patterns to find code patterns
* Use `list_code_definition_names` to understand code structure before modifications

#### Command Execution
* Use `execute_command` sparingly, preferring built-in file operation tools when possible
* Always provide clear explanations for any executed commands
* Set `requires_approval` to true for potentially impactful operations

### Development Workflow

#### Planning Phase (PLAN MODE)
* Begin complex tasks in PLAN mode to discuss approach
* Analyze existing codebase patterns using search tools
* Review related test files to understand testing patterns
* Present clear implementation steps for approval
* Ask clarifying questions early to avoid rework

#### Implementation Phase (ACT MODE)
* Make changes incrementally, one file at a time
* Verify each change before proceeding
* Follow patterns discovered during planning phase
* Focus on maintaining test coverage
* Use error messages and linter feedback to guide fixes

### Code Modifications

#### General Guidelines
* Follow .editorconfig rules strictly
* Preserve file headers and license information
* Maintain consistent XML documentation
* Respect existing error handling patterns
* Keep line endings consistent with existing files

#### Quality Checks
* Verify changes match existing code style
* Ensure test coverage for new code
* Validate changes against project conventions
* Check for proper error handling
* Maintain nullable reference type annotations

### MCP Server Integration

* Use appropriate MCP tools when available for specialized tasks
* Access MCP resources efficiently using proper URIs
* Handle MCP operation results appropriately
* Follow server-specific authentication and usage patterns

### Error Handling

* Provide clear error messages and suggestions
* Handle tool operation failures gracefully
* Suggest alternative approaches when primary approach fails
* Roll back changes if necessary to maintain stability

---

## C# Development Standards

### General

* Always use the latest version C#, currently C# 13 features
* Never change global.json unless explicitly asked to
* Never change package.json or package-lock.json files unless explicitly asked to
* Never change NuGet.config files unless explicitly asked to

### Formatting

* Apply code-formatting style defined in `.editorconfig`
* Prefer file-scoped namespace declarations and single-line using directives
* Insert a newline before the opening curly brace of any code block (e.g., after `if`, `for`, `while`, `foreach`, `using`, `try`, etc.)
* Ensure that the final return statement of a method is on its own line
* Use pattern matching and switch expressions wherever possible
* Use `nameof` instead of string literals when referring to member names
* Ensure that XML doc comments are created for any public APIs. When applicable, include `<example>` and `<code>` documentation in the comments

### Nullable Reference Types

* Declare variables non-nullable, and check for `null` at entry points
* Always use `is null` or `is not null` instead of `== null` or `!= null`
* Trust the C# null annotations and don't add null checks when the type system says a value cannot be null

### Testing

* We use xUnit SDK v2 for tests
* Emit "Act", "Arrange" or "Assert" comments
* Use Moq 4.14.x for mocking in tests
* Copy existing style in nearby files for test method names and capitalization

### Running tests

* To build and run tests in the repo, run `dotnet test`, you need one solution open, or specify the solution

---

## Microsoft.Identity.Abstractions Guidelines

### Overview

Microsoft.Identity.Abstractions is a core authentication and authorization library that provides essential interfaces and base classes for implementing identity and access management in .NET applications. The library specializes in:

- Abstractions and options for applications using Microsoft Entra ID
- Token acquisition options and interfaces
- Credential handling abstractions
- Protocol-agnostic identity concepts

Through its well-designed abstractions and interfaces, Microsoft.Identity.Abstractions enables consistent authentication patterns across different authentication providers and scenarios.

### Repository Structure

#### Core Directories
- `/src` - Contains all source code for the Microsoft.Identity.Abstractions package
  - ApplicationOptions - Options describing the authentication part of applications
  - Credentials - Option describing credentials (secrets, certificate, signed assertions)
  - TokenAcquisition - Options and interfaces to acquire tokens, and creating Authorization headers
- `/tests` - Unit tests
- `/build` - Build configuration and scripts

### Key Components

#### Core Abstractions
- CredentialDescription - describes a credential
- MicrosoftIdentityApplicationOptions - describes an application
- ICredentialProvider - Interface to bring your own credential loaders
- ITokenAcquirer - Core interface for token acquisition
- ITokenAcquirerFactory - Factory of Token acquirers
- IAuthorizationHeaderProvider - creates authorization headers (getting tokens and building the protocol string)
- IAuthorizationHeaderBoundProvider - extends IAuthorizationHeaderProvider to provide authorization headers with bound certificate information
- IDownstreamApi - call downstream APIs in an authenticated way.

### Development Guidelines

#### Core Development Principles
- Follow .editorconfig rules strictly
- Design for extensibility and flexibility
- Maintain backward compatibility
- Keep abstractions clean and focused
- Minimize concrete implementations

#### Interface Design Requirements
- Keep interfaces focused and cohesive
- Follow Interface Segregation Principle
- Document interface contracts thoroughly
- Consider extension points
- Maintain consistent naming patterns

#### Abstraction Guidelines
- Create purpose-specific abstractions
- Avoid leaking implementation details
- Design for dependency injection
- Support multiple authentication scenarios
- Enable easy testing through abstractions

#### Testing Requirements
- Test interface contracts thoroughly
- Include mocked implementations
- Verify extensibility points
- Test configuration patterns
- Validate integration scenarios

### Public and Internal API Changes
- The project uses Microsoft.CodeAnalysis.PublicApiAnalyzers
- For any public and internal API (i.e. public and internal member) changes:
  1. Update PublicAPI.Unshipped.txt in the relevant package directory for a public API change
  2. Update InternalAPI.Unshipped.txt in the relevant package directory for an internal API change
  3. Include complete API signatures
  4. Consider backward compatibility impacts
  5. Document breaking changes clearly
  6. Update the mermaid diagrams in the Readme.md file to reflect the changes to the public API.

Example format:
```diff
// Adding new API
+Microsoft.Identity.Abstractions.ITokenAcquirer.GetTokenAsync(TokenRequestContext context) -> System.Threading.Tasks.Task<string>
+Microsoft.Identity.Abstractions.AuthenticationClientOptions.Scopes.get -> System.Collections.Generic.IEnumerable<string>

// Removing API (requires careful consideration)
-Microsoft.Identity.Abstractions.ObsoleteAuthenticationMethod() -> void
```

The analyzer enforces documentation of all public API changes in PublicAPI.Unshipped.txt and all internal API changes in InternalAPI.Unshipped.txt and will fail the build if changes are not properly reflected.

---

## Agent Configuration

```yaml
# Agent Configuration Metadata
name: "Microsoft.Identity.Abstractions Development Agent"
version: "1.0.0"
description: "AI agent configuration for Microsoft.Identity.Abstractions repository"
repository: "AzureAD/microsoft-identity-abstractions-for-dotnet"

# Supported AI Assistants
supported_assistants:
  - name: "GitHub Copilot"
    features: ["code_completion", "chat", "pull_request_review"]
  - name: "Cline"
    features: ["code_analysis", "file_operations", "automated_refactoring"]

# Development Workflow
workflow:
  planning_mode: true
  incremental_changes: true
  test_coverage_required: true
  api_analyzer_compliance: true

# Code Quality Standards
quality_standards:
  language: "C# 13"
  target_frameworks: ["netstandard2.0", "netstandard2.1", "net462", "net8.0", "net9.0"]
  nullable_reference_types: true
  code_analysis: true
  public_api_tracking: true

# Integration Points
integrations:
  build_system: "MSBuild"
  test_framework: "xUnit"
  mocking_framework: "Moq 4.14.x"
  package_analyzer: "Microsoft.CodeAnalysis.PublicApiAnalyzers"
```

---

## Migration Notes

This document replaces the previous `.clinerules` directory structure and consolidates all agent rules and development guidelines into a single, comprehensive document. The migration ensures:

1. **No Loss of Information**: All content from the original files has been preserved and organized
2. **Improved Structure**: Guidelines are now organized in logical sections with clear hierarchy
3. **Enhanced Documentation**: Added context and explanations for contributors
4. **Modern Format**: Uses markdown with YAML configuration blocks as recommended by GitHub
5. **Agent Integration**: Provides clear configuration for AI agents and automation tools

### Original Files Migrated
- `.clinerules/abstractions-guidelines.md` → Section: Microsoft.Identity.Abstractions Guidelines
- `.clinerules/ai-guidelines.md` → Section: AI Assistant Guidelines
- `.clinerules/cline-instructions.md` → Section: AI Assistant Guidelines (Cline-specific content)
- `.clinerules/csharp-guidelines.md` → Section: C# Development Standards

### For Contributors
Please refer to this document for all development guidelines, coding standards, and AI assistant interactions. The guidelines help maintain consistency and quality across the repository while enabling effective AI-assisted development workflows.