using System.Collections.Generic;
using System.Security.Claims;

namespace Sample.IntegrationTests
{
    public static class Identities
    {
        public static readonly IEnumerable<Claim> User = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Name, "User"),
        };

        public static readonly IEnumerable<Claim> Empty = new Claim[0];
    }
}
