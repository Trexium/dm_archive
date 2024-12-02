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
        public string TimelineDateDisplayText { get; set; }
        public List<ArticleTag> Tags { get; set; }
        public bool HasImages {  get; set; }
        public bool HasLinks { get; set; }
    }
}
