using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Acheve.TestHost
{
    internal static class DefautClaimsEncoder
    {
        public static string Encode(IEnumerable<Claim> claims)
        {
            var ticket = new AuthenticationTicket(
                principal: new ClaimsPrincipal(
                    new ClaimsIdentity(claims)),
                authenticationScheme: "TestServer");

            var serializer = new TicketSerializer();
            var bytes = serializer.Serialize(ticket);

            return Convert.ToBase64String(bytes);
        }

        public static IEnumerable<Claim> Decode(string encodedValue)
        {
            if (string.IsNullOrEmpty(encodedValue))
            {
                return Enumerable.Empty<Claim>();
            }

            var serializer = new TicketSerializer();
            try
            {
                var ticket = serializer.Deserialize(Convert.FromBase64String(encodedValue));

                return ticket.Principal.Claims;
            }
            catch (Exception)
            {
                return Enumerable.Empty<Claim>();
            }            
        }
    }
}
