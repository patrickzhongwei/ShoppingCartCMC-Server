using ShoppingCartCMC.Server.Shared.Identity;
using ShoppingCartCMC.Shared.Common;
using ShoppingCartCMC.STS.Models;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ServiceStack.Redis;
using ShoppingCartCMC.Server.Shared.Common;
using ShoppingCartCMC.Server.Shared;
using Newtonsoft.Json;
using ShoppingCartCMC.Server.Shared.DB;



namespace ShoppingCartCMC.STS.Services
{
    public class IdentityWithAdditionalClaimsProfileService: IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<MyIdentityUser> _claimsFactory;
        private readonly UserManager<MyIdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRedisClientsManager _redisManager;
        private readonly IEntityRepositoryHelper _repoHelper;
        private readonly IRedisEntityRepositoryHelper _redisHelper;

        public IdentityWithAdditionalClaimsProfileService(
            UserManager<MyIdentityUser> userManager, 
            IUserClaimsPrincipalFactory<MyIdentityUser> claimsFactory, 
            IHttpContextAccessor httpContextAccessor, 
            IRedisClientsManager redisManager,
            IEntityRepositoryHelper repoHelper,
            IRedisEntityRepositoryHelper redisHelper)
        {
            _userManager            = userManager;
            _claimsFactory          = claimsFactory;
            _httpContextAccessor    = httpContextAccessor;
            _redisManager           = redisManager;
            _repoHelper             = repoHelper;
            _redisHelper            = redisHelper;
        }

      

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub         = context.Subject.GetSubjectId();
            var user        = await _userManager.FindByIdAsync(sub);
            var roles       = await _userManager.GetRolesAsync(user);
            var principal   = await _claimsFactory.CreateAsync(user);

            var claims  = principal.Claims.ToList();
            claims      = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

            //PW: (1) add fullName
            if (claims.Where(claim => claim.Type == ClaimTypeConstants.FullName).Count() <= 0)
                claims.Add(new Claim(ClaimTypeConstants.FullName, user.FullName?? ""));

            //PW: (2) add email
            if (claims.Where(claim => claim.Type == JwtClaimTypes.Email).Count() <= 0)
                claims.Add(new Claim(JwtClaimTypes.Email, user.Email));

            //PW: (3) add roles
            if (claims.Where(claim => claim.Type == JwtClaimTypes.Role).Count() <= 0)
                claims.Add(new Claim(JwtClaimTypes.Role, string.Join(" ", roles.ToArray())));

            //PW: (3) add locale
            string locale = _httpContextAccessor.HttpContext.Request.Query[ClaimTypeConstants.Locale].FirstOrDefault();
            if (!locale.IsNullOrEmpty())
                claims.Add(new Claim(JwtClaimTypes.Locale, locale));            
            

            //PW: (4) add scope in Claim ...
            claims.Add(new Claim(JwtClaimTypes.Scope, StsSetting.ApiResourceName));

            //PW: db claims: 'sub', 'AspNet.Identity.SecurityStamp', 'preferred_usename', 'name', 'email', 'email_verified
            //PW: context.RequestedClaimTypes: "is_enabled", "is_staff", "is_vip", "client_username", and RoleConstants     
            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub             = context.Subject.GetSubjectId();
            var user            = await _userManager.FindByIdAsync(sub);
            context.IsActive    = (user != null) && user.IsEnabled; //PW: double check user must be enabled here, as we've checked at Login already.
        }

    }
}
