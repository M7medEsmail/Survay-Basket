namespace SurvayBacket.Api.Entities
{
    public class VoteAnswer
    {
        public int Id { get; set; }
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public int VoteId { get; set; }
        public Answer Answer { get; set; } = null!;
        public Question Question { get; set; } = null!;
        public Vote Vote { get; set; } = null!;


    }
}
