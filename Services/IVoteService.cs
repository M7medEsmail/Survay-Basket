using SurvayBacket.Api.Abstractions;
using SurvayBacket.Api.Contracts.Votes;

namespace SurvayBacket.Api.Services
{
    public interface IVoteService
    {
        Task<Result> VoteAsync(int pollId, string userId, VoteRequest voteRequest,CancellationToken cancellationToken);
    }
}
