// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Represents comprehensive information about an authorization header,
    /// including the header value, mTLS certificate, and additional headers.
    /// </summary>
    public class AuthorizationHeaderInformation
    {
        /// <summary>
        /// Gets or sets the authorization header value.
        /// For example: "Bearer token", "PoP token", etc.
        /// </summary>
        public string? AuthorizationHeaderValue { get; set; }

        /// <summary>
        /// Gets or sets the mTLS (mutual TLS) certificate for client authentication.
        /// This certificate is used for mutual TLS authentication scenarios.
        /// </summary>
        public X509Certificate2? MTlsCertificate { get; set; }

        /// <summary>
        /// Gets or sets additional headers that should be included with the request.
        /// This dictionary contains key-value pairs of additional HTTP headers
        /// that complement the authorization information.
        /// </summary>
        public IDictionary<string, string>? AdditionalHeaders { get; set; }
    }
}