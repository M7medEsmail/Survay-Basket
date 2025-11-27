using SurvayBacket.Api.Contracts.Answer;

namespace SurvayBacket.Api.Contracts.Question
{
    public record QuestionResponse
        (
         int Id,
         string Content,
        IEnumerable<AnswerResponse> Answers
        );

}
