// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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
using Tests;

namespace Tests
{
    public class MultipleAuthentication
    {
        [Fact]
        public async Task no_authentication_middleware_should_be_allowed()
        {
            var client = CreateClient(null, null, new[] { "scope1" }, "");

            var response = await client.GetAsync("/");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task anonymous_user_should_be_allowed()
        {
            var anon = Principal.Anonymous;

            var client = CreateClient(anon, anon, new[] { "scope1" }, "");

            var response = await client.GetAsync("/");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("scheme1")]
        [InlineData("scheme2")]
        public async Task authenticated_user_with_allowed_scope_should_be_allowed(string scheme)
        {
            var user = Principal.Create("custom",
                new Claim("sub", "123"),
                new Claim("scope", "scope1"));

            var client = CreateClient(user, user, new[] { "scope1" }, scheme);

            var response = await client.GetAsync("/");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("scheme1")]
        [InlineData("scheme2")]
        public async Task authenticated_user_with_missing_scope_should_not_be_allowed(string scheme)
        {
            var user = Principal.Create("custom",
                new Claim("sub", "123"),
                new Claim("scope", "scope2"));

            var client = CreateClient(user, user, new[] { "scope1" }, scheme);

            var response = await client.GetAsync("/");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
        
        private HttpClient CreateClient(ClaimsPrincipal principal1, ClaimsPrincipal principal2, IEnumerable<string> allowedScopes, string scopeAuthenticationScheme)
        {
            var options = new ScopeValidationOptions
            {
                AllowedScopes = allowedScopes,
                AuthenticationScheme = scopeAuthenticationScheme
            };

            var startup = new MultipleAuthenticationStartup(principal1, principal2, options);
            var server = new TestServer(new WebHostBuilder()
                .Configure(startup.Configure)
                .ConfigureServices(startup.ConfigureServices));

            return server.CreateClient();
        }
    }
}