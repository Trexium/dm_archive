namespace DungeonMastersArchive.Models.Campaign
{
    public class CampaignEdit
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public CampaignUser? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public CampaignUser? UpdatedBy { get; set; }
        public string CampaignName { get; set; }
        public List<CampaignUser>? Users { get; set; }
        public CampaignUser? Owner { get; set; }
    }
}
