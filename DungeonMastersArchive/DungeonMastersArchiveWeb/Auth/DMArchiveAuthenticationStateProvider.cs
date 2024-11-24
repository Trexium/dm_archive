using DungeonMastersArchiveWeb.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace DungeonMastersArchiveWeb.Auth
{
    public class DMArchiveAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IUserService _userService;
        private readonly DMArchiveAuthenticationService _authService;
        private AuthenticationState authenticationState;

        public DMArchiveAuthenticationStateProvider(IUserService userService, DMArchiveAuthenticationService authService)
        {
            _userService = userService;
            _authService = authService;
            authenticationState = new AuthenticationState(_authService.CurrentUser);

            _authService.UserChanged += (newUser) =>
            {
                authenticationState = new AuthenticationState(newUser);
                NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));
            };
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync() => Task.FromResult(authenticationState);

        public async Task LoginAsync(string username, string password)
        {
            var principal = new ClaimsPrincipal();
            var user = await _userService.FindUserFromDatabaseAsync(username);

            if (user is not null)
            {
                if (password == user.Password)
                {
                    
                }

                principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Hash, user.Password),
                        new Claim(ClaimTypes.Role, user.Role)
                    }, "SLArmour"));
                //principal = user.ToClaimsPrincipal();

            }

            _authService.CurrentUser = principal;
        }
    }
}
