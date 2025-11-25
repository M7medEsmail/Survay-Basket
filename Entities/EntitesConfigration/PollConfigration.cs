
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SurvayBacket.Api.Entities.EntitesConfigration
{
    public class PollConfigration : IEntityTypeConfiguration<Poll>
    {
        public void Configure(EntityTypeBuilder<Poll> builder)
        {
            builder.HasIndex(x => x.Title).IsUnique();
            builder.Property(x => x.Summary).HasMaxLength(1000);
            builder.Property(x => x.Title).HasMaxLength(100);
        }
    }
}
