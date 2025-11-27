using SurvayBacket.Api.Abstractions;
using SurvayBacket.Api.Contracts.Answer;
using SurvayBacket.Api.Contracts.Question;
using SurvayBacket.Api.Errors;

namespace SurvayBacket.Api.Services
{
    public class QuestionService(ApplicationDbContext context) : IQuestionService
    {
        private readonly ApplicationDbContext _context = context;

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
            return Result.Success();

        }
    }
}
