using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace Tests
{
    public class TestAuthenticationMiddleware : AuthenticationMiddleware<TestAuthenticationOptions>
    {
        public TestAuthenticationMiddleware(RequestDelegate next, IOptions<TestAuthenticationOptions> options, ILoggerFactory loggerFactory, UrlEncoder encoder) 
            : base(next, options, loggerFactory, encoder)
        { }

        protected override AuthenticationHandler<TestAuthenticationOptions> CreateHandler()
        {
            return new TestAuthenticationHandler();
        }
    }
}