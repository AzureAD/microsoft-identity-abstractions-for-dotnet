using Microsoft.Identity.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
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

            AcquireTokenResult result = new AcquireTokenResult(accessTokenValue, now, tenant, idToken, scopes, correlationId, tokenType);
            Assert.Equal(accessTokenValue, result.AccessToken);
            Assert.Equal(now, result.ExpiresOn);
            Assert.Equal(tenant, result.TenantId );
            Assert.Equal(idToken, result.IdToken);
            Assert.Equal(scopes, result.Scopes!);
            Assert.Equal(correlationId, result.CorrelationId);
            Assert.Equal(tokenType, result.TokenType);
        }
    }
}
