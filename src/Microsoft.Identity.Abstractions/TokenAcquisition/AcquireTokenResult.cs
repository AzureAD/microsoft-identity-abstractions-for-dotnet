// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Result of a token acquisition.
    /// </summary>
    public class AcquireTokenResult
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="accessToken">Access token.</param>
        /// <param name="expiresOn">Expiration date/time.</param>
        /// <param name="tenantId">Tenant for which the token was acquired.</param>
        /// <param name="idToken">ID Token, in the case of a token for a user.</param>
        /// <param name="scopes">Scopes granted by the IdP.</param>
        /// <param name="correlationId">Correlation ID of the token acquisition.</param>
        /// <param name="tokenType">Token type of the access token (Bearer or Pop).</param>
        public AcquireTokenResult(
            string accessToken,
            DateTimeOffset expiresOn,
            string tenantId,
            string idToken,
            IEnumerable<string> scopes,
            Guid correlationId,
            string tokenType)
        {
            AccessToken = accessToken;
            ExpiresOn = expiresOn;
            TenantId = tenantId;
            IdToken = idToken;
            Scopes = scopes;
            CorrelationId = correlationId;
            TokenType = tokenType;
        }

        /// <summary>
        /// Access Token that can be used to build an authorization header 
        /// to access protected web APIs. 
        /// </summary>
        /// <seealso cref="IAuthorizationHeaderProvider"/> which creates the authorization
        /// header directly, whatever the protocol.
        public string? AccessToken { get; set; }

        /// <summary>
        /// Gets the point in time in which the Access Token returned in the <see cref="AccessToken"/>
        /// property ceases to be valid. This value is calculated based on current UTC time
        /// measured locally and the value expiresIn received from the service.
        /// </summary>
        public DateTimeOffset ExpiresOn { get; set; }

        /// <summary>
        ///  (Microsoft Identity specific) In the case of Azure AD, Azure AD B2C, or Microsoft Entra External IDs, 
        ///  gets an identifier for the tenant for which the token was acquired.
        ///  This property will be null if tenant information is not returned by the service or the service
        ///  is not Azure AD.
        /// </summary>
        public string? TenantId { get; set; }

        /// <summary>
        /// Gets the Id Token if returned by the service or null if no Id Token is returned.
        /// </summary>
        public string? IdToken { get; set; }

        /// <summary>
        /// Gets the scope values effectively granted by the IdP. They can be different
        /// form the scopes requested.
        /// </summary>
        public IEnumerable<string>? Scopes { get; set; }

        /// <summary>
        /// Gets the correlation id used for the request. If provided in <see cref="AcquireTokenOptions.CorrelationId"/>, this
        /// will be the same, otherwise a new one will be created by the token acquirer.
        /// </summary>
        public Guid CorrelationId { get; set; }

        /// <summary>
        /// Gets the protocol (Bearer or Pop).
        /// </summary>
        public string? TokenType { get; set; }
    }
}
