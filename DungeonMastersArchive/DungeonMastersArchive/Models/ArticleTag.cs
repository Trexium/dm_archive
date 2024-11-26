namespace DungeonMastersArchive.Models
{
    public class ArticleTag
    {
        public int? Id { get; set; }
        public int ArticleId { get; set; }
        public string Tag { get; set; }
    }
}
