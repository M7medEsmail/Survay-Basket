namespace SurvayBacket.Api.Contracts.Users
{
    public record UserProfileResponse(
        
        string email,
        string userName,
        string firstName,
        string lastName
        );
}
