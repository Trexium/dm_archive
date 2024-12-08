using DungeonMasterArchiveData.Data;
using DungeonMastersArchive.Data;
using DungeonMastersArchive.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace DungeonMastersArchive.Components.Account
{
    public class MySignInManager<TUser> : SignInManager<TUser> where TUser : ApplicationUser
    {
        private readonly DMArchiveContext _dmArchiveContext;
        private readonly SystemDefaults _systemDefaults;
        public MySignInManager(UserManager<TUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<TUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<TUser>> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<TUser> confirmation, DMArchiveContext dmArchiveContext, IOptions<SystemDefaults> systemDefauls) : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
            _dmArchiveContext = dmArchiveContext;
            _systemDefaults = systemDefauls.Value;
        }

        public override ILogger Logger { get => base.Logger; set => base.Logger = value; }

        public override Task<bool> CanSignInAsync(TUser user)
        {
            return base.CanSignInAsync(user);
        }

        public override Task<SignInResult> CheckPasswordSignInAsync(TUser user, string password, bool lockoutOnFailure)
        {
            return base.CheckPasswordSignInAsync(user, password, lockoutOnFailure);
        }

        public override AuthenticationProperties ConfigureExternalAuthenticationProperties(string? provider, [StringSyntax("Uri")] string? redirectUrl, string? userId = null)
        {
            return base.ConfigureExternalAuthenticationProperties(provider, redirectUrl, userId);
        }

        public override Task<ClaimsPrincipal> CreateUserPrincipalAsync(TUser user)
        {
            return base.CreateUserPrincipalAsync(user);
        }

        public override bool Equals(object? obj)
        {
            return obj is MySignInManager<TUser> manager &&
                   EqualityComparer<ILogger>.Default.Equals(Logger, manager.Logger) &&
                   EqualityComparer<UserManager<TUser>>.Default.Equals(UserManager, manager.UserManager) &&
                   EqualityComparer<IUserClaimsPrincipalFactory<TUser>>.Default.Equals(ClaimsFactory, manager.ClaimsFactory) &&
                   EqualityComparer<IdentityOptions>.Default.Equals(Options, manager.Options) &&
                   AuthenticationScheme == manager.AuthenticationScheme &&
                   EqualityComparer<HttpContext>.Default.Equals(Context, manager.Context);
        }

        public override Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent)
        {
            return base.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent);
        }

        public override Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent, bool bypassTwoFactor)
        {
            return base.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent, bypassTwoFactor);
        }

        public override Task ForgetTwoFactorClientAsync()
        {
            return base.ForgetTwoFactorClientAsync();
        }

        public override Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync()
        {
            return base.GetExternalAuthenticationSchemesAsync();
        }

        public override Task<ExternalLoginInfo?> GetExternalLoginInfoAsync(string? expectedXsrf = null)
        {
            return base.GetExternalLoginInfoAsync(expectedXsrf);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override Task<TUser?> GetTwoFactorAuthenticationUserAsync()
        {
            return base.GetTwoFactorAuthenticationUserAsync();
        }

        public override bool IsSignedIn(ClaimsPrincipal principal)
        {
            return base.IsSignedIn(principal);
        }

        public override Task<bool> IsTwoFactorClientRememberedAsync(TUser user)
        {
            return base.IsTwoFactorClientRememberedAsync(user);
        }

        public override Task<bool> IsTwoFactorEnabledAsync(TUser user)
        {
            return base.IsTwoFactorEnabledAsync(user);
        }

        public override async Task<SignInResult> PasswordSignInAsync(TUser user, string password, bool isPersistent, bool lockoutOnFailure)
        {
            var archiveUser = _dmArchiveContext.ArchiveUsers.Include(m => m.UserCampaignRoles).FirstOrDefault(m => m.AspNetUserId == user.Id);
            var claimsToRemove = (await this.UserManager.GetClaimsAsync(user)).Where(m => m.Type == "UserId" || m.Type == "Name" || m.Type == "Role" || m.Type == "Campaign");
            if (claimsToRemove != null && claimsToRemove.Any())
            {
                await this.UserManager.RemoveClaimsAsync(user, claimsToRemove);
            }

            await this.UserManager.AddClaimAsync(user, new Claim("UserId", archiveUser.Id.ToString(), "int"));
            await this.UserManager.AddClaimAsync(user, new Claim("Name", archiveUser.Name, "string"));

            if (user.UserName == _systemDefaults.AppAdmin)
            {
                await this.UserManager.AddClaimAsync(user, new Claim("Role", "1"));
            }
            else
            {
                await this.UserManager.AddClaimAsync(user, new Claim("Role", archiveUser.UserCampaignRoles.First(m => m.CampaignId == archiveUser.CurrentCampaignId.Value).RoleId.ToString()));
            }


            await this.UserManager.AddClaimAsync(user, new Claim("Campaign", archiveUser.CurrentCampaignId?.ToString() ?? "0"));

            return await base.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
        }

        public override Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        {
            return base.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);
        }

        public override Task RefreshSignInAsync(TUser user)
        {
            return base.RefreshSignInAsync(user);
        }

        public override Task RememberTwoFactorClientAsync(TUser user)
        {
            return base.RememberTwoFactorClientAsync(user);
        }

        public override Task SignInAsync(TUser user, bool isPersistent, string? authenticationMethod = null)
        {
            return base.SignInAsync(user, isPersistent, authenticationMethod);
        }

        public override Task SignInAsync(TUser user, AuthenticationProperties authenticationProperties, string? authenticationMethod = null)
        {
            return base.SignInAsync(user, authenticationProperties, authenticationMethod);
        }

        public override Task SignInWithClaimsAsync(TUser user, bool isPersistent, IEnumerable<Claim> additionalClaims)
        {
            return base.SignInWithClaimsAsync(user, isPersistent, additionalClaims);
        }

        public override Task SignInWithClaimsAsync(TUser user, AuthenticationProperties? authenticationProperties, IEnumerable<Claim> additionalClaims)
        {
            return base.SignInWithClaimsAsync(user, authenticationProperties, additionalClaims);
        }

        public override Task SignOutAsync()
        {

            return base.SignOutAsync();
        }

        public override string? ToString()
        {
            return base.ToString();
        }

        public override Task<SignInResult> TwoFactorAuthenticatorSignInAsync(string code, bool isPersistent, bool rememberClient)
        {
            return base.TwoFactorAuthenticatorSignInAsync(code, isPersistent, rememberClient);
        }

        public override Task<SignInResult> TwoFactorRecoveryCodeSignInAsync(string recoveryCode)
        {
            return base.TwoFactorRecoveryCodeSignInAsync(recoveryCode);
        }

        public override Task<SignInResult> TwoFactorSignInAsync(string provider, string code, bool isPersistent, bool rememberClient)
        {
            return base.TwoFactorSignInAsync(provider, code, isPersistent, rememberClient);
        }

        public override Task<IdentityResult> UpdateExternalAuthenticationTokensAsync(ExternalLoginInfo externalLogin)
        {
            return base.UpdateExternalAuthenticationTokensAsync(externalLogin);
        }

        public override Task<TUser?> ValidateSecurityStampAsync(ClaimsPrincipal? principal)
        {
            return base.ValidateSecurityStampAsync(principal);
        }

        public override Task<bool> ValidateSecurityStampAsync(TUser? user, string? securityStamp)
        {
            return base.ValidateSecurityStampAsync(user, securityStamp);
        }

        public override Task<TUser?> ValidateTwoFactorSecurityStampAsync(ClaimsPrincipal? principal)
        {
            return base.ValidateTwoFactorSecurityStampAsync(principal);
        }

        protected override Task<bool> IsLockedOut(TUser user)
        {
            return base.IsLockedOut(user);
        }

        protected override Task<SignInResult> LockedOut(TUser user)
        {
            return base.LockedOut(user);
        }

        protected override Task<SignInResult?> PreSignInCheck(TUser user)
        {
            return base.PreSignInCheck(user);
        }

        protected override Task ResetLockout(TUser user)
        {
            return base.ResetLockout(user);
        }

        protected override Task<SignInResult> SignInOrTwoFactorAsync(TUser user, bool isPersistent, string? loginProvider = null, bool bypassTwoFactor = false)
        {
            return base.SignInOrTwoFactorAsync(user, isPersistent, loginProvider, bypassTwoFactor);
        }
    }
}
