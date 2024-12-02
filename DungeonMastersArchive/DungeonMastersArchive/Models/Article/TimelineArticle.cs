namespace DungeonMastersArchive.Models.Article
{
    public class TimelineArticle
    {
        public int Id { get; set; }
        public int SortOrderNumber { get; set; }
        public string ArticleName { get; set; }
        public string ArticleText {  get; set; }
        public int TimelineDay { get; set; }
        public int TimelineMonth { get; set; }
        public int TimelineYear { get; set; }
        public string TimelineDate { get; set; }

    }
}
