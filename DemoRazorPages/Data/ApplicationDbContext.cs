using DemoRazorPages.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DemoRazorPages.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id=1,Name="Drama",DisplayOrder=1},
                new Category { Id=2,Name="Comic",DisplayOrder=2},
                new Category { Id=3,Name="Romantic",DisplayOrder=3}
                );
        }
    } 
}
