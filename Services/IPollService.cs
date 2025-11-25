using SurvayBacket.Api.Abstractions;
using SurvayBacket.Api.Contracts.Polls;
using System.Collections.Generic;
using System.Threading;

namespace SurvayBacket.Api.Services
{
    public interface IPollService
    {
        Task<IEnumerable<PollResponse>> GetAllAsync(CancellationToken cancellationToken);
        Task<Result<PollResponse>> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Result<PollResponse>> CreateAsync(PollRequest poll , CancellationToken cancellationToken);
        Task<Result> UpdateAsync(int id ,PollRequest poll , CancellationToken cancellationToken);
        Task<Result> DeleteAsync(int id , CancellationToken cancellationToken);

    }
}
