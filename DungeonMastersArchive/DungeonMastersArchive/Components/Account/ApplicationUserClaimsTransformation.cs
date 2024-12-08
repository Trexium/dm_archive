using DungeonMastersArchive.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DungeonMastersArchive.Components.Account
{
    public class ApplicationUserClaimsTransformation : IClaimsTransformation
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public ApplicationUserClaimsTransformation(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identity = principal.Identities.FirstOrDefault(c => c.IsAuthenticated);
            if (identity == null) return principal;

            var user = await _userManager.GetUserAsync(principal);
            if (user == null) return principal;

            // Add or replace identity.Claims.


            if (!principal.HasClaim(c => c.Type == "Campaign"))
            {
                identity.AddClaim(new Claim("Campaign", user.));

            }
            if (!principal.HasClaim(c => c.Type == ClaimTypes.Surname))
            {
                identity.AddClaim(new Claim(ClaimTypes.Surname, user.LastName));
            }

            return new ClaimsPrincipal(identity);
        }
    }
}
