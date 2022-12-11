// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Crendential type
    /// </summary>
    public enum CredentialType
    {   
        /// <summary>
        /// Certificate.
        /// </summary>
        Certificate = 0,

        /// <summary>
        /// (Client) secret.
        /// </summary>
        Secret = 1,
        
        /// <summary>
        /// Signed assertion.
        /// </summary>
        SignedAssertion = 2
    }
}
