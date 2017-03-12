// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;

namespace Microsoft.AspNetCore.Builder
{
    public class ScopeValidationOptions
    {
        public string ScopeClaimType { get; set; } = "scope";
        public IEnumerable<string> AllowedScopes { get; set; }
        public string AuthenticationScheme { get; set; }
    }
}