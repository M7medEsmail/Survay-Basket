using SurvayBacket.Api.Abstractions;
using SurvayBacket.Api.Contracts.Question;
using SurvayBacket.Api.Contracts.Votes;
using SurvayBacket.Api.Entities;
using SurvayBacket.Api.Errors;

namespace SurvayBacket.Api.Services
{
    public class VoteService(ApplicationDbContext context) : IVoteService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result> VoteAsync(int pollId, string userId, VoteRequest voteRequest, CancellationToken cancellationToken)
        {
            var hasVoted = await _context.Votes
                           .AnyAsync(v => v.PollId == pollId && v.UserId == userId, cancellationToken);

            if (hasVoted)
                return Result.Failure(VoteError.VoteAlreadyExists);

            var pollIsExist = await _context.Polls.AnyAsync(p => p.Id == pollId && p.IsPublished && p.StartAt <= DateTime.UtcNow && p.EndAt >= DateTime.UtcNow, cancellationToken);
            if (!pollIsExist)
                return Result.Failure(PollError.PollNotFound);

            var availableQuestionIds = await _context.Questions
                                        .Where(q => q.PollId == pollId &&q.IsActive)
                                        .Select(q => q.Id)
                                        .ToListAsync(cancellationToken);
            
            if(!voteRequest.Answers.Select(x=>x.QuestionId).SequenceEqual(availableQuestionIds))               
                return Result.Failure(VoteError.InvalidQuestion);
            
            var vote = new Vote
            {
                PollId = pollId,
                UserId = userId,
                VoteAnswers = voteRequest.Answers.Adapt<IEnumerable<VoteAnswer>>().ToList()
            };

            await _context.Votes.AddAsync(vote, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
