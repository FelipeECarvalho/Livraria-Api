using Livraria.Models.Users;
using System.Security.Claims;

namespace Livraria.Extensions
{
    public static class RoleClaimsExtension
    {
        public static IEnumerable<Claim> GetClaims(this User user)
        {
            var result = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email)
            };

            result.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Slug)));

            return result;
        }
    }
}
