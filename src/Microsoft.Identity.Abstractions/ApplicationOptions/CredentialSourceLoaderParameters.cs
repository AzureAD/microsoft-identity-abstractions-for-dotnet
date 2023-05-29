// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Parameters that credential loaders, like signed assertion providers, can use to load credentials. You won't use this class directly
    /// unless you are writing a custom credential loader.
    /// </summary>
    /// <seealso cref="ICredentialsLoader.LoadCredentialsIfNeededAsync(CredentialDescription, CredentialSourceLoaderParameters?)"/>
    /// <seealso cref="ICredentialsLoader.LoadFirstValidCredentialsAsync(System.Collections.Generic.IEnumerable{CredentialDescription}, CredentialSourceLoaderParameters?)"></seealso>
    public class CredentialSourceLoaderParameters
    {
        /// <summary>
        /// Initialize the CredentialSourceLoaderParameters from the application ID and authority.
        /// </summary>
        /// <param name="clientId">Application ID of the confidential client application that
        /// wants to use these credentials as client credentials</param>
        /// <param name="authority">Authority (Cloud instance and tenant) to which the credential will be presented.</param>
        public CredentialSourceLoaderParameters(string clientId, string authority)
        {
            ClientId = clientId;
            Authority = authority;
        }

        /// <summary>
        /// Application ID of the confidential client application that wants to present the client credentials to the authority.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Authority (Cloud instance and tenant) to which the credential will be presented.
        /// </summary>
        public string Authority { get; set; }
    }
}
