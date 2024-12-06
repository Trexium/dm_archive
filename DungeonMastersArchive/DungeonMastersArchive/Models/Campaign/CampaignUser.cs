namespace DungeonMastersArchive.Models.Campaign
{
    public class CampaignUser
    {
        public int Id { get; set; } 
        public string? Name { get; set; }
        public string? Email { get; set; }
        public CampaignRole? Role { get; set; }
    }
}
