// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Creates the value of an authorization header that the caller can use to call a protected web API with unbound or
    /// bound to certificate (e.g., in case of mTLS PoP scenario) token.
    /// </summary>
    public interface IAuthorizationHeaderBoundProvider : IAuthorizationHeaderProvider<OperationResult<AuthorizationHeaderInformation, AuthorizationHeaderError>>
    {
    }
}
