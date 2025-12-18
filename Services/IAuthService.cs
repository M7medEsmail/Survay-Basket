using SurvayBacket.Api.Abstractions;
using SurvayBacket.Api.Contracts.Authentication;

namespace SurvayBacket.Api.Services
{
    public interface IAuthService
    {
        Task<Result<AuthResponse>> GenerateJwtToken( string email, string password , CancellationToken cancellationToken);
        Task<Result<AuthResponse>> GetRefreshToken( string token, string refreshToken , CancellationToken cancellationToken);
        Task<Result> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken);
        Task<Result> ConfirmEmail(ConfirmEmailRequest confirmEmailRequest);
        Task<Result> ResendConfirmationEmail(ResendInformationEmailRequest resendInformationEmail);




    }
}
