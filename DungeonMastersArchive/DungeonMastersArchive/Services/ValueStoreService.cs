using DungeonMasterArchiveData.Data;
using DungeonMastersArchive.Models;

namespace DungeonMastersArchive.Services
{
    public interface IValueStoreService
    {
        Task<List<ValueStoreItem<int, string>>> GetArticleTypes();
        Task<List<ValueStoreItem<KeyType, ValueType>>> GetGenericValueStoreGroup<KeyType, ValueType>(string group);
        Task<List<ValueStoreItem<int, string>>> GetArticles(int campaignId);
        Task<List<ValueStoreItem<int, string>>> GetTags();
    }
    public class ValueStoreService : IValueStoreService
    {
        private readonly DMArchiveContext _context;

        public ValueStoreService(DMArchiveContext context)
        {
            _context = context;
        }

        public async Task<List<ValueStoreItem<int, string>>> GetTags()
        {
            return _context.ArticleTags.Select(m => new ValueStoreItem<int, string> { Key = m.Id, Value = m.Tag }).ToList();
        }

        public async Task<List<ValueStoreItem<int, string>>> GetArticles(int campaignId)
        {
            return _context.Articles
                .Where(m => m.CampaignId == campaignId)
                .Select(m => new ValueStoreItem<int, string> { Key = m.Id, Value = m.ArticleName}).ToList();
        }

        public async Task<List<ValueStoreItem<int, string>>> GetArticleTypes()
        {
            var valueStore = new List<ValueStoreItem<int, string>>();
            foreach (var articleType in _context.ArticleTypes)
            {
                valueStore.Add(new ValueStoreItem<int, string>(articleType.Id, articleType.DisplayText));
            }

            return valueStore;
        }

        public async Task<List<ValueStoreItem<KeyType, ValueType>>> GetGenericValueStoreGroup<KeyType, ValueType>(string group)
        {
            var dbItems = _context.GenericValueStores.Where(m => m.Group == group).OrderBy(m => m.Value).ToList();
            var items = dbItems
                .Select(m => new ValueStoreItem<KeyType, ValueType>((KeyType)Convert.ChangeType(m.Key, typeof(KeyType)), (ValueType)Convert.ChangeType(m.Value, typeof(ValueType))))
                .ToList();
            return items;
        }
    }
}
