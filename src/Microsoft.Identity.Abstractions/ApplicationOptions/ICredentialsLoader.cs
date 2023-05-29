// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Contract for credential loaders, implemented by classes like the DefaultCertificateLoader or the DefaultCredentialLoader 
    /// in Microsoft.Identity.Web. Credential loaders are used to load credentials from a <see cref="CredentialDescription"/>, the result
    /// is then in the <see cref="CredentialDescription.CachedValue"/> property.
    /// Credential loaders constitute an extensibility point. They delegate to credential source loaders, which are specified in the <see cref="CredentialSourceLoaders"/>
    /// collection, choosing the one which <see cref="ICredentialSourceLoader.CredentialSource"/> matches the credential source of the
    /// credential description to load.
    /// </summary>
    public interface ICredentialsLoader
    {
        /// <summary>
        /// Dictionary of credential source loaders per credential source. Your application can add more to 
        /// process additional credential sources.
        /// </summary>
        IDictionary<CredentialSource, ICredentialSourceLoader> CredentialSourceLoaders { get; }

        /// <summary>
        /// Load a given credential description, if needed. This method will leverage the <see cref="CredentialSourceLoaders"/> to
        /// load the credentials from the description.
        /// </summary>
        /// <param name="credentialDescription">Description of the credentials to load.</param>
        /// <param name="parameters">Parameters, related to the host application, that the credential source loader can use.</param>
        Task LoadCredentialsIfNeededAsync(CredentialDescription credentialDescription, CredentialSourceLoaderParameters? parameters = null);

        /// <summary>
        /// Load the first valid credential from the credentials description list. This is useful when you have multiple deployments
        /// (for instance on your developer machine, you can use a certificate from KeyVault, and when deployed in AKS, you use
        /// workload identity federation for AKS. You can express the list of credentials in the appsettings.json file, and this method will
        /// load the most appropriate based on the order.
        /// </summary>
        /// <param name="credentialDescriptions">Description of the credentials.</param>
        /// <param name="parameters">Parameters, related to the host application, that the credential source loader can use.</param>
        /// <returns>First valid credential description that could be loaded from the credential description list.
        /// <c>null</c> if none is valid. Otherwise the first <see cref="CredentialDescription"/> that could be loaded.</returns>
        Task<CredentialDescription?> LoadFirstValidCredentialsAsync(IEnumerable<CredentialDescription> credentialDescriptions, CredentialSourceLoaderParameters? parameters = null);

        /// <summary>
        /// Resets resettable credentials in the credential description list (for instance reset the 
        /// certificates so that they can be re-loaded again)
        /// Use, for example, before a retry.
        /// </summary>
        /// <param name="credentialDescriptions">Description of the credentials.</param>
        /// <remarks>This method is, for instance, used, in Microsoft.Identity.Web to automatically reload the certificates from
        /// KeyVault, when the certificate was rotated in Azure AD.</remarks>
        void ResetCredentials(IEnumerable<CredentialDescription> credentialDescriptions);
    }
}
