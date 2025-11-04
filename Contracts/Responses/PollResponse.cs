namespace SurvayBacket.Api.Contracts.Response
{
    public record PollResponse(
         int Id,
         string Title,
         string Summary,       
         bool IsPublished ,
         DateTime StartAt ,
         DateTime EndAt 
            );
   
}
