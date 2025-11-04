
namespace SurvayBacket.Api.Services
{
    public class PollService(ApplicationDbContext context) : IPollService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Poll> CreateAsync(Poll poll, CancellationToken cancellationToken)
        {
            await _context.Polls.AddAsync(poll , cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return poll;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var poll =await GetByIdAsync(id,cancellationToken);
            if (poll is null)
                return false;
           _context.Remove(poll);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<IEnumerable<Poll>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Polls.AsNoTracking().ToListAsync();
        }

        public async Task<Poll> GetByIdAsync(int id, CancellationToken cancellationToken) =>
            await _context.Polls.FindAsync(id);

        public async Task<bool> UpdateAsync(int id, Poll poll, CancellationToken cancellationToken)
        {
            var currentPool =await GetByIdAsync(id , cancellationToken);
            if (currentPool == null)
            {
                return false;
            }
            ;
            currentPool.Title = poll.Title;
            currentPool.Summary = poll.Summary;
            currentPool.StartAt = poll.StartAt;
            currentPool.EndAt = poll.EndAt;

           await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
    }
