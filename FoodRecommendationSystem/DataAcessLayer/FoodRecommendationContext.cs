﻿using Microsoft.EntityFrameworkCore.Design;

namespace DataAcessLayer;

public partial class FoodRecommendationContext : DbContext
{
    public FoodRecommendationContext(DbContextOptions<FoodRecommendationContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Food> Foods { get; set; }
    public DbSet<Meal> Meals { get; set; }
    public DbSet<SummaryRating> SummaryRatings { get; set; }
    public DbSet<MealMenu> MealMenus { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<MealName> MealNames { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.Reviews)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Ratings)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId);

        modelBuilder.Entity<Food>()
            .HasMany(f => f.Reviews)
            .WithOne(r => r.Food)
            .HasForeignKey(r => r.FoodId);

        modelBuilder.Entity<Food>()
            .HasMany(f => f.Ratings)
            .WithOne(r => r.Food)
            .HasForeignKey(r => r.FoodId);

        modelBuilder.Entity<Food>()
            .HasMany(f => f.MealPlans)
            .WithOne(mp => mp.Food)
            .HasForeignKey(mp => mp.FoodId);

        modelBuilder.Entity<Food>()
            .HasOne(f => f.SummaryRating)
            .WithOne(sr => sr.Food)
            .HasForeignKey<SummaryRating>(sr => sr.FoodId);

        modelBuilder.Entity<UserNotification>()
            .HasKey(un => new { un.UserId, un.NotificationId });

        modelBuilder.Entity<UserNotification>()
            .HasOne(un => un.User)
            .WithMany(u => u.UserNotifications)
            .HasForeignKey(un => un.UserId);

        modelBuilder.Entity<UserNotification>()
            .HasOne(un => un.Notification)
            .WithMany(n => n.UserNotifications)
            .HasForeignKey(un => un.NotificationId);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

public class FoodRecommendationContextFactory : IDesignTimeDbContextFactory<FoodRecommendationContext>
{
    public FoodRecommendationContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<FoodRecommendationContext>();
        optionsBuilder.UseSqlServer(@"Server=.;Database=FoodRecommendationSystem;Trusted_Connection=True;");

        return new FoodRecommendationContext(optionsBuilder.Options);
    }
}