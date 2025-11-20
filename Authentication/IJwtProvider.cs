using Microsoft.Extensions.Options;

namespace SurvayBacket.Api.Authentication
{
    public interface IJwtProvider
    {
        (string Token, int Expiration) GenerateJwtToken(ApplicationUser user);

        string ValidateToken(string token); 
    }
}
