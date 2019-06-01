using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Authorization.Users;
using Abp.Extensions;
using Platform.Events;
using Platform.Packages;
using Platform.Professions.User;

namespace Platform.Authorization.Users
{
    public class User : AbpUser<User>
    {
        public DateTime DOB { get; set; }
        public ICollection<Order> Orders {get;set;}
        public ICollection<UserProfessions> UserProfessions { get; set; }
        public string NewEmail { get; set; }
        
        [Required]
        [StringLength(150)]
        public override string Name { get; set; }
        
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
               // Surname = AdminUserName,
                EmailAddress = emailAddress,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();

            return user;
        }

        [NotMapped]
        private new string Surname { get; set; }
        [NotMapped]
        private new string FullName { get; }
    }
}
