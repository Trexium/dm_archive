using System.Security.Claims;

namespace DungeonMastersArchiveWeb.Auth
{
    public class User
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public int Age { get; set; }
        public string Role { get; set; }

        //public ClaimsPrincipal ToClaimsPrincipal() => new(new ClaimsIdentity(new Claim[]
        //{
        //new (ClaimTypes.Name, Username),
        //new (ClaimTypes.Hash, Password),
        //new (nameof(Age), Age.ToString())
        //}.Concat(Roles.Select(r => new Claim(ClaimTypes.Role, r)).ToArray()),
        //"SLArmour"));

        public static User FromClaimsPrincipal(ClaimsPrincipal principal) => new()
        {
            Username = principal.FindFirst(ClaimTypes.Name)?.Value ?? "",
            Password = principal.FindFirst(ClaimTypes.Hash)?.Value ?? "",
            Age = Convert.ToInt32(principal.FindFirst(nameof(Age))?.Value),
            Role = principal.FindFirst(ClaimTypes.Role)?.Value ?? ""
        };
    }
}
