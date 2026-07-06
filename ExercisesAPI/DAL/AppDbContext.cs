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
        }

    }
}