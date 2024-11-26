using DungeonMasterArchiveData.Data;
using DungeonMastersArchive.Models;

namespace DungeonMastersArchive.Services
{
    public interface IValueStoreService
    {
        Task<List<ValueStoreItem>> GetArticleTypes();
    }
    public class ValueStoreService : IValueStoreService
    {
        private readonly DMArchiveContext _context;

        public ValueStoreService(DMArchiveContext context)
        {
            _context = context;
        }

        public async Task<List<ValueStoreItem>> GetArticleTypes()
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
