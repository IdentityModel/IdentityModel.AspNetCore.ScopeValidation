// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Tests
{
    public class MultipleAuthenticationStartup
    {
        private readonly ClaimsPrincipal _principal1;
        private readonly ClaimsPrincipal _principal2;
        private readonly ScopeValidationOptions _scopeOptions;
        
        public MultipleAuthenticationStartup(ClaimsPrincipal principal1, ClaimsPrincipal principal2, ScopeValidationOptions options)
        {
            _principal1 = principal1;
            _principal2 = principal2;
            _scopeOptions = options;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication();

            if (_principal1 != null)
            {
                services.AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>(
                    "scheme1",
                    options => options.User = _principal1);
            }

            if (_principal2 != null)
            {
                services.AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>(
                    "scheme2",
                    options => options.User = _principal2);
            }
        }

        public void Configure(IApplicationBuilder app)
        {
            app.AllowScopes(_scopeOptions);

            app.Run(ctx =>
            {
                ctx.Response.StatusCode = 200;
                return Task.FromResult(0);
            });
        }
    }
}