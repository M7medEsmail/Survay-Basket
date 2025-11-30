using System.Reflection.Metadata.Ecma335;

namespace SurvayBacket.Api.Entities
{
    public sealed class Vote
    {
        public int Id { get; set; }
        public int PollId { get; set; } = default!;
        public Poll Poll { get; set; } = default!;
        public string UserId { get; set; }
        public ApplicationUser User { get; set; } = default!;
        public DateTime SubmittedOn { get; set; } = DateTime.UtcNow;
        public ICollection<VoteAnswer> VoteAnswers { get; set; } = [];
    }
}
