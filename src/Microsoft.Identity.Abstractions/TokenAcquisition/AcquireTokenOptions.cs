// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Options directing the token acquisition. These options are provided to the <see cref="ITokenAcquirer"/> methods, or
    /// part of the <see cref="AuthorizationHeaderProviderOptions"/>, or <see cref="DownstreamApiOptions"/>.
    /// </summary>
    public class AcquireTokenOptions
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public AcquireTokenOptions()
        {
        }
        
        /// <summary>
        /// Copy constructor for <see cref="AcquireTokenOptions"/>
        /// </summary>
        public AcquireTokenOptions(AcquireTokenOptions other)
        {
            _ = other ?? throw new ArgumentNullException(nameof(other));

            AuthenticationOptionsName = other.AuthenticationOptionsName;
            CorrelationId = other.CorrelationId;
            ExtraQueryParameters = other.ExtraQueryParameters;
            ExtraHeadersParameters = other.ExtraHeadersParameters;
            ForceRefresh = other.ForceRefresh;
            Claims = other.Claims;
            PopPublicKey = other.PopPublicKey;
            PopClaim = other.PopClaim;
            ManagedIdentity = other.ManagedIdentity?.Clone();
            LongRunningWebApiSessionKey = other.LongRunningWebApiSessionKey;
            Tenant = other.Tenant;
            UserFlow = other.UserFlow;
        }

        /// <summary>
        /// Gets the name of the options describing the confidential client application (ClientID,
        /// Region, Authority, client credentials). In ASP.NET Core, the authentication options name
        /// is the same as the authentication scheme.
        /// </summary>
        public string? AuthenticationOptionsName { get; set; }

        /// <summary>
        /// Sets the correlation ID to be used in the request to the STS "/token" endpoint.
        /// </summary>
        public Guid? CorrelationId { get; set; }

        /// <summary>
        /// Sets query parameters for the query string in the HTTP request to the 
        /// "/token" endpoint.
        /// </summary>
        public IDictionary<string, string>? ExtraQueryParameters { get; set; }

        /// Sets extra headers in the HTTP request to the STS "/token" endpoint.
        public IDictionary<string, string>? ExtraHeadersParameters { get; set; }

        /// <summary>
        /// A string with one or multiple claims to request. It's a json blob (encoded or not)
        /// Normally used with Conditional Access. It receives the Claims member of the UiRequiredException.
        /// It can also be used to request specific optional claims, and for 
        /// <see href="https://learn.microsoft.com/en-us/azure/active-directory/conditional-access/concept-conditional-access-cloud-apps">
        /// CA Auth context</see>
        /// </summary>
        public string? Claims { get; set; }

        /// <summary>
        /// Specifies if the token request will ignore the access token in the token cache
        /// and will attempt to acquire a new access token.
        /// If <c>true</c>, the request will ignore the token cache. The default is <c>false</c>.
        /// Use this option with care and only when needed, for instance, if you know that conditional access policies have changed,
        /// for it induces performance degradation, as the token cache is not utilized, and the STS might throttle the app.
        /// </summary>
        public bool ForceRefresh { get; set; }

        /// <summary>
        /// Modifies the token acquisition request so that the acquired token is a Proof of Possession token (PoP),
        /// rather than a Bearer token.
        /// PoP tokens are similar to Bearer tokens, but are bound to the HTTP request and to a cryptographic key,
        /// which MSAL can manage. See https://aka.ms/msal-net-pop.
        /// </summary>
        public string? PopPublicKey { get; set; }

        /// <summary>
        /// In addition to the <see cref="PopPublicKey"/>, specify the PopClaim when needed in specific POP protocols. 
        /// </summary>
        public string? PopClaim { get; set; }

        /// <summary>
        /// When <see cref="ManagedIdentity"/> is set, the application uses a managed identity instead of client credentials to
        /// acquire an app token.
        /// To use a system-assigned identity, simply leave <see cref="ManagedIdentityOptions.UserAssignedClientId"/> null.
        /// To use a user-assigned identity, set <see cref="ManagedIdentityOptions.UserAssignedClientId"/> to the ClientID of the
        /// user-assigned identity you want to use. Using either form of managed identity requires the application to be deployed
        /// on Azure and the managed identity to be configured. For more details, check the
        /// <see href="https://aka.ms/Entra/ManagedIdentityOverview"> managed identities for Azure documentation</see>.
        /// </summary>
        /// <example>
        /// <format type="text/markdown">
        /// <![CDATA[
        /// The Json fragment below describes how to use a system-assigned Managed Identity for authentication in a confidential client application :
        /// :::code language="json" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/AquireTokenOptionsTests.cs" id="managedidentitysystem_json":::
        /// 
        /// The code below describes the same, programmatically in C#.
        /// :::code language="csharp" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/AquireTokenOptionsTests.cs" id="managedidentitysystem_csharp":::
        /// 
        /// The Json fragment below describes how to use a user-assigned Managed Identity for authentication in a confidential client application :
        /// :::code language="json" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/AquireTokenOptionsTests.cs" id="managedidentityuser_json":::
        /// 
        /// The code below describes the same, programmatically in C#.
        /// :::code language="csharp" source="~/../abstractions-samples/test/Microsoft.Identity.Abstractions.Tests/AquireTokenOptionsTests.cs" id="managedidentityuser_csharp":::
        /// ]]></format>
        /// </example>
        public ManagedIdentityOptions? ManagedIdentity { get; set; }

        /// <summary>
        /// Key used for long running web APIs that need to call downstream web
        /// APIs on behalf of the user. Can be null, if you are not developing a long
        /// running web API, <see cref="LongRunningWebApiSessionKeyAuto"/> if you want
        /// the token acquirer to allocate a session key for you, or your own string
        /// if you want to associate the session with some information you have externally
        /// (for instance a Microsoft Graph hook identifier).
        /// </summary>
        public string? LongRunningWebApiSessionKey { get; set; }

        /// <summary>
        /// Value that can be used for <see cref="LongRunningWebApiSessionKey"/> so that
        /// the token acquirer allocates the long running web api session key for the developer.
        /// </summary>
        public static string LongRunningWebApiSessionKeyAuto { get; } = "AllocateForMe";

        /// <summary>
        /// (Microsoft identity specific)
        /// Enables to override the tenant/account for which to get a token. 
        /// This is useful in multi-tenant apps in the cases where a given user account is a guest 
        /// in other tenants, and you want to acquire tokens for a specific tenant.
        /// </summary>
        /// <remarks>Can be the tenant ID or domain name.</remarks>
        public string? Tenant { get; set; }

        /// <summary>
        /// (Microsoft identity specific)
        /// In the case of AzureAD B2C, uses a particular user flow.
        /// </summary>
        public string? UserFlow { get; set; }

        /// <summary>
        /// Performs a shallow Clone the options (to be able to override them).
        /// </summary>
        /// <returns>A shallow Clone of the options.</returns>
        public virtual AcquireTokenOptions Clone()
        {
            return new AcquireTokenOptions(this);
        }
    }
}
