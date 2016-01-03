using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Http.Authentication;
using System.Threading.Tasks;

namespace Tests
{
    public class TestAuthenticationHandler : AuthenticationHandler<TestAuthenticationOptions>
    {
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var ticket = new AuthenticationTicket(
                Options.User, 
                new AuthenticationProperties(), 
                Options.AuthenticationScheme);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}