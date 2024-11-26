namespace DungeonMastersArchive.Models
{
    public class ArticleImage
    {
        public int? Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int ArticleId { get; set; }
        public int CampaignId { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
    }
}
