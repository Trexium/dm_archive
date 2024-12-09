using DungeonMasterArchiveData.Data;
using DungeonMastersArchive.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DungeonMastersArchive.Components.Account
{
    public class ApplicationUserClaimsTransformation : IClaimsTransformation
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DMArchiveContext _context;
        public ApplicationUserClaimsTransformation(UserManager<ApplicationUser> userManager, DMArchiveContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identity = principal.Identities.FirstOrDefault(c => c.IsAuthenticated);
            if (identity == null) return principal;

            var user = await _userManager.GetUserAsync(principal);
            if (user == null) return principal;

            var archiveUser = _context.ArchiveUsers.Include(m => m.UserCampaignRoles).FirstOrDefault(m => m.AspNetUserId == user.Id);
            if (archiveUser == null) return principal;

            // Add or replace identity.Claims.

            var campaignId = archiveUser.CurrentCampaignId ?? 0;
            var roleId = campaignId != 0 ? archiveUser.UserCampaignRoles.First(m => m.CampaignId == campaignId).RoleId : 0;

            identity.AddClaim(new Claim("Campaign", campaignId.ToString()));
            identity.AddClaim(new Claim("Role", roleId.ToString()));

            return new ClaimsPrincipal(identity);
        }
    }
}
