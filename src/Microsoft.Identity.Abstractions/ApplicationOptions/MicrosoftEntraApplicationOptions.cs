// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Options for configuring authentication specific to Microsoft Entra (Azure AD) for a web app, web API, or daemon application.
    /// </summary>
    public class MicrosoftEntraApplicationOptions : IdentityApplicationOptions
    {
        private string? _authority;

        /// <summary>
        /// Gets or sets the Azure Active Directory instance, e.g. <c>"https://login.microsoftonline.com/"</c>.
        /// </summary>
        public string? Instance { get; set; }

        /// <summary>
        /// Gets or sets the tenant ID. If your application is multi-tenant, you can also use "common" if it supports
        /// both work and school, or personal accounts accounts, or "organizations" if your application supports only work 
        /// and school accounts. If your application is single tenant, set this property to the tenant ID or domain name.
        /// If your application works only for Microsoft personal accounts, use "consumers".
        /// </summary>
        public string? TenantId { get; set; }

        /// <summary>
        /// Gets or sets the Authority to use when making OpenIdConnect calls. By default the authority is computed
        /// from the <see cref="Instance"/> and <see cref="TenantId"/> properties, by concatenating them, and appending "v2.0".
        /// If your authority is not an Azure AD authority, you can set it directly here.
        /// </summary>
        public override string? Authority
        {
            get { return _authority ?? $"{Instance?.TrimEnd('/')}/{TenantId}/v2.0"; }
            set { _authority = value; }
        }

        #region Token acquisition
        /// <summary>
        /// Home tenant of the app in which the app can acquire a token to call a downstream API on behalf of itself.
        /// </summary>
        public string? AppHomeTenantId { get; set; }

        /// <summary>
        /// Specifies the Azure region. See https://aka.ms/azure-region. To have
        /// the app attempt to detect the Azure region automatically,
        /// use "TryAutoDetect".
        /// </summary>
        public string? AzureRegion { get; set; }

        /// <summary>
        /// Specifies the capabilities of the client (for instance {"cp1", "cp2"}). This is
        /// useful to express that the Client is capable of handling claims challenge. If your application
        /// is CAE capable, it needs to express "cp1".
        /// </summary>
        public IEnumerable<string>? ClientCapabilities { get; set; }

        /// <summary>
        /// Specifies if the x5c claim (public key of the certificate) should be sent to the STS.
        /// Sending the x5c enables application developers to achieve easy certificate rollover in Azure AD:
        /// this method will send the public certificate to Azure AD along with the token request,
        /// so that Azure AD can use it to validate the subject name based on a trusted issuer policy.
        /// This saves the application admin from the need to explicitly manage the certificate rollover
        /// (either via the app registration portal or using PowerShell/CLI). 
        /// For details see https://aka.ms/msal-net-sni.
        /// The default is <c>false</c>.
        /// </summary>
        public bool SendX5C { get; set; }
        #endregion Token Acquisition
    }
}
