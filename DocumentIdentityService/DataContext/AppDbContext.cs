using DocumentIdentityService.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DocumentIdentityService.DataContext
{
    public class AppDbContext: IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Family> FamilyAdmins { get; set; }
    }
}
