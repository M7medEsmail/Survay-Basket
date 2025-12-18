using SurvayBacket.Api.Contracts.Authentication;
using SurvayBacket.Api.Contracts.Polls;
using SurvayBacket.Api.Contracts.Question;

namespace SurvayBacket.Api.Mapping
{
    public class MappingConfigration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<QuestionRequest, Question>()
                .Map(dest => dest.Answers, src => src.Answers.Select(answer => new Answer{ Content = answer }));
                    //.Ignore(QuestionRequest => QuestionRequest.Answers);

            config.NewConfig<Poll, PollResponse>()
                .Map(dest => dest.Summary, src => src.Summary);

            config.NewConfig<PollRequest, Poll>()   
                .Map(dest => dest.StartAt, src => src.StartAt)
                .Map(dest => dest.EndAt, src => src.EndAt)
                .Map(dest => dest.Summary, src => src.Summary)
                .Map(dest => dest.Title, src => src.Title)
                .Map(dest => dest.IsPublished, src => src.IsPublished);

            config.NewConfig<RegisterRequest, ApplicationUser>()
                .Map(dest => dest.UserName, src => src.FirstName + src.LastName);

        }
    }
}
