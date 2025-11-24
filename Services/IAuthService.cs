using SurvayBacket.Api.Abstractions;
using SurvayBacket.Api.Contracts.Authentication;

namespace SurvayBacket.Api.Services
{
    public interface IAuthService
    {
        Task<Result<AuthResponse>> GenerateJwtToken( string email, string password , CancellationToken cancellationToken);
        Task<AuthResponse?> GetRefreshToken( string token, string refreshToken , CancellationToken cancellationToken);
        Task<bool> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken);


    }
}
