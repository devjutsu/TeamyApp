using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Teamy.Server.Models;

namespace Teamy.Server.Data
{
    public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<AppUser>
    {
        public AppClaimsPrincipalFactory(
        UserManager<AppUser> userManager
        , IOptions<IdentityOptions> optionsAccessor)
    : base(userManager, optionsAccessor)
        { }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AppUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("DisplayName", user.DisplayName ?? user.UserName));
            return identity;
        }
    }
}
