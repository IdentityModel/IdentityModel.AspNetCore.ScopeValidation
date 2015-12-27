using System.Collections.Generic;

namespace IdentityModel.AspNet.ScopeValidation
{
    public class ScopeValidationOptions
    {
        public string ScopeClaimType { get; set; } = "scope";
        public IEnumerable<string> AllowedScopes { get; set; }
    }
}