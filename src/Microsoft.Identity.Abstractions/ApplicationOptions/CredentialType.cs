// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Crendential type. This is used to specify the type of credential that is being loaded. This value
    /// is provided in the see <see cref="CredentialDescription.CredentialType"/> based on the type of
    /// credential (which depends on the type of <see cref="CredentialSource"/> value of the <see cref="CredentialDescription.SourceType"/>).
    /// </summary>
    public enum CredentialType
    {   
        /// <summary>
        /// Certificate. Certificates can be used both as <see cref="IdentityApplicationOptions.ClientCredentials"/>
        /// or <see cref="IdentityApplicationOptions.TokenDecryptionCredentials"/>.
        /// </summary>
        Certificate = 0,

        /// <summary>
        /// (Client) secret. It can only be used in <see cref="IdentityApplicationOptions.ClientCredentials"/>.
        /// </summary>
        Secret = 1,
        
        /// <summary>
        /// Signed assertion. It can only be used in <see cref="IdentityApplicationOptions.ClientCredentials"/>.
        /// </summary>
        SignedAssertion = 2,

        /// <summary>
        /// Decrypt keys. It can only be used in <see cref="IdentityApplicationOptions.TokenDecryptionCredentials"/>.
        /// </summary>
        DecryptKeys = 3
    }
}
