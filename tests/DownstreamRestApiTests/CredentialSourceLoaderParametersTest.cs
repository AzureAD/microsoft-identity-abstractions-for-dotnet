using Microsoft.Identity.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class CredentialSourceLoaderParametersTest
    {
        const string clientId = "ClientId-from-app-registration";
        const string authority = "https://login.microsoftonline.com/common/v2.0";
        [Fact]
        public void CredentialSourceLoaderParametersProperties()
        {
            CredentialSourceLoaderParameters parameters = new CredentialSourceLoaderParameters(clientId, authority);
            Assert.Equal(clientId, parameters.ClientId);
            Assert.Equal(authority, parameters.Authority);
        }
    }
}
