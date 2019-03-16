using Abp.Authorization;
using Platform.Authorization.Roles;
using Platform.Authorization.Users;

namespace Platform.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
