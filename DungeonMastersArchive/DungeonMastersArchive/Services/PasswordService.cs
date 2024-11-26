using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace DungeonMastersArchive.Services
{
    public interface IPasswordService
    {
        string GetHashedPassword(string password);
        bool Validate(string clearTextPassword, string hashedPassword);
    }

    public class PasswordService : IPasswordService
    {
        private readonly byte[] _salt;

        public PasswordService(byte[] salt)
        {
            _salt = salt;
        }

        public string GetHashedPassword(string password)
        {
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: _salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 100000,
                numBytesRequested: 512 / 8
            ));

            return hashedPassword;
        }

        public bool Validate(string clearTextPassword, string hashedPassword)
        {
            if (GetHashedPassword(clearTextPassword) == hashedPassword)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
