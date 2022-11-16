// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Parameters that credential loaders can use. This is for example the
    /// case for signed assertion providers.
    /// </summary>
    public class CredentialSourceLoaderParameters
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="clientId">Application ID of the confidential client application that
        /// wants to use these credentials as client credentials</param>
        /// <param name="authority">ID of the tenant in which the application is running.</param>
        public CredentialSourceLoaderParameters(string clientId, string authority)
        {
            ClientId = clientId;
            Authority = authority;
        }

        /// <summary>
        /// Application ID of the confidential client application that
        /// wants to use these credentials as client credentials.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Authority (Cloud instance and tenant).
        /// </summary>
        public string Authority { get; set; }
    }
}
