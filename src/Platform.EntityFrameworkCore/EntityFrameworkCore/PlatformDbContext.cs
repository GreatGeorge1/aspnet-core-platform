using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Platform.Authorization.Roles;
using Platform.Authorization.Users;
using Platform.MultiTenancy;
using Platform.Professions;
using Abp.IdentityServer4;
using Abp.Localization;

namespace Platform.EntityFrameworkCore
{
    public class PlatformDbContext : AbpZeroDbContext<Tenant, Role, User, PlatformDbContext>, IAbpPersistedGrantDbContext
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<Profession> Professions { get; set; }
        public DbSet<ProfessionTranslations> ProfessionTranslations { get; set; }
        public DbSet<Block> Blocks { get; set; }
        public DbSet<BlockTranslations> BlockTranslations { get; set; }
        public DbSet<StepInfo> StepInfos { get; set; }
        public DbSet<StepTest> StepTests { get; set; }
        public DbSet<StepTranslations>  StepTranslations{get;set;}
        public DbSet<Answer> Answers { get; set; }

        public DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public PlatformDbContext(DbContextOptions<PlatformDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigurePersistedGrantEntity();
            modelBuilder.Entity<ApplicationLanguageText>()
               .Property(p => p.Value)
               .HasMaxLength(100); // any integer that is smaller than 10485760

            modelBuilder.Entity<Profession>()
                .HasMany(p => p.Blocks)
                .WithOne(b => b.Profession)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StepBase>()
                .ToTable("Steps")
                .HasDiscriminator<string>("StepType")
                .HasValue<StepInfo>("Info")
                .HasValue<StepTest>("Test");

            modelBuilder.Entity<Block>()
                .HasMany(b => b.Steps)
                .WithOne(s => s.Block)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
