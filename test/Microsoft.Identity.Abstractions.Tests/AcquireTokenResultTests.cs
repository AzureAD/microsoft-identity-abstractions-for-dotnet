// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Security.Cryptography.X509Certificates;
using Xunit;

namespace Microsoft.Identity.Abstractions.TokenAcquisition.Tests
{
    public class AcquireTokenResultTests
    {
        [Fact]
        public void TestAcquireTokenResult()
        {
            const string accessTokenValue = "AccessToken";
            DateTimeOffset now = DateTimeOffset.UtcNow;
            const string tenant = "common";
            const string idToken = "idToken";
            string[] scopes = new[] { "user.read", "mail.read" };
            Guid correlationId = Guid.NewGuid();
            const string tokenType = "bearer";

            AcquireTokenResult result = new(accessTokenValue, now, tenant, idToken, scopes, correlationId, tokenType);
            Assert.Equal(accessTokenValue, result.AccessToken);
            Assert.Equal(now, result.ExpiresOn);
            Assert.Equal(tenant, result.TenantId );
            Assert.Equal(idToken, result.IdToken);
            Assert.Equal(scopes, result.Scopes!);
            Assert.Equal(correlationId, result.CorrelationId);
            Assert.Equal(tokenType, result.TokenType);

            Assert.Null(result.BindingCertificate);
            Assert.Null(result.AdditionalResponseParameters);

            result.AdditionalResponseParameters = new System.Collections.Generic.Dictionary<string, string>
            {
                { "param1", "value1" },
                { "param2", "value2" }
            };
        }
    }
}
