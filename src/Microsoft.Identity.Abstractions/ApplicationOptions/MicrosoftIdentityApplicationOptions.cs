// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Options for configuring authentication for a web app, web API, or daemon application, using Azure Active Directory. 
    /// It has both AAD and B2C configuration attributes.
    /// </summary>
    public class MicrosoftIdentityApplicationOptions : MicrosoftEntraApplicationOptions
    {
        /// <summary>
        /// If set to <c>true</c>, when the user signs-in in a web app, the application requests an auth code 
        /// for the frontend (single page application using MSAL.js for instance). This will allow the front end
        /// JavaScript code to bypass going to the authoriize endpoint (which requires reloading the page), by 
        /// directly redeeming the auth code to get access tokens to call APIs.
        /// See https://aka.ms/msal-net/hybrid-spa-sample for details. Only works for AAD, not B2C.
        /// </summary>
        /// The default is <c>false.</c>
        public bool WithSpaAuthCode { get; set; }

        #region AADB2C
        /// <summary>
        /// Gets or sets the domain of the Azure Active Directory tenant, e.g. contoso.onmicrosoft.com.
        /// </summary>
        public string? Domain { get; set; }

        /// <summary>
        /// Gets or sets the edit profile user flow name for B2C, e.g. b2c_1_edit_profile.
        /// </summary>
        public string? EditProfilePolicyId { get; set; }

        /// <summary>
        /// Gets or sets the sign up or sign in user flow name for B2C, e.g. b2c_1_susi.
        /// </summary>
        public string? SignUpSignInPolicyId { get; set; }

        /// <summary>
        /// Gets or sets the reset password user flow name for B2C, e.g. B2C_1_password_reset.
        /// </summary>
        public string? ResetPasswordPolicyId { get; set; }

        /// <summary>
        /// Gets the default user flow (which is signUpsignIn).
        /// </summary>
        public string? DefaultUserFlow => SignUpSignInPolicyId;
        #endregion AADB2C

        #region web app
        /// <summary>
        /// Sets the ResetPassword route path (from the root of the web site).
        /// Defaults to /MicrosoftIdentity/Account/ResetPassword,
        /// which is the value used by Microsoft.Identity.Web.UI.
        /// If you override it, you need to provide your own controller/actions.
        /// </summary>
        public string? ResetPasswordPath { get; set; } = "/MicrosoftIdentity/Account/ResetPassword";

        /// <summary>
        /// Sets the Error route path.
        /// Defaults to the value /MicrosoftIdentity/Account/Error,
        /// which is the value used by Microsoft.Identity.Web.UI.
        /// </summary>
        public string? ErrorPath { get; set; } = "/MicrosoftIdentity/Account/Error";
        #endregion web app
    }
}
