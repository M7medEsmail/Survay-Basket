
using SurvayBacket.Api.Abstractions;
using SurvayBacket.Api.Contracts.Polls;
using SurvayBacket.Api.Errors;

namespace SurvayBacket.Api.Services
{
    public class PollService(ApplicationDbContext context) : IPollService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<PollResponse> CreateAsync(PollRequest poll, CancellationToken cancellationToken)
        {
            await _context.Polls.AddAsync(poll.Adapt<Poll>() , cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return poll.Adapt<PollResponse>();
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var poll =await _context.Polls.FindAsync(id);
            if (poll is null)
                return Result.Failure(PollError.PollNotFound);
           _context.Remove(poll);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<IEnumerable<PollResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            var allPolls = await _context.Polls.AsNoTracking().ToListAsync();
            return allPolls.Adapt<IEnumerable<PollResponse>>();
        }

        public async Task<Result<PollResponse>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var poll = await _context.Polls.FindAsync(id);
            return poll is null ? Result.Failure<PollResponse>(PollError.PollNotFound) : Result.Success(poll.Adapt<PollResponse>());

        }
            

        public async Task<Result> UpdateAsync(int id, PollRequest poll, CancellationToken cancellationToken)
        {
            var currentPool =await  _context.Polls.FindAsync(id);
            if (currentPool == null)
            {
                return Result.Failure(PollError.PollNotFound);
            }
            currentPool.Title = poll.Title;
            currentPool.Summary = poll.Summary;
            currentPool.StartAt = poll.StartAt;
            currentPool.EndAt = poll.EndAt;

           await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
    }
