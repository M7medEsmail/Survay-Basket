namespace SurvayBacket.Api.Contracts.Polls
{
    public record PollResponse(
         string Title,
         string Summary,       
         bool IsPublished ,
         DateTime StartAt ,
         DateTime EndAt 
            );
   
}
