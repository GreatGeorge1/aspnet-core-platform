using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Platform.Authorization.Roles;
using Platform.Authorization.Users;
using Platform.MultiTenancy;
using Platform.Professions;
using Abp.IdentityServer4;

namespace Platform.EntityFrameworkCore
{
    public class PlatformDbContext : AbpZeroDbContext<Tenant, Role, User, PlatformDbContext>, IAbpPersistedGrantDbContext
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<Profession> Professions { get; set; }
        public DbSet<ProfessionTranslations> ProfessionTranslations { get; set; }
        public DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public PlatformDbContext(DbContextOptions<PlatformDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigurePersistedGrantEntity();
        }
    }
}
