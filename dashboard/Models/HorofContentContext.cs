using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HorofDashboard.Models;

public partial class HorofContentContext : DbContext
{
    public HorofContentContext()
    {
    }

    public HorofContentContext(DbContextOptions<HorofContentContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Achievement> Achievements { get; set; }

    public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<LessonProgress> LessonProgresses { get; set; }

    public virtual DbSet<Level> Levels { get; set; }

    public virtual DbSet<LevelPricing> LevelPricings { get; set; }

    public virtual DbSet<MasterSlideType> MasterSlideTypes { get; set; }

    public virtual DbSet<PaymentTransaction> PaymentTransactions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RoleClaim> RoleClaims { get; set; }

    public virtual DbSet<Slide> Slides { get; set; }

    public virtual DbSet<Unit> Units { get; set; }

    public virtual DbSet<UserAchievement> UserAchievements { get; set; }

    public virtual DbSet<UserActivityLog> UserActivityLogs { get; set; }

    public virtual DbSet<UserClaim> UserClaims { get; set; }

    public virtual DbSet<UserLogin> UserLogins { get; set; }

    public virtual DbSet<UserProgress> UserProgresses { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<UserSlideProgress> UserSlideProgresses { get; set; }

    public virtual DbSet<UserSubscription> UserSubscriptions { get; set; }

    public virtual DbSet<UserToken> UserTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Arabic_CI_AS");

        modelBuilder.Entity<Achievement>(entity =>
        {
            entity.Property(e => e.BadgeColor).HasMaxLength(20);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IconUrl).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.RequirementType).HasMaxLength(50);
        });

        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable("ApplicationUser");

            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.PreferredLanguage).HasMaxLength(10);
            entity.Property(e => e.ProfilePicture).HasMaxLength(500);
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasIndex(e => e.UnitId, "IX_Lessons_UnitId");

            entity.Property(e => e.Url).HasDefaultValue("");

            entity.HasOne(d => d.Unit).WithMany(p => p.Lessons).HasForeignKey(d => d.UnitId);
        });

        modelBuilder.Entity<LessonProgress>(entity =>
        {
            entity.ToTable("LessonProgress");

            entity.HasIndex(e => e.LessonId, "IX_LessonProgress_LessonId");

            entity.HasIndex(e => e.UserId, "IX_LessonProgress_UserId");

            entity.HasOne(d => d.Lesson).WithMany(p => p.LessonProgresses).HasForeignKey(d => d.LessonId);

            entity.HasOne(d => d.User).WithMany(p => p.LessonProgresses).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Level>(entity =>
        {
            entity.Property(e => e.PicIcon).HasColumnName("Pic_Icon");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Url).HasDefaultValue("");
        });

        modelBuilder.Entity<LevelPricing>(entity =>
        {
            entity.ToTable("LevelPricing");

            entity.HasIndex(e => e.LevelId, "IX_LevelPricing_LevelId");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.DiscountedAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PriceType).HasMaxLength(50);

            entity.HasOne(d => d.Level).WithMany(p => p.LevelPricings).HasForeignKey(d => d.LevelId);
        });

        modelBuilder.Entity<MasterSlideType>(entity =>
        {
            entity.ToTable("MasterSlideType");
        });

        modelBuilder.Entity<PaymentTransaction>(entity =>
        {
            entity.HasIndex(e => e.LevelId, "IX_PaymentTransactions_LevelId");

            entity.HasIndex(e => e.UserId, "IX_PaymentTransactions_UserId");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.PaymentIntentId).HasMaxLength(200);
            entity.Property(e => e.PaymentProvider).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TransactionId).HasMaxLength(200);

            entity.HasOne(d => d.Level).WithMany(p => p.PaymentTransactions).HasForeignKey(d => d.LevelId);

            entity.HasOne(d => d.User).WithMany(p => p.PaymentTransactions).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Slide>(entity =>
        {
            entity.HasIndex(e => e.LessonId, "IX_Slides_LessonId");

            entity.HasIndex(e => e.MasterSlideTypeId, "IX_Slides_MasterSlideTypeId");

            entity.Property(e => e.Points).HasColumnName("points");
            entity.Property(e => e.UrlId).HasColumnName("Url_Id");

            entity.HasOne(d => d.Lesson).WithMany(p => p.Slides).HasForeignKey(d => d.LessonId);

            entity.HasOne(d => d.MasterSlideType).WithMany(p => p.Slides).HasForeignKey(d => d.MasterSlideTypeId);
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.ToTable("units");

            entity.HasIndex(e => e.LevelId, "IX_units_LevelId");

            entity.Property(e => e.Url).HasDefaultValue("");

            entity.HasOne(d => d.Level).WithMany(p => p.Units).HasForeignKey(d => d.LevelId);
        });

        modelBuilder.Entity<UserAchievement>(entity =>
        {
            entity.HasIndex(e => e.AchievementId, "IX_UserAchievements_AchievementId");

            entity.HasIndex(e => e.UserId, "IX_UserAchievements_UserId");

            entity.HasOne(d => d.Achievement).WithMany(p => p.UserAchievements).HasForeignKey(d => d.AchievementId);

            entity.HasOne(d => d.User).WithMany(p => p.UserAchievements).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<UserActivityLog>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_UserActivityLogs_UserId");

            entity.Property(e => e.ActivityType).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.IpAddress).HasMaxLength(45);
            entity.Property(e => e.UserAgent).HasMaxLength(1000);

            entity.HasOne(d => d.User).WithMany(p => p.UserActivityLogs).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<UserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
        });

        modelBuilder.Entity<UserProgress>(entity =>
        {
            entity.ToTable("UserProgress");

            entity.HasIndex(e => e.LessonId, "IX_UserProgress_LessonId");

            entity.HasIndex(e => e.LevelId, "IX_UserProgress_LevelId");

            entity.HasIndex(e => e.SlideId, "IX_UserProgress_SlideId");

            entity.HasIndex(e => e.UnitId, "IX_UserProgress_UnitId");

            entity.HasIndex(e => e.UserId, "IX_UserProgress_UserId").IsUnique();

            entity.Property(e => e.OverallProgressPercent).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Lesson).WithMany(p => p.UserProgresses).HasForeignKey(d => d.LessonId);

            entity.HasOne(d => d.Level).WithMany(p => p.UserProgresses).HasForeignKey(d => d.LevelId);

            entity.HasOne(d => d.Slide).WithMany(p => p.UserProgresses).HasForeignKey(d => d.SlideId);

            entity.HasOne(d => d.Unit).WithMany(p => p.UserProgresses).HasForeignKey(d => d.UnitId);

            entity.HasOne(d => d.User).WithOne(p => p.UserProgress).HasForeignKey<UserProgress>(d => d.UserId);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId });
        });

        modelBuilder.Entity<UserSlideProgress>(entity =>
        {
            entity.ToTable("UserSlideProgress");

            entity.HasIndex(e => e.SlideId, "IX_UserSlideProgress_SlideId");

            entity.HasIndex(e => e.UserId, "IX_UserSlideProgress_UserId");

            entity.Property(e => e.Answer).HasDefaultValue("");

            entity.HasOne(d => d.Slide).WithMany(p => p.UserSlideProgresses).HasForeignKey(d => d.SlideId);

            entity.HasOne(d => d.User).WithMany(p => p.UserSlideProgresses).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<UserSubscription>(entity =>
        {
            entity.HasIndex(e => e.LevelId, "IX_UserSubscriptions_LevelId");

            entity.HasIndex(e => e.UserId, "IX_UserSubscriptions_UserId");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.PaymentId).HasMaxLength(200);
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.SubscriptionType).HasMaxLength(50);

            entity.HasOne(d => d.Level).WithMany(p => p.UserSubscriptions).HasForeignKey(d => d.LevelId);

            entity.HasOne(d => d.User).WithMany(p => p.UserSubscriptions).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<UserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
