using DungeonMasterArchiveData.Data;
using DungeonMastersArchiveWeb.Models;

namespace DungeonMastersArchiveWeb.Services
{
    public interface IValueStoreService
    {
        List<ValueStoreItem> GetArticleTypes();
    }
    public class ValueStoreService : IValueStoreService
    {
        private readonly DMArchiveContext _context;

        public ValueStoreService(DMArchiveContext context)
        {
            _context = context;
        }

        public List<ValueStoreItem> GetArticleTypes()
        {
            var valueStore = new List<ValueStoreItem>();
            foreach (var articleType in _context.ArticleTypes)
            {
                valueStore.Add(new ValueStoreItem(articleType.Id, articleType.DisplayText));
            }

            return valueStore;
        }
    }
}
