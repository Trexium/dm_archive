namespace DungeonMastersArchive.Models.Article
{
    public class Article
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public string ArticleName { get; set; }
        public string ArticleText { get; set; }
        public string ArticleTypeDisplayText { get; set; }
        public string ArticleTypeId { get; set; }
        public int TimelineDay { get; set; }
        public int? TimelineMonthId { get; set; }
        public string TimelineMonthStringId { get; set; }
        public string TimelineMonthText { get; set; }
        public int TimelineYear { get; set; }
        //public List<ArticleImageMetadata> Images { get; set; }
        //public List<ArticleLink> Links { get; set; }
        public List<ArticleTag> Tags { get; set; }
    }
}
