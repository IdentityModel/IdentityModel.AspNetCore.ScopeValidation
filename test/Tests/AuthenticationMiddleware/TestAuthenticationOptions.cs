// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Builder;
using System.Security.Claims;

namespace Tests
{
    public class TestAuthenticationOptions : AuthenticationOptions
    {
        public ClaimsPrincipal User { get; set; }
    }
}