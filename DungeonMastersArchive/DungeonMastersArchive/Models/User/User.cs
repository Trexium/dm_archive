namespace DungeonMastersArchive.Models.User
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CurrentCampaignId { get; set; }
        public string AspUserId { get; set; }
    }
}
