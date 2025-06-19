using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using YoussefWebRazor_Temp.Models;

namespace YoussefWebRazor_Temp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryID = 1, Name = "Action", DisplayOrder = 1 },
                new Category { CategoryID = 2, Name = "SciFi", DisplayOrder = 2 },
                new Category { CategoryID = 3, Name = "History", DisplayOrder = 3 }
            );
        }
    }
}
