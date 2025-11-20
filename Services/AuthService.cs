using Microsoft.AspNetCore.Identity;
using SurvayBacket.Api.Authentication;
using SurvayBacket.Api.Contracts.Authentication;

namespace SurvayBacket.Api.Services
{
    public class AuthService(UserManager<ApplicationUser > userManager , IJwtProvider jwtProvider) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IJwtProvider _jwtProvider = jwtProvider;

        public async Task<AuthResponse?> GenerateJwtToken(string email, string password, CancellationToken cancellationToken)
        {

            var user =await _userManager.FindByEmailAsync(email);
            if (user is null)
                return null;

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
            if (!isPasswordValid)
                return null;

            var (token, expiration) = _jwtProvider.GenerateJwtToken(user);

            return new AuthResponse(user.Id,user.FirstName ,user.LastName,user.Email!,token , expiration);

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
