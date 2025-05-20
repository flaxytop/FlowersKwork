using Flowers.Domain.Entities;
using Flowers.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Flowers.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<FN> FN { get; set; }
    public DbSet<Category> Categories { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure PostgreSQL-specific column types
        modelBuilder.Entity<ApplicationUser>()
            .Property(u => u.Email)
            .HasColumnType("text");

        modelBuilder.Entity<ApplicationUser>()
            .Property(u => u.UserName)
            .HasColumnType("text");

        modelBuilder.Entity<ApplicationUser>()
            .Property(u => u.Role)
            .HasColumnType("text");

        // Configure unique email
        modelBuilder.Entity<ApplicationUser>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}