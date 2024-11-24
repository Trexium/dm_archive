using DungeonMasterArchiveData.Data;
using DbModels = DungeonMasterArchiveData.Models;
using DungeonMastersArchiveWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace DungeonMastersArchiveWeb.Services
{
    public interface IArticleService
    {
        Models.Article GetArticle(int id);
        Models.Article SaveArticle(Models.Article article);
        Models.Article GetArticles(int campaignId);
    }
    public class ArticleService : IArticleService
    {
        private readonly DMArchiveContext _context;

        public ArticleService(DMArchiveContext context)
        {
            _context = context;
        }

        public Article GetArticle(int id)
        {
            var dbArticle = _context.Articles
                .Include(m => m.ArticleType)
                .Include(m => m.ArticleImages)
                .Include(m => m.ArticleLinkChildArticles)
                .Include(m => m.ArticleLinkParentArticles)
                .Include(m => m.ArticleTags)
                .FirstOrDefault(a => a.Id == id);

            if (dbArticle == null)
            {
                return null;
            }


            var model = new Models.Article();
            model.Id = dbArticle.Id;
            model.ArticleText = dbArticle.ArticleText;
            model.ArticleTypeDisplayText = dbArticle.ArticleType.DisplayText;
            model.ArticleTypeId = dbArticle.ArticleTypeId.ToString();
            model.UpdatedAt = dbArticle.UpdatedAt;
            model.UpdatedBy = dbArticle.UpdateBy;
            model.CampaignId = dbArticle.CampaignId;
            model.CreatedAt = dbArticle.CreatedAt;
            model.CreatedBy = dbArticle.CreatedBy;
            model.IsDeleted = dbArticle.IsDeleted;
            model.IsPublished = dbArticle.IsPublished;

            if (dbArticle.ArticleImages != null && dbArticle.ArticleImages.Any())
            {

            }

            if (dbArticle.ArticleLinkParentArticles != null && dbArticle.ArticleLinkParentArticles.Any())
            {

            }

            if (dbArticle.ArticleLinkChildArticles != null && dbArticle.ArticleLinkChildArticles.Any())
            {

            }

            if (dbArticle.ArticleTags != null && dbArticle.ArticleTags.Any())
            {

            }


            return model;
        }

        public Article GetArticles(int campaignId)
        {
            throw new NotImplementedException();
        }

        public Article SaveArticle(Article article)
        {
            DbModels.Article dbArticle;
            if (!article.Id.HasValue)
            {
                dbArticle = new DbModels.Article();
                dbArticle.CreatedBy = 1; // TODO: Get user
            }
            else
            {
                dbArticle = _context.Articles.First(m => m.Id == article.Id);
                dbArticle.UpdateBy = 1; // TODO: Get user
                dbArticle.UpdatedAt = DateTime.Now;
            }
            dbArticle.ArticleDay = article.TimelineDay;
            dbArticle.ArticleMonth = article.TimelineMonth;
            dbArticle.ArticleYear = article.TimelineYear;

            dbArticle.ArticleText = article.ArticleText;
            dbArticle.ArticleName = article.ArticleName;
            dbArticle.ArticleTypeId = int.TryParse(article.ArticleTypeId, out int result) ? result : 8;
            dbArticle.CampaignId = article.CampaignId;
            
            if (!article.Id.HasValue)
            {
                _context.Articles.Add(dbArticle);
            }

            _context.SaveChanges();
            return GetArticle(dbArticle.Id);
        }
    }
}
