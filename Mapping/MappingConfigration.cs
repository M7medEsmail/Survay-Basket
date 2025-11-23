using SurvayBacket.Api.Contracts.Polls;

namespace SurvayBacket.Api.Mapping
{
    public class MappingConfigration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Poll, PollResponse>()
                .Map(dest => dest.Summary, src => src.Summary);

            config.NewConfig<PollRequest, Poll>()
                .Map(dest => dest.StartAt, src => src.StartAt)
                .Map(dest => dest.EndAt, src => src.EndAt)
                .Map(dest => dest.Summary, src => src.Summary)
                .Map(dest => dest.Title, src => src.Title)
                .Map(dest => dest.IsPublished, src => src.IsPublished);


        }
    }
}
