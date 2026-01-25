using SurvayBacket.Api.Abstractions;
using SurvayBacket.Api.Contracts.Results;

namespace SurvayBacket.Api.Services
{
    public interface IResultService
    {
        Task<Result<PollVoteResponse>> GetPollVoteAsync(int pollId, CancellationToken cancellationToken);
        Task<Result<IEnumerable<VotePerDayResponse>>> GetVotePerDay(int pollId, CancellationToken cancellationToken);
        Task<Result<IEnumerable<VotePerQuestionResponse>>> GetVotePerQuestion(int pollId, CancellationToken cancellationToken);

    }
}
