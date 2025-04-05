using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Domain.Models;

namespace WebApplication1.DataAccess.DataContext
{
    public class PollDbContext : IdentityDbContext
    {
        public PollDbContext(DbContextOptions<PollDbContext> options)
            : base(options)
        {
        }

        public DbSet<Poll> Polls { get; set; } // tes
        public DbSet<Vote> Votes { get; set; } 
    }
}
