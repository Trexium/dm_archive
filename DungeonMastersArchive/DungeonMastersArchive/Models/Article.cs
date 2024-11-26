namespace DungeonMastersArchive.Models
{
    public class Article
    {
        public int? Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string ArticleName { get; set; }
        public string ArticleText { get; set; }
        public string ArticleTypeDisplayText { get; set; }
        public string ArticleTypeId { get; set; }
        public bool IsPublished { get; set; }
        public int CampaignId { get; set; }
        public int TimelineDay { get; set; }
        public int TimelineMonth { get; set; }
        public int TimelineYear { get; set; }
        public List<ArticleImage> Images { get; set; }
        public List<ArticleLink> ParentLinks { get; set; }
        public List<ArticleLink> ChildLinks { get; set; }
        public List<ArticleTag> Tags { get; set; }
    }
}
