using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.Security.Claims;

namespace Acheve.TestHost
{
    public class TestServerOptions : AuthenticationSchemeOptions
    {
        public IEnumerable<Claim> CommonClaims { get; set; } = new Claim[0];

        public string NameClaimType { get; set; } = ClaimTypes.Name;

        public string RoleClaimType { get; set; } = ClaimTypes.Role;
    }
}