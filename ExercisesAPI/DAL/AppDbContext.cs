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

    }
}