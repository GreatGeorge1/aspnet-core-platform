using Abp.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Platform.Authorization.Roles;
using Platform.Authorization.Users;
using Platform.MultiTenancy;

namespace Platform.Identity
{
    public class SecurityStampValidator : AbpSecurityStampValidator<Tenant, Role, User>
    {
        public SecurityStampValidator(
            IOptions<SecurityStampValidatorOptions> options, 
            SignInManager signInManager,
            ISystemClock systemClock) 
            : base(
                  options, 
                  signInManager, 
                  systemClock)
        {
        }
    }
}
