using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.WebEncoders;

namespace Tests
{
    public class TestAuthenticationMiddleware : AuthenticationMiddleware<TestAuthenticationOptions>
    {
        public TestAuthenticationMiddleware(RequestDelegate next, TestAuthenticationOptions options, ILoggerFactory loggerFactory, IUrlEncoder encoder) 
            : base(next, options, loggerFactory, encoder)
        { }

        protected override AuthenticationHandler<TestAuthenticationOptions> CreateHandler()
        {
            return new TestAuthenticationHandler();
        }
    }
}