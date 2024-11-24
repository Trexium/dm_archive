namespace DungeonMastersArchiveWeb.Models
{
    public class ArticleLink
    {
        public int? Id { get; set; }
        public int ParentArticleId { get; set; }
        public int ChildArticleId { get; set; }
    }
}
