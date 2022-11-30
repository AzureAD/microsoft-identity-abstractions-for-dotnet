// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Options passed-in to call downstream web APIs.
    /// </summary>
    public class DownstreamRestApiOptions : AuthorizationHeaderProviderOptions
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public DownstreamRestApiOptions()
        {
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="other"></param>
        public DownstreamRestApiOptions(DownstreamRestApiOptions other) : base(other)
        {
            Scopes = other.Scopes;
        }

        /// <inheritdoc/>
        public new virtual AuthorizationHeaderProviderOptions Clone()
        {
            return new DownstreamRestApiOptions(this);
        }

        /// <summary>
        /// Scopes required to call the downstream web API.
        /// For instance "user.read mail.read".
        /// For Microsoft identity, in the case of application tokens (token 
        /// requested by the app on behalf of itself), there should be only one scope, and it
        /// should end in "./default")
        /// </summary>
        public IEnumerable<string>? Scopes { get; set; }
    }
}
