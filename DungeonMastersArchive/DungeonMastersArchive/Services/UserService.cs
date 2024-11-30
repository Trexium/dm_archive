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

        public async Task<User> GetCurrentUser()
        {
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