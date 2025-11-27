using SurvayBacket.Api.Abstractions;
using SurvayBacket.Api.Contracts.Question;

namespace SurvayBacket.Api.Services
{
    public interface IQuestionService
    {
        Task<Result<QuestionResponse>> AddAsync(int pollId,QuestionRequest question, CancellationToken cancellationToken);

        Task<Result<IEnumerable<QuestionResponse>>> GetAll(int pollId, CancellationToken cancellationToken);    

        Task<Result<QuestionResponse>> GetByIdAsync(int pollId, int questionId, CancellationToken cancellationToken);
        Task<Result> ToggleStatusAsync(int pollId, int questionId, CancellationToken cancellationToken);



    }
}
