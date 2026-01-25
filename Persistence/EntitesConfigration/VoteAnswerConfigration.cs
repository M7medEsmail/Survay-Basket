
namespace SurvayBacket.Api.Persistence.EntitesConfigration
{
    public class VoteAnswerConfigration : IEntityTypeConfiguration<VoteAnswer>
    {
        public void Configure(EntityTypeBuilder<VoteAnswer> builder)
        {
            builder.HasIndex(a => new {a.QuestionId , a.VoteId})
                .IsUnique();

        }
    }
}
