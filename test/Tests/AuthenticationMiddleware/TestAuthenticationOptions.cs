using Microsoft.AspNet.Authentication;
using System.Security.Claims;

namespace Tests
{
    public class TestAuthenticationOptions : AuthenticationOptions
    {
        public ClaimsPrincipal User { get; set; }
    }
}