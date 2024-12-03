using DungeonMasterArchiveData.Data;
using DungeonMastersArchive.Models.User;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DataModels = DungeonMasterArchiveData.Models;


namespace DungeonMastersArchive.Services
{
    public interface IUserService
    {
        Task<User> GetCurrentUser();
        Task<List<UserMini>> GetUsers(int? campaignId);
        Task<EditUser> GetEditUser(int userId, int campaignId);
    }
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly DMArchiveContext _context;

        public UserService(DMArchiveContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<EditUser> GetEditUser(int userId, int campaignId)
        {
            var archiveUser = _context.ArchiveUsers
                .Include(m => m.AspNetUser)
                .Include(m => m.UserCampaignRoles).ThenInclude(m => m.Role)
                .FirstOrDefault(m => m.Id == userId);

            if (archiveUser != null)
            {
                var user = new EditUser
                {
                    Id = archiveUser.Id,
                    Email = archiveUser.AspNetUser.Email,
                    Name = archiveUser.Name,
                    IsVerified = archiveUser.AspNetUser.EmailConfirmed,
                    AspNetUser = archiveUser.AspNetUser,
                    IsDeleted = archiveUser.
                };

                var dbRole = archiveUser.UserCampaignRoles.FirstOrDefault(m => m.CampaignId == campaignId)?.Role;

                user.Role = new Role
                {
                    Id = dbRole.Id,
                    Name = dbRole.Name
                };

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
            var debugUser = new User { Id = 1, Name = "DebugUser", CurrentCampaignId = 2 };
            return debugUser;
            var userGuid = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userGuid))
            {
                var dbUser = _context.ArchiveUsers.Include(m => m.AspNetUser).FirstOrDefault(m => m.AspNetUserId == userGuid);

                var user = new User
                {
                    Id = dbUser.Id,
                    Name = dbUser.Name,
                    CurrentCampaignId = dbUser.CurrentCampaignId ?? 0
                };

                return user;
            }

            return null;
        }

    }
}