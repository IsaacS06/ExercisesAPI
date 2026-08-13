using Microsoft.EntityFrameworkCore;
using ExercisesAPI.DAL.DomainClasses;

namespace ExercisesAPI.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public virtual DbSet<User>? Users { get; set; }
        public virtual DbSet<MenuItem>? MenuItems { get; set; }
        public virtual DbSet<Category>? Categories { get; set; }

        public virtual DbSet<Tray>? Trays { get; set; }
        public virtual DbSet<TrayItem>? TrayItems { get; set; }
        public virtual DbSet<RestaurantBranch>? RestaurantBranches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MenuItem>()
                .HasOne(m => m.Category)
                .WithMany(c => c.MenuItems)
                .HasForeignKey(m => m.CategoryId);

            modelBuilder.Entity<Tray>()
                .HasOne(t => t.User)
                .WithMany(u => u.Trays)
                .HasForeignKey(t => t.UserID);

            modelBuilder.Entity<TrayItem>()
                .HasOne(ti => ti.Tray)
                .WithMany(t => t.TrayItems)
                .HasForeignKey(ti => ti.TrayId);

            modelBuilder.Entity<TrayItem>()
                .HasOne(ti => ti.MenuItem)
                .WithMany(m => m.TrayItems)
                .HasForeignKey(ti => ti.MenuItemId);

            // Configure the RestaurantBranch entity
            modelBuilder.Entity<RestaurantBranch>(entity =>
            {
                entity.ToTable("RestaurantBranches");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Latitude)
                    .HasPrecision(10, 8);

                entity.Property(e => e.Longitude)
                    .HasPrecision(11, 8);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20);

                entity.Property(e => e.OpeningHours)
                    .HasMaxLength(100);
            });

            // Seed branch data (London, ON locations)
            modelBuilder.Entity<RestaurantBranch>().HasData(
                new RestaurantBranch
                {
                    Id = 1,
                    Name = "Downtown Branch",
                    Address = "201 King Street, London, ON",
                    Latitude = 42.9832m,
                    Longitude = -81.2492m,
                    PhoneNumber = "+1-519-555-0101",
                    OpeningHours = "Mon-Sun 10:00 AM - 10:00 PM",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new RestaurantBranch
                {
                    Id = 2,
                    Name = "Masonville Branch",
                    Address = "1680 Richmond St N, London, ON",
                    Latitude = 43.0200m,
                    Longitude = -81.2770m,
                    PhoneNumber = "+1-519-555-0102",
                    OpeningHours = "Mon-Sun 10:00 AM - 10:00 PM",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new RestaurantBranch
                {
                    Id = 3,
                    Name = "White Oaks Branch",
                    Address = "1105 Wellington Rd, London, ON",
                    Latitude = 42.9440m,
                    Longitude = -81.2300m,
                    PhoneNumber = "+1-519-555-0103",
                    OpeningHours = "Mon-Sun 10:00 AM - 10:00 PM",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new RestaurantBranch
                {
                    Id = 4,
                    Name = "Hyde Park Branch",
                    Address = "1265 Hyde Park Rd, London, ON",
                    Latitude = 43.0080m,
                    Longitude = -81.3320m,
                    PhoneNumber = "+1-519-555-0104",
                    OpeningHours = "Mon-Sun 10:00 AM - 10:00 PM",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new RestaurantBranch
                {
                    Id = 5,
                    Name = "Argyle Branch",
                    Address = "1249 Oxford St E, London, ON",
                    Latitude = 42.9910m,
                    Longitude = -81.1890m,
                    PhoneNumber = "+1-519-555-0105",
                    OpeningHours = "Mon-Sun 10:00 AM - 10:00 PM",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new RestaurantBranch
                {
                    Id = 6,
                    Name = "Westmount Branch",
                    Address = "785 Wonderland Rd S, London, ON",
                    Latitude = 42.9560m,
                    Longitude = -81.2870m,
                    PhoneNumber = "+1-519-555-0106",
                    OpeningHours = "Mon-Sun 10:00 AM - 10:00 PM",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            );
        }

    }
}