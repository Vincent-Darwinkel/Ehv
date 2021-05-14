using System;
using System.Globalization;
using System.Linq;
using Event_Service.CustomExceptions;
using Event_Service.Enums;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Event_Service.Logic
{
    public class JwtLogic
    {
        public T GetClaim<T>(string jwt, JwtClaim claim)
        {
            if (jwt == null)
            {
                throw new UnprocessableException();
            }

            string key = Enum.GetName(typeof(JwtClaim), claim);
            var handler = new JsonWebTokenHandler();
            JsonWebToken jwtToken = handler.ReadJsonWebToken(jwt);

            string foundClaim = jwtToken.Claims?
                .FirstOrDefault(c => c.Type.Equals(key, StringComparison.OrdinalIgnoreCase))?
                .Value;

            if (typeof(T) == typeof(Guid))
            {
                return (T)Convert.ChangeType(Guid.Parse(foundClaim), typeof(T), CultureInfo.InvariantCulture);
            }
            if (typeof(T) == typeof(AccountRole))
            {
                return (T)Convert.ChangeType(Enum.Parse<AccountRole>(foundClaim), typeof(T), CultureInfo.InvariantCulture);
            }

            return default;
        }
    }
}