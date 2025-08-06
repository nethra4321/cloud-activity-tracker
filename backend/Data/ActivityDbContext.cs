using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data
{
    public class ActivityDbContext : DbContext
    {
        public ActivityDbContext(DbContextOptions<ActivityDbContext> options) : base(options) { }

        public DbSet<ActivityEvent> ActivityEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActivityEvent>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd(); 
        }
        
    }
}
