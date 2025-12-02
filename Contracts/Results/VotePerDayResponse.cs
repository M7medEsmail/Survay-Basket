namespace SurvayBacket.Api.Contracts.Results
{
    public record VotePerDayResponse
    (
        DateOnly Date,
        int NumberOfVotes
     );
}
