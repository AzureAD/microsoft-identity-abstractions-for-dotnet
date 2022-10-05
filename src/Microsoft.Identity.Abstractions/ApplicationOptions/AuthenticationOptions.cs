// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Options for configuring authentication in a web app, web API or daemon app.
    /// <para>
    /// This class contains configuration properties for any OAuth 2.0 identity provider.
    /// For Azure AD specific options see the derived class: <see cref="MicrosoftAuthenticationOptions"/>.
    /// </para>
    /// </summary>
    /// <example></example>
    public class AuthenticationOptions
    {
        /// <summary>
        /// Gets or sets the authority to use when calling the STS. 
        /// If using AzureAD, rather use <see cref="MicrosoftAuthenticationOptions.Instance"/>
        /// and <see cref="MicrosoftAuthenticationOptions.TenantId"/>
        /// </summary>
        /// <example>
        /// <code>
        /// AuthenticationOptions options = new 
        /// {
        ///  Authority = "https://login.microsoftonline.com/common/"
        /// };
        /// </code>
        /// </example>
        public virtual string? Authority { get; set; }

        /// <summary>
        /// Gets or sets the 'client_id' (application ID) as it appears in the 
        /// application registration. This is the string representation of a GUID.
        /// </summary>
        public string? ClientId
        {
            get;
            set;
        }

        /// <summary>
        /// Flag used to enable/disable logging of Personally Identifiable Information (PII).
        /// PII logs are never written to default outputs.
        /// Default is set to <c>false</c>, which ensures that your application is compliant with GDPR. You can set
        /// it to <c>true</c> for advanced debugging requiring PII.
        /// </summary>
        public bool EnablePiiLogging { get; set; }

        #region Token Acquisition
        /// <summary>
        /// Does the app provide client credentials.
        /// </summary>
        public bool HasClientCredentials
        {
            get => (ClientCredentials != null && ClientCredentials.Any());
        }

        /// <summary>
        /// Description of the client credentials provided to prove the identity of the web app,
        /// web API, or daemon app.
        /// </summary>
        /// <example> An example in the appsetting.json:
        /// <code>
        /// "ClientCredentials": [
        ///   {
        ///     "SourceType": "StoreWithDistinguishedName",
        ///      "CertificateStorePath": "CurrentUser/My",
        ///      "CertificateDistinguishedName": "CN=WebAppCallingWebApiCert"
        ///     }
        ///    ]
        ///   </code>
        ///   See also https://aka.ms/ms-id-web-certificates.
        ///   </example>
        public IEnumerable<CredentialDescription>? ClientCredentials { get; set; }
        #endregion Token acquisition

        #region web API
        /// <summary>
        /// In a web API, audience of the tokens that will be accepted by the web API.
        /// <para>If your web API accepts several audiences, see <see cref="Audiences"/>.</para>
        /// </summary>
        public string? Audience { get; set; }

        /// <summary>
        /// In a web API, accepted audiences for the tokens received by the web API.
        /// <para>See also <see cref="Audience"/>.</para>
        /// </summary>
        public IEnumerable<string>? Audiences { get; set; }

        /// <summary>
        /// Description of the credentials (usually certificates) used to decrypt an encrypted 
        /// token in a web API.
        /// </summary>
        /// <example> An example in the appsetting.json:
        /// <code>
        /// "TokenDecryptionCredentials": [
        ///   {
        ///     "SourceType": "StoreWithDistinguishedName",
        ///      "CertificateStorePath": "CurrentUser/My",
        ///      "CertificateDistinguishedName": "CN=WebAppCallingWebApiCert"
        ///     }
        ///    ]
        ///   </code>
        ///   See also https://aka.ms/ms-id-web-certificates.
        ///   </example>
        public IEnumerable<CredentialDescription>? TokenDecryptionCredentials { get; set; }

        /// <summary>
        /// Web APIs called by daemon applications can validate a token based on roles (representing app permissions), 
        /// or using the ACL-based authorization pattern for the client (daemon) to the web API. If using ACL-based authorization,
        /// the implementation will not throw if roles or scopes are not in the Claims.
        /// For details see https://aka.ms/ms-identity-web/daemon-ACL.
        /// </summary>
        /// The default is <c>false.</c>
        public bool AllowWebApiToBeAuthorizedByACL { get; set; }
        #endregion web API
    }
}
