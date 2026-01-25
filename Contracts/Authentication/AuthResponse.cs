namespace SurvayBacket.Api.Contracts.Authentication
{
    public record AuthResponse
    (
        string id ,
        string FirstName,
        string LastName,
        string Email,
        string Token,
        int ExpireInMinutes,
        string RefreshToken,
        DateTime RefreshTokenExpireOn
        );
}
