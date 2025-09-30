// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Represents comprehensive information about an authorization header,
    /// including the header value and binding certificate.
    /// </summary>
    public class AuthorizationHeaderInformation
    {
        /// <summary>
        /// Gets or sets the authorization header value.
        /// For example: "Bearer token", "PoP token", etc.
        /// </summary>
        public string? AuthorizationHeaderValue { get; set; }

        /// <summary>
        /// Gets or sets the binding certificate for client authentication.
        /// This certificate is used to bind the authorization header to the client,
        /// commonly used in mutual TLS (mTLS) authentication scenarios and Proof-of-Possession (PoP) protocols.
        /// </summary>
        public X509Certificate2? BindingCertificate { get; set; }
    }
}
}