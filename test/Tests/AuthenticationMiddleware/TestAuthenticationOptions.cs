
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;

namespace Tests
{
    public class TestAuthenticationOptions : AuthenticationOptions
    {
        public ClaimsPrincipal User { get; set; }
    }
}