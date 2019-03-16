using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Platform.Authorization.Roles;
using Platform.Authorization.Users;
using Platform.MultiTenancy;
using Platform.Professions;

namespace Platform.EntityFrameworkCore
{
    public class PlatformDbContext : AbpZeroDbContext<Tenant, Role, User, PlatformDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<Profession> Professions { get; set; }
        public DbSet<ProfessionTranslations> ProfessionTranslations { get; set; }

        public PlatformDbContext(DbContextOptions<PlatformDbContext> options)
            : base(options)
        {
        }
    }
}
