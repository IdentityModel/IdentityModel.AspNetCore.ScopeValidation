// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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