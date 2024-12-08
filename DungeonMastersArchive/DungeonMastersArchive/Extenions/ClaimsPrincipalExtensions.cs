
using Microsoft.AspNetCore.Authentication;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Claims;
using System.Security.Principal;

namespace DungeonMastersArchive.Extenions
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool HasCampaign(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.HasClaim(m => m.Type == "Campaign");
        }

        public static bool AccessCheck(this ClaimsPrincipal claimsPrincipal, AuthLevel authLevel)
        {
            if (claimsPrincipal.HasClaim(m => m.Type == "Role"))
            {
                var roleId = int.Parse(claimsPrincipal.Claims.First(m => m.Type == "Role").Value);
                if (roleId <= (int)authLevel)
                {
                    return true;
                }
            }
            return false;
        }

        public static int GetCampaign(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal.HasCampaign())
            {
                return int.Parse(claimsPrincipal.Claims.First(m => m.Type == "Campaign").Value);
            }
            return -1;
        }

        public static int GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal.HasClaim(m => m.Type == "UserId"))
            {
                return int.Parse(claimsPrincipal.Claims.First(m => m.Type == "UserId").Value);
            }

            return -1;
        }

        public static string GetName(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal.HasClaim(m => m.Type == "Name"))
            {
                return claimsPrincipal.Claims.First(m => m.Type == "Name").Value;
            }
            return null;
        }

        public static string GetUsername(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal.HasClaim(m => m.Type == ClaimTypes.NameIdentifier))
            {
                return claimsPrincipal.Claims.First(m => m.Type == ClaimTypes.NameIdentifier).Value;
            }
            return null;
        }

        public static string GetAspUserId(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal.HasClaim(m => m.Type == ClaimTypes.NameIdentifier))
            {
                return claimsPrincipal.Claims.First(m => m.Type == ClaimTypes.NameIdentifier).Value;
            }
            return null;
        }

        public static List<Claim> GetFungableClaims(this ClaimsPrincipal claimsPrincipal)
        {
            var claims = claimsPrincipal.Claims.Where(m => m.Type == "Campaign" || m.Type == "Role").ToList();
            return claims;
        }

        public static void AddUpdateClaim(this ClaimsPrincipal claimsPrincipal, string key, string value)
        {
            var identity = claimsPrincipal.Identity as ClaimsIdentity;
            if (identity == null)
                return;

            // check for existing claim and remove it
            var existingClaim = identity.FindFirst(key);
            if (existingClaim != null)
                identity.RemoveClaim(existingClaim);

            // add new claim
            identity.AddClaim(new Claim(key, value));
            //var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            //authenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(identity), new AuthenticationProperties() { IsPersistent = true });
        }
    }

}
