using FluentAssertions;
using IdentityModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class PrimaryAuthentication
    {
        [Fact]
        public async Task Anonymous_User_Should_Be_Allowed()
        {
            var principal = Principal.Anonymous;
            var allowedScopes = new[] { "scope1", "scope2" };

            var client = CreateClient(principal, allowedScopes);
            var response = await client.GetAsync("/");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Authenticated_User_Missing_Scopes_Should_Be_Forbidden()
        {
            var principal = Principal.Create("custom",
                new Claim("sub", "123"));
            var allowedScopes = new[] { "scope1", "scope2" };

            var client = CreateClient(principal, allowedScopes);
            var response = await client.GetAsync("/");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Authenticated_User_Matching_Scope_Should_Be_Allowed()
        {
            var principal = Principal.Create("custom",
                new Claim("sub", "123"),
                new Claim("scope", "scope1"));
            var allowedScopes = new[] { "scope1", "scope2" };

            var client = CreateClient(principal, allowedScopes);
            var response = await client.GetAsync("/");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        private HttpClient CreateClient(ClaimsPrincipal principal, IEnumerable<string> allowedScopes)
        {
            var startup = new PrimaryAuthenticationStartup(principal, allowedScopes);
            var server = new TestServer(new WebHostBuilder()
                .Configure(startup.Configure)
                .ConfigureServices(startup.ConfigureServices));

            return server.CreateClient();
        }
    }
}