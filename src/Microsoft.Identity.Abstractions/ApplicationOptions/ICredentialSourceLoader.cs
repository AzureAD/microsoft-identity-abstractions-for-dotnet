// Copyright (c) Microsoft Corporation. All rights reserved.
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
        /// <param name="throwExceptions">Specifies if exceptions should be thrown when
        /// the credentials cannot be loaded (by default <c>false</c>).</param>
        Task LoadIfNeededAsync(CredentialDescription credentialDescription, bool throwExceptions=false);

        /// <summary>
        /// Loadable CredentialSource.
        /// </summary>
        CredentialSource CredentialSource { get; }
    }
}
