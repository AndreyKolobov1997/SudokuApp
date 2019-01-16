using Microsoft.EntityFrameworkCore;
using SudokuApp.Data.Entity;

namespace SudokuApp.Data.Config
{
    public class SudokuAppDbContext : DbContext
    {
        public DbSet<User> User { get; set; }

        public DbSet<Grid> Grid { get; set; }

        public SudokuAppDbContext(DbContextOptions<SudokuAppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(x => x.UserName)
                .IsUnique();
        }
    }
}