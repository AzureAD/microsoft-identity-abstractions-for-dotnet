﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Interface to implement to load credentials, for instance certificates.
    /// </summary>
    public interface ICredentialSourceLoader
    {
        /// <summary>
        /// Load the credential from the description, if needed.
        /// </summary>
        /// <param name="credentialDescription">Description of the credential.</param>
        Task LoadIfNeededAsync(CredentialDescription credentialDescription);

        /// <summary>
        /// Loadable CredentialSource.
        /// </summary>
        CredentialSource CredentialSource { get; }
    }
}
