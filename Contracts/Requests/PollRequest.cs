namespace SurvayBacket.Api.Contracts.Request
{
    public record PollRequest(
         string Title,
         string Summary,
         bool IsPublished,
         DateTime StartAt,
         DateTime EndAt
        );

}
