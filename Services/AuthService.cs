using Microsoft.AspNetCore.Identity;
using SurvayBacket.Api.Abstractions;
using SurvayBacket.Api.Authentication;
using SurvayBacket.Api.Contracts.Authentication;
using SurvayBacket.Api.Errors;
using System.Security.Cryptography;

namespace SurvayBacket.Api.Services
{
    public class AuthService(UserManager<ApplicationUser > userManager , IJwtProvider jwtProvider) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IJwtProvider _jwtProvider = jwtProvider;
        public static readonly int RefreshTokenExpireDay = 60;
        public async Task<Result<AuthResponse>> GenerateJwtToken(string email, string password, CancellationToken cancellationToken)
        {

            var user =await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Result.Failure<AuthResponse>(UserError.InvalidCredentials);

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
            if (!isPasswordValid)
                return Result.Failure<AuthResponse>(UserError.InvalidCredentials);

            var (token, expiration) = _jwtProvider.GenerateJwtToken(user);

            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiryTime = DateTime.UtcNow.AddDays(RefreshTokenExpireDay);

            user.RefreshTokens.Add(new RefreshToken{
                Token = refreshToken,
                ExpireOn = refreshTokenExpiryTime,

            });

            await _userManager.UpdateAsync(user);
            var resonse = new AuthResponse(user.Id, user.FirstName, user.LastName, user.Email!, token, expiration, refreshToken, refreshTokenExpiryTime);
            return Result.Success(resonse);

        }
        public async Task<AuthResponse?> GetRefreshToken(string token, string refreshToken, CancellationToken cancellationToken)
        {
            string? userId = _jwtProvider.ValidateToken(token);
            if (userId is null)
                return null;

            var user =await _userManager.FindByIdAsync(userId);
            if (user is null)
                return null;

            var UserRefreshToken = user.RefreshTokens.FirstOrDefault(rt => rt.Token == refreshToken && rt.IsActive);
            if (UserRefreshToken is null)
                return null;

            UserRefreshToken.RevokedOn = DateTime.UtcNow;

            var (newtoken, expiration) = _jwtProvider.GenerateJwtToken(user);
            var NewrefreshToken = GenerateRefreshToken();
            var refreshTokenExpiryTime = DateTime.UtcNow.AddDays(RefreshTokenExpireDay);

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = NewrefreshToken,
                ExpireOn = refreshTokenExpiryTime,

            });

            await _userManager.UpdateAsync(user);
            return new AuthResponse(user.Id, user.FirstName, user.LastName, user.Email!, newtoken, expiration, NewrefreshToken, refreshTokenExpiryTime);

        }
        private static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public async Task<bool> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
                UserName = registerRequest.Email,
                Email = registerRequest.Email,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName
            };
            await _userManager.CreateAsync(user, registerRequest.Password);
            return true;
        }


    }
}
 