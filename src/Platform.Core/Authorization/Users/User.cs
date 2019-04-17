using System;
using System.Collections.Generic;
using Abp.Authorization.Users;
using Abp.Extensions;
using Platform.Events;
using Platform.Packages;

namespace Platform.Authorization.Users
{
    public class User : AbpUser<User>
    {

        public string PhoneNumber { get; set; }
        public DateTime DOB { get; set; }
        public ICollection<Order> Orders {get;set;}
        public ICollection<UserEvents> UserEvents { get; set; }
        public const string DefaultPassword = "123qwe";

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public static User CreateTenantAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();

            return user;
        }
    }
}
