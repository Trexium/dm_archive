using DungeonMasterArchiveData.Data;
using DbModels = DungeonMasterArchiveData.Models;
using DungeonMastersArchive.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace DungeonMastersArchive.Services
{
    public interface IArticleService
    {
        Task<Models.Article> GetArticle(int id);
        Task<Models.Article> SaveArticle(Models.Article article);
        Task<List<Models.Article>> GetArticles();
        Task<bool> DeleteArticle(int id);
    }
    public class ArticleService : IArticleService
    {
        private readonly DMArchiveContext _context;
        private readonly IUserService _userService;

        public ArticleService(DMArchiveContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<bool> DeleteArticle(int id)
        {
            var article = _context.Articles.FirstOrDefault(m => m.Id == id);
            if (article != null)
            {
                article.IsDeleted = true;
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Models.Article> GetArticle(int id)
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
            model.ArticleName = dbArticle.ArticleName;
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

        public async Task<List<Models.Article>> GetArticles()
        {
            var user = await _userService.GetCurrentUser();
            var dbArticles = _context.Articles.Include(m => m.ArticleType).Where(m => m.CampaignId == user.CurrentCampaignId && !m.IsDeleted).ToList();
            var articles = new List<Models.Article>();
            foreach (var dbArticle in dbArticles)
            {
                articles.Add(new Article
                {
                    Id = dbArticle.Id,
                    ArticleName = dbArticle.ArticleName,
                    ArticleText = dbArticle.ArticleText,
                    ArticleTypeDisplayText = dbArticle.ArticleType.DisplayText,
                    ArticleTypeId = dbArticle.ArticleTypeId.ToString(),
                    UpdatedAt = dbArticle.UpdatedAt,
                    UpdatedBy = dbArticle.UpdateBy,
                    CampaignId = dbArticle.CampaignId,
                    CreatedAt = dbArticle.CreatedAt,
                    CreatedBy = dbArticle.CreatedBy,
                    IsDeleted = dbArticle.IsDeleted,
                    IsPublished = dbArticle.IsPublished,
                });
            }

            return articles;
        }

        public async Task<Models.Article> SaveArticle(Article article)
        {
            var user = await _userService.GetCurrentUser();

            DbModels.Article dbArticle;
            if (!article.Id.HasValue)
            {
                dbArticle = new DbModels.Article();
                dbArticle.CreatedBy = user.Id;
                dbArticle.CampaignId = user.CurrentCampaignId.Value;
            }
            else
            {
                dbArticle = _context.Articles.First(m => m.Id == article.Id);
                dbArticle.UpdateBy = user.Id;
                dbArticle.UpdatedAt = DateTime.Now;
            }
            dbArticle.ArticleDay = article.TimelineDay;
            dbArticle.ArticleMonth = article.TimelineMonthId;
            dbArticle.ArticleYear = article.TimelineYear;

            dbArticle.ArticleText = article.ArticleText;
            dbArticle.ArticleName = article.ArticleName;
            dbArticle.ArticleTypeId = int.TryParse(article.ArticleTypeId, out int result) ? result : 8;
            dbArticle.CampaignId = 2; //dbArticle.CampaignId = article.CampaignId;


            if (!article.Id.HasValue)
            {
                _context.Articles.Add(dbArticle);
            }

            try
            {
                await _context.SaveChangesAsync();
                return await GetArticle(dbArticle.Id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
