// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Options for configuring authentication in a web app, web API or daemon app.
    /// <para>
    /// This class contains configuration properties for any OAuth 2.0 identity provider.
    /// For Azure AD specific options see the derived class: <see cref="MicrosoftIdentityApplicationOptions"/>. This class
    /// and its derived class are usually used as options, that are deserialized from a configuration file like appsettings.json
    /// </para>
    /// </summary>
    /// <example>
    /// </example>
    public class IdentityApplicationOptions
    {
        /// <summary>
        /// Gets or sets the authority to use when calling the identity provider. 
        /// For AzureAD or Azure AD B2C, rather use <see cref="MicrosoftIdentityApplicationOptions.Instance"/>
        /// and <see cref="MicrosoftIdentityApplicationOptions.TenantId"/>. For Microsoft Entra External IDs, use
        /// the authority of the form <c>https://subdomain.ciamlogin.com</c>.
        /// </summary>
        /// <example>
        /// <code>
        /// IdentityApplicationOptions options = new 
        /// {
        ///  Authority = "https://subdomain.ciamlogin.com"
        /// };
        /// </code>
        /// </example>
        public virtual string? Authority { get; set; }

        /// <summary>
        /// Gets or sets the 'client_id' (application ID) as it appears in the 
        /// application registration. This is the string representation of a GUID.
        /// </summary>
        public string? ClientId { get; set; }

        /// <summary>
        /// Flag used to enable/disable logging of Personally Identifiable Information (PII).
        /// PII logs are never written to default outputs.
        /// Default is set to <c>false</c>, which ensures that your application is compliant with GDPR. You can set
        /// it to <c>true</c> for advanced debugging requiring PII.
        /// </summary>
        public bool EnablePiiLogging { get; set; }

        /// <summary>
        /// Sets query parameters for the query string in the HTTP request to the IdP. This parameter is useful
        /// if you want to send the request to a specific test slice, or a particular dc.
        /// </summary>
        public IDictionary<string, string>? ExtraQueryParameters { get; set; }

        #region Token Acquisition
        /// <summary>
        /// Description of the client credentials that the app provides to prove its identity to the IdP,
        /// See <see cref="CredentialSource"/> for the list of supported credential types.
        /// </summary>
        /// <example>
        /// <format type="text/markdown">
        /// <![CDATA[
        /// Here is an example of client credentials in the AzureAd section of the *appsetting.json*. The app will try to use
        /// workload identity federation from Managed identity (when setup and deployed in Azure), and otherwise, will use a certificate
        /// from Key Vault, and otherwise, will use a client secret.
        /// ```json
        ///  "ClientCredentials": [
        ///   {
        ///    "SourceType": "SignedAssertionFromManagedIdentity",
        ///    "ManagedIdentityClientId": "Optional GUID of user assigned Managed identity"
        ///   },
        ///   {
        ///    "SourceType": "KeyVault",
        ///    "KeyVaultUrl": "https://webappsapistests.vault.azure.net",
        ///    "KeyVaultCertificateName": "Self-Signed-5-5-22"
        ///   },
        ///   {
        ///    "SourceType": "ClientSecret",
        ///    "ClientSecret": "***"
        ///   }
        ///  ]
        /// ```
        ///   See also https://aka.ms/ms-id-web-certificates.
        /// ]]></format>
        /// </example>
        public IEnumerable<CredentialDescription>? ClientCredentials { get; set; }
        #endregion Token acquisition

        #region web API
        /// <summary>
        /// In a web API, audience of the tokens that will be accepted by the web API.
        /// <para>If your web API accepts several audiences, see <see cref="Audiences"/>.</para>
        /// </summary>
        /// <remarks>If both Audience and <see cref="Audiences"/>, are expressed, the effective audiences is the
        /// union of these properties.</remarks>
        public string? Audience { get; set; }

        /// <summary>
        /// In a web API, accepted audiences for the tokens received by the web API.
        /// <para>See also <see cref="Audience"/>.</para>
        /// The audience is the intended recipient of the token. You can usually assume that the ApplicationID of your web API
        /// is a valid audience. It can, in general be any of the App ID URIs (or resource identitfier) you defined for your application
        /// during its registration in the Azure portal.
        /// </summary>
        /// <example>
        /// <format type="text/markdown">
        /// <![CDATA[
        /// Here is an example of client credentials in the AzureAd section of the *appsetting.json*. The app will try to use
        /// workload identity federation from Managed identity (when setup and deployed in Azure), and otherwise, will use a certificate
        /// from Key Vault, and otherwise, will use a client secret.
        /// ```json
        ///  "Audiences": [
        ///    "api://a88bb933-319c-41b5-9f04-eff36d985612",
        ///    "a88bb933-319c-41b5-9f04-eff36d985612",
        ///    "https://mydomain.com/myapp"
        ///  ]
        /// ```
        ///   See also https://aka.ms/ms-id-web-certificates.
        /// ]]></format>
        /// </example>
        /// <remarks>If both Audiences and <see cref="Audience"/>, are expressed, the effective audiences is the
        /// union of these properties.</remarks>
        public IEnumerable<string>? Audiences { get; set; }

        /// <summary>
        /// Description of the credentials (usually certificates) used to decrypt an encrypted 
        /// token in a web API.
        /// </summary>
        /// <example> 
        /// <format type="text/markdown">
        /// <![CDATA[
        /// Here is how to specify a decrypt certificate read from the certificate store:
        /// 
        /// ```json
        /// "TokenDecryptionCredentials": [
        ///   {
        ///     "SourceType": "StoreWithDistinguishedName",
        ///      "CertificateStorePath": "CurrentUser/My",
        ///      "CertificateDistinguishedName": "CN=WebAppCallingWebApiCert"
        ///     }
        ///    ]
        /// ```   
        ///   See also https://aka.ms/ms-id-web-certificates.
        /// ]]></format>
        /// </example>
        public IEnumerable<CredentialDescription>? TokenDecryptionCredentials { get; set; }

        /// <summary>
        /// Web APIs called on behalf of a user can validate a token based on scopes (representing delegated permissions).
        /// Web APIs called by daemon applications can validate a token based on roles (representing app permissions).
        /// By default, the web API will validate the presence of roles and scopes. You can set this property to <c>false</c> to
        /// use the ACL-based authorization pattern for the client (daemon) to the web API. If using ACL-based authorization,
        /// the implementation will not throw if roles or scopes are not in the Claims.
        /// For details see https://aka.ms/ms-identity-web/daemon-ACL.
        /// </summary>
        /// The default is <c>false.</c>
        public bool AllowWebApiToBeAuthorizedByACL { get; set; }
        #endregion web API
    }
}
