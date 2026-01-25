namespace SurvayBacket.Api.Contracts.Results
{
    public record VoteResponse
    (
        string VoterName,
        DateTime VoteDate,
        IEnumerable<QuestionAnswerResponse> SelectedAnswer

        );
}
