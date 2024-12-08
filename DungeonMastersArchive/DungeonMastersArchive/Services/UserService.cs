using DungeonMasterArchiveData.Data;
using DungeonMasterArchiveData.Models;
using DungeonMastersArchive.Components.Account;
using DungeonMastersArchive.Data;
using DungeonMastersArchive.Models;
using DungeonMastersArchive.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MudBlazor;
using System.Security.Claims;
using System.Text;
using DataModels = DungeonMasterArchiveData.Models;


namespace DungeonMastersArchive.Services
{
    public interface IUserService
    {
        Task<User> GetCurrentUser();
        Task<List<UserMini>> GetUsers(int? campaignId);
        Task<UserEdit> GetEditUser(int userId, int campaignId);
        Task<bool> DeleteUser(int userId);
        Task<bool> UndeleteUser(int userId);
        Task<UserEdit> SaveUser(UserEdit user, int campaignId);
        Task<int> SetCurrentCampaign(int userId, int campaignId);
        Task<bool> AddUserToCampaign(int userId, int campaignId, int roleId, bool setAsCurrent = false);

    }
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly DMArchiveContext _context;
        private readonly SystemDefaults _systemDefaults;

        public UserService(DMArchiveContext context, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, IUserStore<ApplicationUser> userStore, IOptions<SystemDefaults> systemDefaults)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _userStore = userStore;
            _systemDefaults = systemDefaults.Value;
        }

        public async Task<int> SetCurrentCampaign(int userId, int campaignId)
        {
            try
            {
                var dbUser = _context.ArchiveUsers.Include(m => m.UserCampaignRoles).FirstOrDefault(m => m.Id == userId);
                if (dbUser != null)
                {
                    dbUser.CurrentCampaignId = campaignId;
                    await _context.SaveChangesAsync();
                    var roleId = dbUser.UserCampaignRoles.FirstOrDefault(m => m.CampaignId == campaignId)?.RoleId ?? 0;
                    return roleId;
                }
            }
            catch (Exception ex)
            {

            }
            return 0;
        }

        public async Task<bool> AddUserToCampaign(int userId, int campaignId, int roleId, bool setAsCurrent = false)
        {
            try
            {
                var alreadyOnCampaign = _context.UserCampaignRoles.Where(m => m.UserId == userId && m.CampaignId == campaignId).Any();
                if (!alreadyOnCampaign)
                {
                    var userCampaign = new UserCampaignRole();
                    userCampaign.UserId = userId;
                    userCampaign.CampaignId = campaignId;
                    userCampaign.RoleId = roleId;
                    _context.UserCampaignRoles.Add(userCampaign);
                    await _context.SaveChangesAsync();

                    if (setAsCurrent)
                    {
                        await SetCurrentCampaign(userId, campaignId);
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        public async Task<UserEdit> GetEditUser(int userId, int campaignId)
        {
            var archiveUser = _context.ArchiveUsers
                .Include(m => m.AspNetUser)
                .Include(m => m.UserCampaignRoles).ThenInclude(m => m.Role)
                .FirstOrDefault(m => m.Id == userId);

            if (archiveUser != null)
            {
                var user = new UserEdit
                {
                    Id = archiveUser.Id,
                    Email = archiveUser.AspNetUser.Email,
                    Name = archiveUser.Name,
                    IsVerified = archiveUser.AspNetUser.EmailConfirmed,
                    IsDeleted = archiveUser.IsDeleted,
                    AspNetUserId = archiveUser.AspNetUserId,
                };

                var dbRole = archiveUser.UserCampaignRoles.FirstOrDefault(m => m.CampaignId == campaignId)?.Role;

                user.RoleId = dbRole.Id;

                return user;
            }

            return null;
        }

        public async Task<List<UserMini>> GetUsers(int? campaignId)
        {
            var archiveUsers = _context.ArchiveUsers.Include(m => m.AspNetUser).Where(m => campaignId == null || m.UserCampaignRoles.Select(m2 => m2.CampaignId).Contains(campaignId.Value)).ToList();

            if (archiveUsers.Any())
            {
                var users = archiveUsers.Select(m => new UserMini
                {
                    Id = m.Id,
                    Email = m.AspNetUser.Email,
                    Name = m.Name
                }).ToList();
                return users;
            }

            return null;
        }

        public async Task<User> GetCurrentUser()
        {
            //var debugUser = new User { Id = 1, Name = "DebugUser", CurrentCampaignId = 2 };
            //return debugUser;
            var userGuid = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userGuid))
            {
                var dbUser = _context.ArchiveUsers.Include(m => m.AspNetUser).FirstOrDefault(m => m.AspNetUserId == userGuid);

                var user = new User
                {
                    Id = dbUser.Id,
                    Name = dbUser.Name,
                    CurrentCampaignId = dbUser.CurrentCampaignId ?? 0,
                    AspUserId = userGuid
                };

                return user;
            }

            return null;
        }

        public async Task<bool> DeleteUser(int userId)
        {
            var user = _context.ArchiveUsers.FirstOrDefault(m => m.Id == userId);
            if (user != null)
            {
                user.IsDeleted = true;
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UndeleteUser(int userId)
        {
            var user = _context.ArchiveUsers.FirstOrDefault(m => m.Id == userId);
            if (user != null)
            {
                user.IsDeleted = false;
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<UserEdit> SaveUser(UserEdit user, int campaignId)
        {
            DataModels.ArchiveUser dbUser;
            if (!user.Id.HasValue)
            {
                dbUser = new DataModels.ArchiveUser();
                dbUser.IsDeleted = false;
                dbUser.AspNetUserId = user.AspNetUserId;
            }
            else
            {
                await _context.UserCampaignRoles.Where(m => m.UserId == user.Id && m.CampaignId == campaignId).ExecuteDeleteAsync();
                dbUser = _context.ArchiveUsers
                    .Include(m => m.AspNetUser)
                    .Include(m => m.UserCampaignRoles)
                    .First(m => m.Id == user.Id);
            }

            dbUser.Name = user.Name;

            dbUser.UserCampaignRoles = new List<DataModels.UserCampaignRole>();
            dbUser.UserCampaignRoles.Add(new DataModels.UserCampaignRole
            {
                CampaignId = campaignId,
                RoleId = user.RoleId.Value
            });

            if (!user.Id.HasValue)
            {
                _context.ArchiveUsers.Add(dbUser);

            }

            try
            {
                await _context.SaveChangesAsync();
                return await GetEditUser(dbUser.Id, campaignId);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}