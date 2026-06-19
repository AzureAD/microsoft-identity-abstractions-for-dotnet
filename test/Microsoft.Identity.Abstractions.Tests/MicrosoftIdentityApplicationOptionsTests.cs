// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Microsoft.Identity.Abstractions.ApplicationOptions.Tests
{
    public class MicrosoftIdentityApplicationOptionsTests
    {
        private const string instance = "https://login.microsoftonline.com/";
        private const string clientId = "application-id-from-app-registration";
        private const string tenant = "common";
        private const string audience = clientId;
        private const string azureRegion = "westus";
        private string[] clientCapabilities = new[] { "cp1" };
        private const string domain = "mydomain.com";
        private CredentialDescription secret = new() { SourceType = CredentialSource.ClientSecret, ClientSecret = "blah" };
        private CredentialDescription decryptCert = new CredentialDescription { SourceType = CredentialSource.Base64Encoded, Base64EncodedValue = "0123" };
        private string[] audiences = new[] { "https://myapi", clientId };
        private const string appHomeTenantId = "this-is-a-tenant-guid";
        private const string name = "OptionsName";
        [Fact]
        public void MicrosoftIdentityApplicationOptionsProperties()
        {
            MicrosoftIdentityApplicationOptions microsoftIdentityApplicationOptions = new()
            {
                Name = name,
                Instance = instance,
                TenantId = tenant,
                AppHomeTenantId = appHomeTenantId,
                ClientId = clientId,
                Audience = audience,
                AzureRegion = azureRegion,
                ClientCapabilities = clientCapabilities,
                Domain = domain,
                SendX5C = true,
                WithSpaAuthCode = true,
                EnablePiiLogging = true,
                ExtraQueryParameters = new Dictionary<string, string> { { "slice", "test" } },
                ClientCredentials = new[]
                {
                    secret
                },
                AllowWebApiToBeAuthorizedByACL = true,
                TokenDecryptionCredentials = new[]
                {
                    decryptCert
                },
                EditProfilePolicyId = "EditProfilePolicyId",
                SignUpSignInPolicyId = "SignUpSignInPolicyId",
                ResetPasswordPolicyId = "ResetPasswordPolicyId",
                ResetPasswordPath = "ResetPasswordPath",
                ErrorPath = "ErrorPath",
            };

            Assert.Equal("https://login.microsoftonline.com/common/v2.0", microsoftIdentityApplicationOptions.Authority);
            
            microsoftIdentityApplicationOptions.Authority = "https://login.microsoftonline.com/common";

            Assert.Equal(name, microsoftIdentityApplicationOptions.Name);
            Assert.Equal("https://login.microsoftonline.com/common", microsoftIdentityApplicationOptions.Authority);
            Assert.Equal(clientId, microsoftIdentityApplicationOptions.ClientId);
            Assert.Equal(tenant, microsoftIdentityApplicationOptions.TenantId);
            Assert.Equal(appHomeTenantId, microsoftIdentityApplicationOptions.AppHomeTenantId);
            Assert.Equal(clientId, microsoftIdentityApplicationOptions.Audience);
            Assert.Equal(clientCapabilities, microsoftIdentityApplicationOptions.ClientCapabilities);
            Assert.Equal(azureRegion, microsoftIdentityApplicationOptions.AzureRegion);
            Assert.True(microsoftIdentityApplicationOptions.SendX5C);
            Assert.True(microsoftIdentityApplicationOptions.AllowWebApiToBeAuthorizedByACL);
            Assert.True(microsoftIdentityApplicationOptions.WithSpaAuthCode);
            Assert.True(microsoftIdentityApplicationOptions.EnablePiiLogging);
            Assert.Equal(secret, microsoftIdentityApplicationOptions.ClientCredentials.First());
            Assert.Equal(decryptCert, microsoftIdentityApplicationOptions.TokenDecryptionCredentials.First());
            Assert.Equal(domain, microsoftIdentityApplicationOptions.Domain);
            Assert.Equal(nameof(microsoftIdentityApplicationOptions.EditProfilePolicyId), microsoftIdentityApplicationOptions.EditProfilePolicyId);
            Assert.Equal(nameof(microsoftIdentityApplicationOptions.SignUpSignInPolicyId), microsoftIdentityApplicationOptions.SignUpSignInPolicyId);
            Assert.Equal(nameof(microsoftIdentityApplicationOptions.ResetPasswordPolicyId), microsoftIdentityApplicationOptions.ResetPasswordPolicyId);
            Assert.Equal(nameof(microsoftIdentityApplicationOptions.ResetPasswordPath), microsoftIdentityApplicationOptions.ResetPasswordPath);
            Assert.Equal(nameof(microsoftIdentityApplicationOptions.ErrorPath), microsoftIdentityApplicationOptions.ErrorPath);
            Assert.Equal(nameof(microsoftIdentityApplicationOptions.SignUpSignInPolicyId), microsoftIdentityApplicationOptions.DefaultUserFlow);
            Assert.NotEmpty(microsoftIdentityApplicationOptions.ExtraQueryParameters);
        }


        [Fact]
        public void IdentityApplicationOptionsProperties()
        {
            IdentityApplicationOptions identityApplicationOptions = new()
            {
                Authority = "https://google.com/",
                Audiences = audiences
            };

            Assert.Equal("https://google.com/", identityApplicationOptions.Authority);
            Assert.Equal(audiences, identityApplicationOptions.Audiences);
        }

        [Theory]
        [InlineData(null, null, null, "//v2.0")]
        [InlineData("Instance", "Tenant", null, "Instance/Tenant/v2.0")]
        [InlineData("Instance/", "Tenant", null, "Instance/Tenant/v2.0")]
        [InlineData("Instance/", "Tenant", "Authority", "Authority")]
        public void AuthorityDefaultValues(string? instance, string? tenant, string? authorityIn, string authorityOut)
        {
            MicrosoftIdentityApplicationOptions identityApplicationOptions = new()
            {
                Instance = instance,
                TenantId = tenant,
            };
            if (authorityIn != null)
            {
                identityApplicationOptions.Authority = authorityIn;
            }
            
            Assert.Equal(authorityOut, identityApplicationOptions.Authority);

        }
    }
}
