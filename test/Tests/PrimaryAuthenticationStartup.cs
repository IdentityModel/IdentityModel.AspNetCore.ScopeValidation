using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Tests
{
    public class PrimaryAuthenticationStartup
    {
        private readonly IEnumerable<string> _allowedScopes;
        private readonly ClaimsPrincipal _principal;

        public PrimaryAuthenticationStartup(ClaimsPrincipal principal, IEnumerable<string> allowedScopes)
        {
            _principal = principal;
            _allowedScopes = allowedScopes;
        }

        public void Configure(IApplicationBuilder app)
        {
            app.Use(async (ctx, next) =>
            {
                ctx.User = _principal;
                await next();
            });

            app.AllowScopes(new ScopeValidationOptions { AllowedScopes = _allowedScopes });

            app.Run(ctx =>
            {
                ctx.Response.StatusCode = 200;
                return Task.FromResult(0);
            });
        }

        public void ConfigureServices(IServiceCollection services)
        { }
    }
}