using DocumentAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options)
        {
        }
        public DbSet<Documents> Document { get; set; } 
    }
}
