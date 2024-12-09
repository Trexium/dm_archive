using Microsoft.AspNetCore.Identity;

namespace DungeonMastersArchive.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public int? CurrentCampaign { get; set; }
        public int? CurrentRole { get; set; }
    }

}
