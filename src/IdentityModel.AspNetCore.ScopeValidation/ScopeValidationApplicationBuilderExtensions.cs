using IdentityModel.AspNetCore.ScopeValidation;

namespace Microsoft.AspNetCore.Builder
{
    public static class ScopeValidationApplicationBuilderExtensions
    {
        public static IApplicationBuilder AllowScopes(this IApplicationBuilder app, params string[] scopes)
        {
            var options = new ScopeValidationOptions
            {
                AllowedScopes = scopes
            };

            return app.AllowScopes(options);
        }

        public static IApplicationBuilder AllowScopes(this IApplicationBuilder app, ScopeValidationOptions options)
        {
            return app.UseMiddleware<ScopeValidationMiddleware>(options);
        }
    }
}