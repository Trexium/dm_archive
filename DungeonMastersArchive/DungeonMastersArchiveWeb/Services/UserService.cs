using DungeonMasterArchiveData.Data;
using DataModels = DungeonMasterArchiveData.Models;
using DungeonMastersArchiveWeb.Auth;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace DungeonMastersArchiveWeb.Services
{
    public interface IUserService
    {
        Task<User?> FindUserFromDatabaseAsync(string username);
        Task PersistUserToBrowserAsync(User user);
    }
    public class UserService : IUserService
    {
        private readonly ProtectedLocalStorage _protectedLocalStorage;
        private readonly string _storageKey = "slArmourIdentity";
        private readonly DMArchiveContext _context;

        public UserService(DMArchiveContext context, ProtectedLocalStorage protectedLocalStorage)
        {
            _context = context;
            _protectedLocalStorage = protectedLocalStorage;
        }

        public async Task<User?> FindUserFromDatabaseAsync(string username)
        {
            var dbUser = _context.Users.Include(m => m.Role).FirstOrDefault(m => m.Username == username);

            if (dbUser != null)
            {
                var user = new User();
                user.Username = dbUser.Username;
                user.Password = dbUser.Password;
                user.Age = dbUser.Age;
                user.Role = dbUser.Role.Name;
                await PersistUserToBrowserAsync(user);
                return user;
            }
            else
            {
                return null;
            }
        }

        public async Task PersistUserToBrowserAsync(User user)
        {
            string userJson = JsonConvert.SerializeObject(user);
            await _protectedLocalStorage.SetAsync(_storageKey, userJson);
        }
    }
}
