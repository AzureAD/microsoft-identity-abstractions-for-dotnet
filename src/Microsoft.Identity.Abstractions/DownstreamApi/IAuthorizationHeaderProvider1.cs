// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Creates an authorization header value that the caller can use to access a protected web API, which supports either unbound or
    /// bound to a certificate (for example, in an mTLS PoP scenario) tokens.
    /// </summary>
    public interface IAuthorizationHeaderProvider1 : IAuthorizationHeaderProvider<OperationResult<AuthorizationHeaderInformation, AuthorizationHeaderError>>
    {
    }
}
