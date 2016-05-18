using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using IdentityModel;

namespace Tests
{
    public class MultipleAuthentication
    {
        [Fact]
        public async Task No_Authentication_Middleware_Should_Be_Allowed()
        {
            var client = CreateClient(null, null, false, new[] { "scope1" }, "");

            var response = await client.GetAsync("/");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Single_Anonymous_User_Passive_Should_Be_Allowed()
        {
            var anon = Principal.Anonymous;

            var client = CreateClient(anon, null, false, new[] { "scope1" }, "");

            var response = await client.GetAsync("/");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Multiple_Anonymous_Users_Passive_Should_Be_Allowed()
        {
            var anon = Principal.Anonymous;

            var client = CreateClient(anon, anon, false, new[] { "scope1" }, "");

            var response = await client.GetAsync("/");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Multiple_Anonymous_Users_Active_Should_Be_Allowed()
        {
            var anon = Principal.Anonymous;

            var client = CreateClient(anon, anon, true, new[] { "scope1" }, "");

            var response = await client.GetAsync("/");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Multiple_Anonymous_User_Passive_Scheme1_Should_Be_Allowed()
        {
            var anon = Principal.Anonymous;

            var client = CreateClient(anon, anon, false, new[] { "scope1" }, "scheme1");

            var response = await client.GetAsync("/");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Multiple_Anonymous_User_Active_Scheme1_Should_Be_Allowed()
        {
            var anon = Principal.Anonymous;

            var client = CreateClient(anon, anon, true, new[] { "scope1" }, "scheme1");

            var response = await client.GetAsync("/");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Single_Authenticated_User_Missing_Scope_Passive_Should_Be_Allowed()
        {
            var user = Principal.Create("custom",
                new Claim("sub", "123"));

            var client = CreateClient(user, null, false, new[] { "scope1" }, "");

            var response = await client.GetAsync("/");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Single_Authenticated_User_Missing_Scope_Active_Should_Be_Forbidden()
        {
            var user = Principal.Create("custom",
                new Claim("sub", "123"));

            var client = CreateClient(user, null, true, new[] { "scope1" }, "");

            var response = await client.GetAsync("/");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Multiple_Authenticated_User_Missing_Scope_Active_Should_Be_Forbidden()
        {
            var user1 = Principal.Create("custom",
                new Claim("sub", "123"));
            var user2 = Principal.Create("custom",
                new Claim("sub", "456"));

            var client = CreateClient(user1, user2, true, new[] { "scope1" }, "");

            var response = await client.GetAsync("/");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Multiple_Authenticated_Users_Included_Scope_Active_Should_Be_Allowed()
        {
            var user1 = Principal.Create("custom",
                new Claim("sub", "123"));
            var user2 = Principal.Create("custom",
                new Claim("sub", "456"),
                new Claim("scope", "scope1"));

            var client = CreateClient(user1, user2, true, new[] { "scope1" }, "");

            var response = await client.GetAsync("/");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Multiple_Authenticated_Users_Included_Scope_Active_Scheme1_Should_Be_Forbidden()
        {
            var user1 = Principal.Create("custom",
                new Claim("sub", "123"));
            var user2 = Principal.Create("custom",
                new Claim("sub", "456"),
                new Claim("scope", "scope1"));

            var client = CreateClient(user1, user2, true, new[] { "scope1" }, "scheme1");

            var response = await client.GetAsync("/");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Multiple_Authenticated_Users_Included_Scope_Active_Scheme2_Should_Be_Allowed()
        {
            var user1 = Principal.Create("custom",
                new Claim("sub", "123"));
            var user2 = Principal.Create("custom",
                new Claim("sub", "456"),
                new Claim("scope", "scope1"));

            var client = CreateClient(user1, user2, true, new[] { "scope1" }, "scheme2");

            var response = await client.GetAsync("/");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        //[Fact]
        //public async Task Authenticated_User_Missing_Scopes_Should_Be_Forbidden()
        //{
        //    var principal = Principal.Create("custom",
        //        new Claim("sub", "123"));
        //    var allowedScopes = new[] { "scope1", "scope2" };

        //    var client = CreateClient(principal, allowedScopes);
        //    var response = await client.GetAsync("/");

        //    response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        //}

        //[Fact]
        //public async Task Authenticated_User_Matching_Scope_Should_Be_Allowed()
        //{
        //    var principal = Principal.Create("custom",
        //        new Claim("sub", "123"),
        //        new Claim("scope", "scope1"));
        //    var allowedScopes = new[] { "scope1", "scope2" };

        //    var client = CreateClient(principal, allowedScopes);
        //    var response = await client.GetAsync("/");

        //    response.StatusCode.Should().Be(HttpStatusCode.OK);
        //}

        private HttpClient CreateClient(ClaimsPrincipal principal1, ClaimsPrincipal principal2, bool automaticAuthenticate, IEnumerable<string> allowedScopes, string scopeAuthenticationScheme)
        {
            var options = new ScopeValidationOptions
            {
                AllowedScopes = allowedScopes,
                AuthenticationScheme = scopeAuthenticationScheme
            };

            var startup = new MultipleAuthenticationStartup(principal1, principal2, automaticAuthenticate, options);
            var server = new TestServer(new WebHostBuilder()
                .Configure(startup.Configure)
                .ConfigureServices(startup.ConfigureServices));

            return server.CreateClient();
        }
    }
}