using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SurvayBacket.Api.Extensions;
using System.Net.WebSockets;
using System.Reflection;

namespace SurvayBacket.Api.Persistence
{
    public class ApplicationDbContext (DbContextOptions<ApplicationDbContext> options ,IHttpContextAccessor httpContextAccessor) :
        IdentityDbContext<ApplicationUser>(options)

    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public DbSet<Poll> Polls { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<VoteAnswer> VoteAnswers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new PollConfigration());
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); // get all configration for classes that implement IEntityTypeConfiguration

            //Change behavior of delete to restrict instead of cascade delete

            var ForeignKeys = modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()).Where(fq=>fq.DeleteBehavior == DeleteBehavior.Cascade && !fq.IsOwnership);

            foreach (var foreignKey in ForeignKeys)
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var Entries = ChangeTracker.Entries<AuditableEntity>();

            var CurrentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();

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
