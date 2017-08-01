// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace Tests
{
    public class TestAuthenticationOptions : AuthenticationSchemeOptions
    {
        public ClaimsPrincipal User { get; set; }
    }
}