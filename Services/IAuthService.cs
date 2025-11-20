using SurvayBacket.Api.Contracts.Authentication;

namespace SurvayBacket.Api.Services
{
    public interface IAuthService
    {
        Task<AuthResponse?> GenerateJwtToken( string email, string password , CancellationToken cancellationToken);
        Task<bool> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken);


    }
}
