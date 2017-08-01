// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Tests
{
    public class TestAuthenticationHandler : AuthenticationHandler<TestAuthenticationOptions>
    {
        public TestAuthenticationHandler(IOptionsSnapshot<TestAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) 
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var ticket = new AuthenticationTicket(
                Options.User, 
                new AuthenticationProperties(),
                Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}