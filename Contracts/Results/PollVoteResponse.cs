namespace SurvayBacket.Api.Contracts.Results
{
    public record PollVoteResponse
     (
        string Title,
        IEnumerable<VoteResponse> Votes
     );
}
