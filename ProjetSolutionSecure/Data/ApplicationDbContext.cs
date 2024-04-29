using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjetSolutionSecure.Models;

namespace ProjetSolutionSecure.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ProjetSolutionSecure.Models.Voiture> Voiture { get; set; } = default!;
        public DbSet<Product> Product { get; set; } = default!;
    }
}
