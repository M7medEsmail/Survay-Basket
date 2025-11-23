namespace SurvayBacket.Api.Contracts.Polls
{
    public record PollRequest(
         string Title,
         string Summary,
         bool IsPublished,
         DateTime StartAt,
         DateTime EndAt
        );

}
