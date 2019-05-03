using Abp.Localization;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Platform.Authorization.Roles;
using Platform.Authorization.Users;
using Platform.Events;
using Platform.MultiTenancy;
using Platform.Packages;
using Platform.Professions;
using Platform.Professions.Blocks;
using Platform.Professions.User;

namespace Platform.EntityFrameworkCore
{
    public class PlatformDbContext : AbpZeroDbContext<Tenant, Role, User, PlatformDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<Profession> Professions { get; set; }
        public DbSet<ProfessionContent> ProfessionContent { get; set; }
        public DbSet<Block> Blocks { get; set; }
        public DbSet<BlockContent> BlockContent { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<StepContent> StepContent { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<AnswerContent> AnswerContent { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderPackages> OrderPackages { get; set; }
        public DbSet<UserProfessions> UserProfessions { get; set; }
        public DbSet<UserTests> UserTests { get; set; }
        public DbSet<UserTestAnswers> UserTestAnswers { get; set; }
        public DbSet<UserSeenSteps> UserSeenSteps { get; set; }

        public PlatformDbContext(DbContextOptions<PlatformDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationLanguageText>()
               .Property(p => p.Value)
               .HasMaxLength(100); // any integer that is smaller than 10485760

            modelBuilder.Entity<Profession>()
                .HasMany(p => p.Blocks)
                .WithOne(b => b.Profession)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Profession>()
                .HasMany(p => p.Packages)
                .WithOne(b => b.Profession)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Profession>()
                .HasMany(p => p.Content)
                .WithOne(b => b.Core)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Profession>()
                .HasMany(p => p.Events)
                .WithOne(b => b.Profession)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Step>()
                .HasMany(s => s.Answers)
                .WithOne(a => a.Test);

            modelBuilder.Entity<Block>()
                .HasMany(b => b.Steps)
                .WithOne(s => s.Block)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Block>().ToTable("Blocks");

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

            modelBuilder.Entity<OrderPackages>()
                .HasKey(op => op.Id);
            modelBuilder.Entity<OrderPackages>()
                .HasOne(op => op.Order)
                .WithMany(o => o.OrderPackages);
            modelBuilder.Entity<OrderPackages>()
                .HasOne(op => op.Package)
                .WithMany(p => p.OrderPackages);

            modelBuilder.Entity<UserTestAnswers>()
                .HasKey(ut => ut.Id);
            modelBuilder.Entity<UserTestAnswers>()
                .HasOne(ut => ut.UserTest)
                .WithMany(ut => ut.UserTestAnswers);
            modelBuilder.Entity<UserTestAnswers>()
                .HasOne(ut => ut.Answer)
                .WithMany(a => a.UserTestAnswers);

        }
    }
}
