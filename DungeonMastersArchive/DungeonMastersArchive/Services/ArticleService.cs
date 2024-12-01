using DungeonMasterArchiveData.Data;
using DbModels = DungeonMasterArchiveData.Models;
using DungeonMastersArchive.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using Humanizer;
using System.Diagnostics.CodeAnalysis;
using DungeonMastersArchive.Models.Article;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DungeonMastersArchive.Services
{
    public interface IArticleService
    {
        Task<ArticleEdit> GetArticleEdit(int id);
        Task<Article> GetArticle(int id);
        Task<ArticleEdit> SaveArticle(ArticleEdit article);
        Task<List<ArticleMini>> GetArticles();
        Task<bool> DeleteArticle(int id);
        Task RemoveImageFromArticle(int imageId);
        Task<Dictionary<string, List<ArticleLink>>> GetArticleLinks(int articleId);
        Task<List<ArticleImageMetadata>> GetArticleImages(int articleId);
        Task<List<TimelineArticle>> GetTimeline(int campaignId);
    }
    public class ArticleService : IArticleService
    {
        private readonly DMArchiveContext _context;
        private readonly IUserService _userService;
        private readonly IValueStoreService _valueStoreService;
        private readonly IDateService _dateService;

        public ArticleService(DMArchiveContext context, IUserService userService, IValueStoreService valueStoreService, IDateService dateService)
        {
            _context = context;
            _userService = userService;
            _valueStoreService = valueStoreService;
            _dateService = dateService;
        }

        public async Task<List<TimelineArticle>> GetTimeline(int campaignId)
        {
            var dbArticles = _context.Articles.Where(m => m.CampaignId == campaignId && m.ArticleTypeId == 4 && !m.IsDeleted && m.IsPublished).OrderBy(m => m.ArticleYear).ThenBy(m => m.ArticleMonth).ThenBy(m => m.ArticleDay).ToList();

            if (dbArticles.Any())
            {
                var timelineArticles = new List<TimelineArticle>();
                var monthDict = (await _valueStoreService.GetGenericValueStoreGroup<int, string>("months")).ToDictionary(k => k.Key, v => v.Value);
                var counter = 1;
                foreach (var article in dbArticles)
                {
                    timelineArticles.Add(new TimelineArticle
                    {
                        Id = article.Id,
                        DayCount = counter,
                        ArticleName = article.ArticleName,
                        ArticleText = article.ArticleText.Length > 100 ? $"{article.ArticleText.Substring(0, 97)}..." : article.ArticleText,
                        TimelineDay = article.ArticleDay.Value,
                        TimelineMonth = article.ArticleMonth.Value,
                        TimelineYear = article.ArticleYear.Value,
                        TimelineDate = $"Den {_dateService.GetOrdinalString(counter)} dagen, {article.ArticleDay.Value}{_dateService.GetDaySuffix(article.ArticleDay.Value)} {monthDict[article.ArticleMonth.Value]}."
                    });
                    counter++;
                }
                return timelineArticles;
            }
            return null;
        }

        public async Task<List<ArticleImageMetadata>> GetArticleImages(int articleId)
        {
            var dbImages = _context.ArticleImages.Where(m => m.ArticleId == articleId).ToList();
            if (dbImages.Any())
            {
                var images = new List<ArticleImageMetadata>();
                foreach (var dbImage in dbImages)
                {
                    images.Add(new ArticleImageMetadata
                    {
                        Id = dbImage.Id,
                        ArticleId = dbImage.ArticleId,
                        CampaignId = dbImage.CampaignId,
                        CreatedAt = dbImage.CreatedAt,
                        FileName = dbImage.FileName,
                        Title = dbImage.Title
                    });
                }

                return images;
            }

            return null;
        }

        public async Task<Dictionary<string, List<ArticleLink>>> GetArticleLinks(int articleId)
        {
            var dbChildLinks = _context.ArticleLinks.Include(m => m.ChildArticle).ThenInclude(m => m.ArticleType).Where(m => m.ParentArticleId == articleId);
            var dbParentLinks = _context.ArticleLinks.Include(m => m.ParentArticle).ThenInclude(m => m.ArticleType).Where(m => m.ChildArticleId == articleId);

            var linkList = new List<ArticleLink>();

            foreach (var dbLink in dbChildLinks)
            {
                linkList.Add(new ArticleLink
                {
                    ArticleId = dbLink.ChildArticleId,
                    ArticleName = dbLink.ChildArticle.ArticleName,
                    ArticleType = dbLink.ChildArticle.ArticleType.DisplayText,
                    GroupName = dbLink.GroupName
                });
            }
            foreach (var dbLink in dbParentLinks)
            {
                linkList.Add(new ArticleLink
                {
                    ArticleId = dbLink.ParentArticleId,
                    ArticleName = dbLink.ParentArticle.ArticleName,
                    ArticleType = dbLink.ParentArticle.ArticleType.DisplayText,
                    GroupName = dbLink.GroupName
                });
            }

            var linksWithGroup = linkList.Where(m => !string.IsNullOrEmpty(m.GroupName));
            var linksWithoutGroup = linkList.Where(m => string.IsNullOrEmpty(m.GroupName) && !linksWithGroup.Select(m2 => m2.ArticleId).Contains(m.ArticleId));

            var links = linksWithGroup.Select(m => new { Key = m.GroupName, Value = m }).ToList();
            links.AddRange(linksWithoutGroup.Select(m => new { Key = m.ArticleType, Value = m }));

            var resultDict = links.GroupBy(m => m.Key).ToDictionary(k => k.Key, v => v.Select(m => m.Value).ToList());
            if (resultDict != null && resultDict.Any())
            {
                return resultDict;
            }
            else
            {
                return null;
            }
        }

        public async Task<Article> GetArticle(int id)
        {
            var dbArticle = _context.Articles
                .Include(m => m.ArticleType)
                .Include(m => m.ArticleImages)
                .Include(m => m.ArticleTags)
                .FirstOrDefault(a => a.Id == id);

            if (dbArticle == null)
            {
                return null;
            }


            var model = new Models.Article.Article();
            model.Id = dbArticle.Id;
            model.ArticleName = dbArticle.ArticleName;
            model.ArticleText = dbArticle.ArticleText;
            model.ArticleTypeDisplayText = dbArticle.ArticleType.DisplayText;
            model.ArticleTypeId = dbArticle.ArticleTypeId.ToString();
            model.UpdatedAt = dbArticle.UpdatedAt;
            model.UpdatedBy = dbArticle.UpdateBy;
            model.CreatedAt = dbArticle.CreatedAt.Value;
            model.CreatedBy = dbArticle.CreatedBy;

            if (dbArticle.ArticleTypeId == 4)
            {
                var monthDict = (await _valueStoreService.GetGenericValueStoreGroup<int, string>("months")).ToDictionary(k => k.Key, v => v.Value);
                var day = _context.Articles.Where(m => m.CampaignId == dbArticle.CampaignId && !m.IsDeleted && m.IsPublished).OrderBy(m => m.ArticleYear).ThenBy(m => m.ArticleMonth).ThenBy(m => m.ArticleDay).ToList().IndexOf(dbArticle);
                model.ArticleTypeDisplayText = $"Den {_dateService.GetOrdinalString(day)} dagen, {dbArticle.ArticleDay.Value}{_dateService.GetDaySuffix(dbArticle.ArticleDay.Value)} {monthDict[dbArticle.ArticleMonth.Value]}.";
            }

            if (dbArticle.ArticleTags != null && dbArticle.ArticleTags.Any())
            {
                model.Tags = new List<ArticleTag>();
                foreach (var tag in dbArticle.ArticleTags)
                {
                    model.Tags.Add(new ArticleTag { Id = tag.Id, ArticleId = tag.ArticleId, Tag = tag.Tag });
                }
            }

            return model;
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

        public async Task<ArticleEdit> GetArticleEdit(int id)
        {
            var dbArticle = _context.Articles
                .Include(m => m.ArticleType)
                .Include(m => m.ArticleImages)
                .Include(m => m.ArticleTags)
                .FirstOrDefault(a => a.Id == id);

            if (dbArticle == null)
            {
                return null;
            }


            var model = new Models.Article.ArticleEdit();
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
            model.TimelineDay = dbArticle.ArticleDay;
            model.TimelineMonthId = dbArticle.ArticleMonth;
            model.TimelineYear = dbArticle.ArticleYear;
            model.TimelineMonthStringId = dbArticle.ArticleMonth?.ToString();

            if (dbArticle.ArticleImages != null && dbArticle.ArticleImages.Any())
            {
                model.Images = new List<ArticleImageMetadata>();
                foreach (var dbImage in dbArticle.ArticleImages)
                {
                    model.Images.Add(new ArticleImageMetadata
                    {
                        Id = dbImage.Id,
                        ArticleId = dbImage.ArticleId,
                        CampaignId = dbImage.CampaignId,
                        CreatedAt = dbImage.CreatedAt,
                        FileName = dbImage.FileName,
                        Title = dbImage.Title
                    });
                }
            }

            var parentArticles = _context.ArticleLinks.Include(m => m.ParentArticle).ThenInclude(m => m.ArticleType).Where(m => m.ChildArticleId == dbArticle.Id).ToList();
            if (parentArticles != null && parentArticles.Any())
            {
                model.ParentLinks = parentArticles
                    .Select(m => new ArticleLink
                    {
                        ArticleId = m.ParentArticleId,
                        ArticleName = m.ParentArticle.ArticleName,
                        GroupName = m.GroupName,
                        ArticleType = m.ParentArticle.ArticleType.DisplayText
                    }).ToList();
            }

            var childArticles = _context.ArticleLinks.Include(m => m.ChildArticle).ThenInclude(m => m.ArticleType).Where(m => m.ParentArticleId == dbArticle.Id).ToList();
            if (childArticles != null && childArticles.Any())
            {
                model.ChildLinks = childArticles
                    .Select(m => new ArticleLink
                    {
                        ArticleId = m.ChildArticleId,
                        ArticleName = m.ChildArticle.ArticleName,
                        GroupName = m.GroupName,
                        ArticleType = m.ChildArticle.ArticleType.DisplayText
                    }).ToList();
            }

            if (dbArticle.ArticleTags != null && dbArticle.ArticleTags.Any())
            {
                model.Tags = new List<ArticleTag>();
                foreach (var tag in dbArticle.ArticleTags)
                {
                    model.Tags.Add(new ArticleTag { Id = tag.Id, ArticleId = tag.ArticleId, Tag = tag.Tag });
                }
            }


            return model;
        }

        public async Task<List<ArticleMini>> GetArticles()
        {
            var user = await _userService.GetCurrentUser();
            var dbArticles = _context.Articles.Include(m => m.ArticleType).Where(m => m.CampaignId == user.CurrentCampaignId && !m.IsDeleted).ToList();
            var articles = new List<ArticleMini>();
            foreach (var dbArticle in dbArticles)
            {
                articles.Add(new ArticleMini
                {
                    Id = dbArticle.Id,
                    ArticleName = dbArticle.ArticleName,
                    ArticleType = dbArticle.ArticleType.DisplayText,
                    IsPublished = dbArticle.IsPublished,
                });
            }

            return articles;
        }

        public async Task RemoveImageFromArticle(int imageId)
        {
            var image = _context.ArticleImages.FirstOrDefault(m => m.Id == imageId);
            if (image != null)
            {
                image.ArticleId = null;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ArticleEdit> SaveArticle(ArticleEdit article)
        {
            var user = await _userService.GetCurrentUser();

            DbModels.Article dbArticle;
            if (!article.Id.HasValue)
            {
                dbArticle = new DbModels.Article();
                dbArticle.CreatedBy = user.Id;
                dbArticle.CampaignId = user.CurrentCampaignId;
            }
            else
            {
                dbArticle = _context.Articles
                    .Include(m => m.ArticleTags)
                    .Include(m => m.ArticleImages)
                    .Include(m => m.ArticleLinkChildArticles)
                    .Include(m => m.ArticleLinkParentArticles)
                    .First(m => m.Id == article.Id);
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

            _context.SaveChanges();


            if (article.Images != null)
            {
                var currentIds = article.Images.Where(m => m.Id.HasValue).Select(m => m.Id.Value).ToList();

                var toBeRemoved = dbArticle.ArticleImages.Where(m => !currentIds.Contains(m.Id));

                foreach (var image in toBeRemoved)
                {
                    image.ArticleId = null;
                }

                var imagesToAdd = article.Images.Where(m => m.Id == null).ToList();
                foreach (var image in imagesToAdd)
                {
                    if (dbArticle.ArticleImages == null)
                    {
                        dbArticle.ArticleImages = new List<DbModels.ArticleImage>();
                    }
                    dbArticle.ArticleImages.Add(new DbModels.ArticleImage
                    {
                        CampaignId = dbArticle.CampaignId,
                        CreatedBy = user.Id,
                        FileName = image.FileName,
                        Title = image.Title
                    });
                }

                _context.SaveChanges();
            }

            if (article.Tags != null)
            {
                _context.ArticleTags.RemoveRange(dbArticle.ArticleTags);
                _context.SaveChanges();

                dbArticle.ArticleTags = new List<DbModels.ArticleTag>();
                foreach (var tag in article.Tags)
                {
                    dbArticle.ArticleTags.Add(new DbModels.ArticleTag { Tag = tag.Tag });
                }
            }

            if (article.ChildLinks != null)
            {
                _context.ArticleLinks.RemoveRange(_context.ArticleLinks.Where(m => m.ParentArticleId == dbArticle.Id));
                _context.SaveChanges();

                var dbChilds = new List<DbModels.ArticleLink>();
                foreach (var childLink in article.ChildLinks)
                {
                    dbChilds.Add(new DbModels.ArticleLink { ChildArticleId = childLink.ArticleId, ParentArticleId = dbArticle.Id, GroupName = childLink.GroupName });
                }
                _context.ArticleLinks.AddRange(dbChilds);
            }

            

            try
            {
                await _context.SaveChangesAsync();
                return await GetArticleEdit(dbArticle.Id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
