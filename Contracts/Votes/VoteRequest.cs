namespace SurvayBacket.Api.Contracts.Votes
{
    public record VoteRequest(
            
        IEnumerable<VoteAnswerRequest> Answers
        );

}
