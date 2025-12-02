using SurvayBacket.Api.Abstractions;
using SurvayBacket.Api.Contracts.Results;
using SurvayBacket.Api.Errors;

namespace SurvayBacket.Api.Services
{
    public class ResultService(ApplicationDbContext context) : IResultService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result<PollVoteResponse>> GetPollVoteAsync(int pollId, CancellationToken cancellationToken)
        {
           
            var pollVote = await _context.Polls
                .Where( p => p.Id == pollId)
                .Select(x=>  new PollVoteResponse(
                
                    x.Title,
                    x.Votes.Select (v=> new VoteResponse(
                        $"{v.User.FirstName} {v.User.LastName}",    
                        v.SubmittedOn,
                        v.VoteAnswers.Select(q=> new QuestionAnswerResponse(
                            q.Question.Content,
                            q.Answer.Content
                        ))
                     ))
                 )).SingleOrDefaultAsync(cancellationToken);


            return pollVote is null ? Result.Failure<PollVoteResponse>(PollError.PollNotFound) : Result.Success(pollVote);

        }

        public async Task<Result<IEnumerable<VotePerDayResponse>>> GetVotePerDay(int pollId, CancellationToken cancellationToken)
        {
            var isExist = await _context.Polls.AnyAsync(p => p.Id == pollId, cancellationToken);
            if (!isExist)
                return Result.Failure<IEnumerable<VotePerDayResponse>>(PollError.PollNotFound);

            var votePerDay = await _context.Votes
                .Where(v => v.PollId == pollId)
                .GroupBy(v => new { Date = DateOnly.FromDateTime(v.SubmittedOn)})
                .Select(g => new VotePerDayResponse(
                    g.Key.Date,
                    g.Count()
                ))
                .ToListAsync(cancellationToken);
            return Result.Success<IEnumerable<VotePerDayResponse>>(votePerDay);
        }


        public async Task<Result<IEnumerable<VotePerQuestionResponse>>> GetVotePerQuestion(int pollId, CancellationToken cancellationToken)
        {
            var isExist = await _context.Polls.AnyAsync(p => p.Id == pollId, cancellationToken);
            if (!isExist)
                return Result.Failure<IEnumerable<VotePerQuestionResponse>>(PollError.PollNotFound);

            var votePerQuestion = await _context.VoteAnswers
                .Where(va => va.Vote.PollId == pollId)
                .Select(g => new VotePerQuestionResponse(
                    g.Question.Content,
                    g.Question.VoteAnswers
                        .GroupBy(va =>  new {AnswerId = va.AnswerId, AnswerContent = va.Answer.Content})
                        .Select(ag => new VotePerAnswerResponse(
                            ag.Key.AnswerContent,
                            ag.Count()
                        ))
                ))
                .ToListAsync(cancellationToken);

            return Result.Success<IEnumerable<VotePerQuestionResponse>>(votePerQuestion);

        }

    }
}
