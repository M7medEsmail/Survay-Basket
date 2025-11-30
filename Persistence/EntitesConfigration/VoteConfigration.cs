
namespace SurvayBacket.Api.Persistence.EntitesConfigration
{
    public class VoteConfigration : IEntityTypeConfiguration<Vote>
    {
        public void Configure(EntityTypeBuilder<Vote> builder)
        {
            builder.HasIndex(a => new {a.PollId , a.UserId})
                .IsUnique();


        }
    }
}
