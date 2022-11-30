﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Net.Http;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Options passed-in to call downstream web APIs. To call Microsoft Graph, see rather
    /// <c>MicrosoftGraphOptions</c> in the <c>Microsoft.Identity.Web.MicrosoftGraph</c> assembly.
    /// </summary>
    public class AuthorizationHeaderProviderOptions
    {   
        /// <summary>
        /// Default constructor.
        /// </summary>
        public AuthorizationHeaderProviderOptions()
        {
            TokenAcquirerOptions = new AcquireTokenOptions();
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="other">Options to copy from.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="other"/> is <c>null</c>.</exception>
        public AuthorizationHeaderProviderOptions(AuthorizationHeaderProviderOptions other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            BaseUrl = other.BaseUrl;
            RelativePath = other.RelativePath;
            TokenAcquirerOptions = other.TokenAcquirerOptions.Clone();
            HttpMethod = other.HttpMethod;
            CustomizeHttpRequestMessage = other.CustomizeHttpRequestMessage;
            ProtocolScheme = other.ProtocolScheme;
        }

        /// <summary>
        /// Base URL for the called downstream web API. For instance <c>"https://graph.microsoft.com/beta/"</c>.
        /// </summary>
        public string? BaseUrl { get; set; }

        /// <summary>
        /// Path relative to the <see cref="BaseUrl"/> (for instance "me").
        /// </summary>
        public string RelativePath { get; set; } = string.Empty;

        /// <summary>
        /// HTTP method used to call this downstream web API (by default Get).
        /// </summary>
        public HttpMethod HttpMethod { get; set; } = HttpMethod.Get;

        /// <summary>
        /// Provides an opportunity for the caller app to customize the HttpRequestMessage. For example,
        /// to customize the headers. This is called after the message was formed, including
        /// the Authorization header, and just before the message is sent.
        /// </summary>
        public Action<HttpRequestMessage>? CustomizeHttpRequestMessage { get; set; }

        /// <summary>
        /// Options related to the token acquisition.
        /// </summary>
        public AcquireTokenOptions TokenAcquirerOptions { get; set; } = new AcquireTokenOptions();

        /// <summary>
        /// Name of the protocol scheme used to create the authorization header.
        /// By default "Bearer"
        /// </summary>
        public string ProtocolScheme { set; get; } = "Bearer";

        /// <summary>
        /// Clone the options (to be able to override them).
        /// </summary>
        /// <returns>A clone of the options.</returns>
        public virtual AuthorizationHeaderProviderOptions Clone()
        {
            return new AuthorizationHeaderProviderOptions(this);
        }

        /// <summary>
        /// Return the downstream web API URL.
        /// </summary>
        /// <returns>URL of the downstream web API.</returns>
#pragma warning disable CA1055 // Uri return values should not be strings
        public string GetApiUrl()
#pragma warning restore CA1055 // Uri return values should not be strings
        {
            return BaseUrl?.TrimEnd('/') + $"/{RelativePath}";
        }
    }
}
