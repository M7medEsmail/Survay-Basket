using Microsoft.AspNetCore.OutputCaching;
using SurvayBacket.Api.Abstractions;
using SurvayBacket.Api.Contracts.Answer;
using SurvayBacket.Api.Contracts.Question;
using SurvayBacket.Api.Errors;

namespace SurvayBacket.Api.Services
{
    public class QuestionService(ApplicationDbContext context , IOutputCacheStore outputCacheStore) : IQuestionService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IOutputCacheStore _outputCacheStore = outputCacheStore;

        public async Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest Request, CancellationToken cancellationToken)
        {
            var pollIsExist = await _context.Polls.AnyAsync(p => p.Id == pollId, cancellationToken);
            if (!pollIsExist)
                return Result.Failure<QuestionResponse>(PollError.PollNotFound);

            var QestionIsExist = await _context.Questions.AnyAsync(q => q.Content == Request.Content && q.PollId == pollId, cancellationToken);
            if (QestionIsExist)
                return Result.Failure<QuestionResponse>(QuestionError.QuestionAlreadyExists);

            var question = Request.Adapt<Question>();

            question.PollId = pollId;
            //Request.Answers.ForEach(answerRequest => question.Answers.Add(new Answer { Content = answerRequest }));

            await _context.Questions.AddAsync(question, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await _outputCacheStore.EvictByTagAsync("OutPutCache", cancellationToken); // use to make cache consistancy


            return Result.Success(question.Adapt<QuestionResponse>());

        }
        public async Task<Result<IEnumerable<QuestionResponse>>> GetAll(int pollId, CancellationToken cancellationToken)
        {
            var pollIsExist = await _context.Polls.AnyAsync(p => p.Id == pollId, cancellationToken);
            if (!pollIsExist)
                return Result.Failure<IEnumerable< QuestionResponse>>(PollError.PollNotFound);

            var question= await _context.Questions
                .AsNoTracking()
                .Where(q => q.PollId == pollId)
                .Include(q => q.Answers)
                //.Select(q => new QuestionResponse
                //(
                //     q.Id,
                //     q.Content,
                //     q.Answers.Select(a => new AnswerResponse(a.Id , a.Content))
                //))
                .ProjectToType<QuestionResponse>()
                .ToListAsync(cancellationToken);

            return Result.Success(question.Adapt<IEnumerable<QuestionResponse>>());
        }
        public async Task<Result<QuestionResponse>> GetByIdAsync(int pollId, int questionId, CancellationToken cancellationToken)
        {
             var question =await _context.Questions
                .AsNoTracking()
                .Where(q => q.PollId == pollId && q.Id == questionId)
                .Include(q => q.Answers)
                .ProjectToType<QuestionResponse>()
                .FirstOrDefaultAsync(cancellationToken);

            return question is null ? Result.Failure<QuestionResponse>(QuestionError.QuestionNotFound) : Result.Success(question);


        }
        public async Task<Result> ToggleStatusAsync(int pollId, int questionId, CancellationToken cancellationToken)
        {
            var question =await _context.Questions
                .SingleOrDefaultAsync(q => q.PollId == pollId && q.Id == questionId);
            if (question is null)
                return Result.Failure<QuestionResponse>(QuestionError.QuestionNotFound);
            question.IsActive = !question.IsActive;
            await _context.SaveChangesAsync(cancellationToken);
            await _outputCacheStore.EvictByTagAsync("OutPutCache", cancellationToken);// use to make cache consistancy

            return Result.Success();

        }
        public async Task<Result> UpdateAsync(int pollId, int questionId, QuestionRequest request, CancellationToken cancellationToken)
        {
            var QuestionIsExist = await _context.Questions
                .AnyAsync(q => q.Content == request.Content && q.PollId == pollId && q.Id != questionId, cancellationToken);

            if (QuestionIsExist)
                return Result.Failure<QuestionResponse>(QuestionError.QuestionAlreadyExists);

            var question = await _context.Questions.Include(q => q.Answers)
                .SingleOrDefaultAsync(q => q.PollId == pollId && q.Id == questionId, cancellationToken);

            if (question is null)
                return Result.Failure<QuestionResponse>(QuestionError.QuestionNotFound);

            question.Content = request.Content;
            var currentQuestion = question.Answers.Select(a => a.Content).ToList();

            // Add new answers
            foreach (var answerRequest in request.Answers)
            {
                if (!currentQuestion.Contains(answerRequest))
                {
                    question.Answers.Add(new Answer { Content = answerRequest });
                }
            }
            question.Answers.ToList().ForEach(answer =>
            {
                answer.IsActive = request.Answers.Contains(answer.Content);
            });

            await _context.SaveChangesAsync(cancellationToken);
            await _outputCacheStore.EvictByTagAsync("OutPutCache", cancellationToken); // use to make cache consistancy

            return Result.Success();
        }
        public async Task<Result<IEnumerable<QuestionResponse>>> GetAvailable(int pollId, string userId, CancellationToken cancellationToken)
        { 
            var hasVoted = await _context.Votes
                .AnyAsync(v => v.PollId == pollId && v.UserId == userId, cancellationToken);

            if (hasVoted)
                return Result.Failure<IEnumerable<QuestionResponse>>(VoteError.VoteAlreadyExists);

            var pollIsExist = await _context.Polls.AnyAsync(p => p.Id == pollId && p.IsPublished && p.StartAt <= DateTime.UtcNow && p.EndAt >= DateTime.UtcNow,  cancellationToken);
            if(!pollIsExist)
                return Result.Failure<IEnumerable<QuestionResponse>>(PollError.PollNotFound);

            var questions = await _context.Questions
                .Where(q => q.PollId == pollId && q.IsActive)
                .Include(q => q.Answers)
                .Select( q => new QuestionResponse
                (
                     q.Id,  
                     q.Content,
                     q.Answers
                        .Where(a => a.IsActive)
                        .Select(a => new AnswerResponse(a.Id, a.Content))
                )).AsNoTracking().ToListAsync(cancellationToken);

            return Result.Success<IEnumerable<QuestionResponse>>(questions);
        }


    }
}
