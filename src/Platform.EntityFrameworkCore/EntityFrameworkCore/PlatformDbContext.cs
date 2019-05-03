using Abp.Localization;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Platform.Authorization.Roles;
using Platform.Authorization.Users;
using Platform.Events;
using Platform.MultiTenancy;
using Platform.Packages;
using Platform.Professions;
using Platform.Professions.User;

namespace Platform.EntityFrameworkCore
{
    public class PlatformDbContext : AbpZeroDbContext<Tenant, Role, User, PlatformDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<Profession> Professions { get; set; }
        public DbSet<ProfessionTranslations> ProfessionTranslations { get; set; }
        public DbSet<Block> Blocks { get; set; }
        public DbSet<BlockTranslations> BlockTranslations { get; set; }
        public DbSet<StepInfo> StepInfos { get; set; }
        public DbSet<StepTest> StepTests { get; set; }
        public DbSet<StepBase> Steps { get; set; }
        public DbSet<StepTranslations> StepTranslations { get; set; }
        public DbSet<Answer> Answers { get; set; }

        public DbSet<Package> Packages { get; set; }
        public DbSet<PackageTranslations> PackageTranslations { get; set; }
        public DbSet<PackageProfession> PackageProfessions { get; set; }

        public DbSet<Event> Events { get; set; }
        public DbSet<EventTranslations> EventTranslations { get; set; }
        public DbSet<EventProfession> EventProfessions { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<UserEvents> UserEvents {get;set;}
        public DbSet<UserProfessions> UserProfessions { get; set; }
        public DbSet<UserTests> UserTests { get; set; }

        public PlatformDbContext(DbContextOptions<PlatformDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           // modelBuilder.ConfigurePersistedGrantEntity();
            modelBuilder.Entity<ApplicationLanguageText>()
               .Property(p => p.Value)
               .HasMaxLength(100); // any integer that is smaller than 10485760

            modelBuilder.Entity<Profession>()
                .HasMany(p => p.Blocks)
                .WithOne(b => b.Profession)
                .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<StepBase>()
            //    .Property(s => s.Duration)
            //    .HasConversion<TimeSpanToTicksConverter>();

           // modelBuilder.Entity<StepInfo>().ToTable("StepInfos");
           // modelBuilder.Entity<StepTest>();

            modelBuilder.Entity<StepBase>()
                .ToTable("Steps")
                .HasDiscriminator<string>("StepType")
                .HasValue<StepInfo>("Info")
                .HasValue<StepTest>("Test");

            modelBuilder.Entity<Block>()
                .HasMany(b => b.Steps)
                .WithOne(s => s.Block)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PackageProfession>()
                //в odata не работают композитные ключи(в swagger тоже)
                //.HasKey(pp => new { pp.PackageId, pp.ProfessionId });
                .HasKey(pp => pp.Id);
            modelBuilder.Entity<PackageProfession>()
                .HasOne(pp => pp.Package)
                .WithMany(pp => pp.PackageProfessions)
                .HasForeignKey(pp => pp.PackageId);
            modelBuilder.Entity<PackageProfession>()
                .HasOne(pp => pp.Profession)
                .WithMany(pp => pp.PackageProfessions)
                .HasForeignKey(pp => pp.ProfessionId);

            modelBuilder.Entity<EventProfession>()
                .HasKey(ep => ep.Id);
            modelBuilder.Entity<EventProfession>()
                .HasOne(ep => ep.Event)
                .WithMany(ep => ep.EventProfessions)
                .HasForeignKey(ep => ep.EventId);
            modelBuilder.Entity<EventProfession>()
                .HasOne(ep => ep.Profession)
                .WithMany(ep => ep.EventProfessions)
                .HasForeignKey(ep => ep.ProfessionId);

            modelBuilder.Entity<UserEvents>()
                .HasKey(ep => ep.Id);
            modelBuilder.Entity<UserEvents>()
                .HasOne(ep => ep.User)
                .WithMany(ep => ep.UserEvents)
                .HasForeignKey(ep => ep.UserId);
            modelBuilder.Entity<UserEvents>()
                .HasOne(ep => ep.Event)
                .WithMany(ep => ep.UserEvents)
                .HasForeignKey(ep => ep.EventId);

            modelBuilder.Entity<UserProfessions>()
                .HasKey(ep => ep.Id);
            modelBuilder.Entity<UserProfessions>()
                .HasOne(ep => ep.User)
                .WithMany(ep => ep.UserProfessions)
                .HasForeignKey(ep => ep.UserId);
            modelBuilder.Entity<UserProfessions>()
                .HasOne(ep => ep.Profession)
                .WithMany(ep => ep.UserProfessions)
                .HasForeignKey(ep => ep.ProfessionId);

            modelBuilder.Entity<UserProfessions>()
                .HasMany(up => up.UserTests)
                .WithOne(ut => ut.UserProfession)
                .OnDelete(DeleteBehavior.Cascade);

      
        }
    }
}
