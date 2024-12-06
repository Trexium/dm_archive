using DungeonMasterArchiveData.Data;
using DungeonMastersArchive.Models.Campaign;
using Microsoft.EntityFrameworkCore;

namespace DungeonMastersArchive.Services
{
    public interface ICampaignService
    {
        Task<List<CampaignMini>> GetCampaigns(int userId);
        Task<CampaignEdit> GetCampaignEdit(int campaignId);
    }
    public class CampaignService : ICampaignService
    {
        private readonly DMArchiveContext _context;

        public CampaignService(DMArchiveContext context)
        {
            _context = context;
        }

        public async Task<CampaignEdit> GetCampaignEdit(int campaignId)
        {
            var dbResult = _context.UserCampaignRoles.Include(m => m.Campaign).Include(m => m.User).Include(m => m.Role).Where(m => m.CampaignId == campaignId);
            if (dbResult != null && dbResult.Any())
            {
                var dbCampaign = dbResult.First().Campaign;
                var campaign = new CampaignEdit();
                campaign.Id = dbCampaign.Id;
                campaign.CreatedAt = dbCampaign.CreatedAt.Value;
                campaign.CreatedBy = _context.ArchiveUsers.Include(m => m.AspNetUser).Where(m => m.Id == dbCampaign.CreatedBy.Value)
                    .Select(m => new CampaignUser
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Email = m.AspNetUser.Email
                    }).FirstOrDefault();
                campaign.UpdatedAt = dbCampaign.UpdatedAt;
                campaign.UpdatedBy = _context.ArchiveUsers.Include(m => m.AspNetUser).Where(m => m.Id == dbCampaign.UpdateBy.Value)
                    .Select(m => new CampaignUser
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Email = m.AspNetUser.Email
                    }).FirstOrDefault();
                campaign.Owner = dbCampaign.OwnerId.HasValue ? _context.ArchiveUsers.Include(m => m.AspNetUser).Where(m => m.Id == dbCampaign.OwnerId)
                    .Select(m => new CampaignUser
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Email = m.AspNetUser.Email
                    }).FirstOrDefault() : null;
                campaign.Users = dbResult.Select(m => new CampaignUser
                {
                    Id = m.UserId,
                    Email = m.User.AspNetUser.Email,
                    Name = m.User.Name,
                    Role = new CampaignRole { Id = m.RoleId, Name = m.Role.Name }
                }).ToList();
                campaign.CampaignName = dbCampaign.CampaignName;

                return campaign;
            }
            return null;
        }

        public async Task<List<CampaignMini>> GetCampaigns(int userId)
        {
            var dbCampaigns = _context.Campaigns.Include(m => m.Owner).Include(m => m.UserCampaignRoles).Where(m => m.OwnerId == userId || userId == 1).ToList();

            if (dbCampaigns != null && dbCampaigns.Any())
            {
                var campaigns = new List<CampaignMini>();
                foreach (var dbCampaign in dbCampaigns)
                {
                    campaigns.Add(new CampaignMini
                    {
                        Id = dbCampaign.Id,
                        Name = dbCampaign.CampaignName,
                        OwnerName = dbCampaign.Owner?.Name,
                        NumberOfUsers = dbCampaign.UserCampaignRoles.Count
                    });
                }

                return campaigns;
            }
            return null;
        }
    }
}
