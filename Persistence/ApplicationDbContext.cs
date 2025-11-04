using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SurvayBacket.Api.Entities.EntitesConfigration;
using System.Reflection;

namespace SurvayBacket.Api.Persistence
{
    public class ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) :
        IdentityDbContext<ApplicationUser>(options)

    {
        public DbSet<Poll> Polls { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new PollConfigration());
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); // get all configration for classes that implement IEntityTypeConfiguration
            base.OnModelCreating(modelBuilder);
        }

    }
}
