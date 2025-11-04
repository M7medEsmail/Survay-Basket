using System.Collections.Generic;
using System.Threading;

namespace SurvayBacket.Api.Services
{
    public interface IPollService
    {
        Task<IEnumerable<Poll>> GetAllAsync(CancellationToken cancellationToken);
        Task<Poll> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Poll> CreateAsync(Poll poll , CancellationToken cancellationToken);
        Task<bool> UpdateAsync(int id ,Poll poll , CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id , CancellationToken cancellationToken);

    }
}
