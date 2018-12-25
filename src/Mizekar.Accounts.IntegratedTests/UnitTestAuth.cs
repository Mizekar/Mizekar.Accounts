using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Mizekar.Accounts.IntegratedTests
{
    public class UnitTestAuth
    {
        [Fact]
        public async Task CallProtectedApi()
        {
            var identityUrl = "https://localhost:44300";

            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
            {
                Address = identityUrl,
            });
            Assert.False(disco.IsError, disco.Error);

            // request token
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
            {
                Address = identityUrl + "/connect/token",
                ClientId = "client",
                ClientSecret = "secret",
                Scope = "api1"
            });

            Assert.False(tokenResponse.IsError, tokenResponse.Error);

            // call api
            var newClient = new HttpClient();
            newClient.SetBearerToken(tokenResponse.AccessToken);

            var responseApi = await newClient.GetAsync("https://localhost:44348/api/identity");
            Assert.True(responseApi.IsSuccessStatusCode, responseApi.StatusCode.ToString());
        }
    }
}
