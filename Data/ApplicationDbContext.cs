using Microsoft.EntityFrameworkCore;
using EquipShare.Models;
using Microsoft.Extensions.Logging;

namespace EquipShare.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Booking> Bookings { get; set; } // Added missing DbSet

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships and constraints
            modelBuilder.Entity<Equipment>()
                .HasOne(e => e.Owner)
                .WithMany(u => u.Equipment)
                .HasForeignKey(e => e.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Equipment>()
                .HasOne(e => e.Category)
                .WithMany(c => c.Equipment)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Booking relationships
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Equipment)
                .WithMany()
                .HasForeignKey(b => b.EquipmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Renter)
                .WithMany()
                .HasForeignKey(b => b.RenterId)
                .OnDelete(DeleteBehavior.Restrict);


            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
                entity.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.LastName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.PhoneNumber).HasMaxLength(20);
            });

            // Configure Equipment entity
            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Location).HasMaxLength(500);
                entity.Property(e => e.ImageUrl).HasMaxLength(500);
                entity.Property(e => e.PricePerDay).HasColumnType("decimal(18,2)");
            });

            // Configure Category entity
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Description).HasMaxLength(500);
            });

            // Configure Booking entity
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.Property(b => b.Status).IsRequired().HasMaxLength(20);
                entity.Property(b => b.TotalPrice).HasColumnType("decimal(18,2)");
            });


            // Seed initial categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Construction & Tools", Description = "Construction equipment and tools for building and renovation" },
                new Category { Id = 2, Name = "Outdoor & Gardening", Description = "Outdoor equipment, gardening tools, and landscaping supplies" },
                new Category { Id = 3, Name = "Vehicles & Transport", Description = "Vehicles, trailers, and transportation equipment" },
                new Category { Id = 4, Name = "Electronics & Tech", Description = "Electronic devices, computers, and technology equipment" },
                new Category { Id = 5, Name = "Events & Party Supplies", Description = "Equipment and supplies for events, parties, and celebrations" },
                new Category { Id = 6, Name = "Sports & Recreation", Description = "Sports equipment and recreational gear" },
                new Category { Id = 7, Name = "Agriculture & Farming", Description = "Agricultural machinery and farming equipment" },
                new Category { Id = 8, Name = "Miscellaneous", Description = "Other equipment and tools that don't fit into specific categories" }
            );
        }
    }
}