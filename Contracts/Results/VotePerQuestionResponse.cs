namespace SurvayBacket.Api.Contracts.Results
{
    public record VotePerQuestionResponse
     (
        string Question,
        IEnumerable<VotePerAnswerResponse> SelectedAnswers
        );
    
}
