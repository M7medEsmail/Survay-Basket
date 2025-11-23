using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SurvayBacket.Api.Entities.EntitesConfigration;
using System.Net.WebSockets;
using System.Reflection;

namespace SurvayBacket.Api.Persistence
{
    public class ApplicationDbContext (DbContextOptions<ApplicationDbContext> options ,IHttpContextAccessor httpContextAccessor) :
        IdentityDbContext<ApplicationUser>(options)

    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public DbSet<Poll> Polls { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new PollConfigration());
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); // get all configration for classes that implement IEntityTypeConfiguration
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var Entries = ChangeTracker.Entries<AuditableEntity>();

            var CurrentUserId = _httpContextAccessor.HttpContext?.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            foreach (var entire in Entries)
            {
                if (entire.State == EntityState.Added)
                {
                    entire.Property(x=>x.CreatedById).CurrentValue = CurrentUserId;
                }
                else if (entire.State == EntityState.Modified)
                {
                    entire.Property(x => x.UpdatedById).CurrentValue = CurrentUserId;
                    entire.Property(x => x.UpdatedOn).CurrentValue = DateTime.UtcNow;
                }

            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
