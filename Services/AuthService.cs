using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using SurvayBacket.Api.Abstractions;
using SurvayBacket.Api.Authentication;
using SurvayBacket.Api.Contracts.Authentication;
using SurvayBacket.Api.Errors;
using SurvayBacket.Api.Helper;
using System.Security.Cryptography;
using System.Text;

namespace SurvayBacket.Api.Services
{
    public class AuthService(UserManager<ApplicationUser > userManager,
        SignInManager<ApplicationUser> signInManager ,
        IJwtProvider jwtProvider,
        ILogger<AuthService> logger ,
        IEmailSender emailSender , 
        IHttpContextAccessor httpContextAccessor) : IAuthService 

    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly IJwtProvider _jwtProvider = jwtProvider;
        private readonly ILogger<AuthService> _logger = logger;
        private readonly IEmailSender _emailSender = emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        public static readonly int RefreshTokenExpireDay = 60;
        public async Task<Result<AuthResponse>> GenerateJwtToken(string email, string password, CancellationToken cancellationToken)
        {

            var user =await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Result.Failure<AuthResponse>(UserError.InvalidCredentials);

            //var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
            //if (!isPasswordValid)
            //    return Result.Failure<AuthResponse>(UserError.InvalidCredentials);
                
            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if(result.Succeeded)
            {

                var (token, expiration) = _jwtProvider.GenerateJwtToken(user);

                var refreshToken = GenerateRefreshToken();
                var refreshTokenExpiryTime = DateTime.UtcNow.AddDays(RefreshTokenExpireDay);

                user.RefreshTokens.Add(new RefreshToken
                {
                    Token = refreshToken,
                    ExpireOn = refreshTokenExpiryTime,
                });

                await _userManager.UpdateAsync(user);
                var resonse = new AuthResponse(user.Id, user.FirstName, user.LastName, user.Email!, token, expiration, refreshToken, refreshTokenExpiryTime);
                return Result.Success(resonse);
            }
            return Result.Failure<AuthResponse>(result.IsNotAllowed ? UserError.EmailNotConfirmed : UserError.InvalidCredentials);

        }
        public async Task<Result<AuthResponse>> GetRefreshToken(string token, string refreshToken, CancellationToken cancellationToken)
        {
            string? userId = _jwtProvider.ValidateToken(token);
            if (userId is null)
                return Result.Failure<AuthResponse>(UserError.UserIsNull);

            var user =await _userManager.FindByIdAsync(userId);
            if (user is null)
                return Result.Failure<AuthResponse>(UserError.UserIsNull);

            RefreshToken? UserRefreshToken = user.RefreshTokens.FirstOrDefault(rt => rt.Token == refreshToken && rt.IsActive);
            if (UserRefreshToken is null)
                return Result.Failure<AuthResponse>(UserError.UserIsNull);

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
            var result= new AuthResponse(user.Id, user.FirstName, user.LastName, user.Email!, newtoken, expiration, NewrefreshToken, refreshTokenExpiryTime);

            return Result.Success(result);
        }
        private static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public async Task<Result> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken)
        {
            var emailIsExist = await _userManager.Users.AnyAsync(u => u.Email == registerRequest.Email, cancellationToken);
            if (emailIsExist)
                return Result.Failure(UserError.DublicateEmail);

            var user =registerRequest.Adapt<ApplicationUser>();
            var result = await _userManager.CreateAsync(user, registerRequest.Password);

            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                _logger.LogInformation($"Confirmation Code is : {code}");
                await SendInformationEmail(user, code);

                return Result.Success();
            }

            var errors = result.Errors.First();
            return Result.Failure(new Error(errors.Code, errors.Description, StatusCodes.Status400BadRequest));
            
        }

        public async Task<Result> SendResetPasswordCode(ForgetPasswordRequest forgetPasswordRequest)
        {
            var user = await _userManager.FindByEmailAsync(forgetPasswordRequest.Email);
            if (user is null)
                return Result.Success(); //we do not want to reveal that the user does not exist
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
          
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            _logger.LogInformation($"Reset Password Code is : {code}");
           await SendResetPasswordEmail(user, code);
            return Result.Success();
        }
     
        public async Task<Result> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordRequest.Email);
            if (user is null || !user.EmailConfirmed)
                return Result.Failure(UserError.InvalidCode);

            IdentityResult result;
            try
            {
                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPasswordRequest.Code));
                result = await _userManager.ResetPasswordAsync(user, code, resetPasswordRequest.NewPassword);
            }
            catch (FormatException)
            {
                result = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
            }

            if(result.Succeeded)
                return Result.Success();
            var errors = result.Errors.First();
            return Result.Failure(new Error(errors.Code, errors.Description, StatusCodes.Status401Unauthorized));

        }
        
        public async Task<Result> ResendConfirmationEmail(ResendInformationEmailRequest resendInformationEmail)
        {
            var user = await _userManager.FindByEmailAsync(resendInformationEmail.Email);
            if (user is null)
                return Result.Failure(UserError.UserIsNull);

            if (user.EmailConfirmed)
                return Result.Failure(UserError.DulicatedConfirmarion);

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            _logger.LogInformation($"Confirmation Code is : {code}");
            await SendInformationEmail(user, code);
            return Result.Success();
        }
        private async Task SendInformationEmail(ApplicationUser user , string code)
        {
            var origin = _httpContextAccessor.HttpContext?.Request.Headers["origin"];
            var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",
                new Dictionary<string, string>
                {
                    {"{{UserName}}" , user.FirstName },
                    {"{{ConfirmUrl}}" , $"{origin}/api/auth/confirm-email?userId={user.Id}&code={code}" }
                });

            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "Survay Basket: Confirm your email", emailBody));
            await Task.CompletedTask;
        }
        private async Task SendResetPasswordEmail(ApplicationUser user, string code)
        {
            var origin = _httpContextAccessor.HttpContext?.Request.Headers["origin"];
            var emailBody = EmailBodyBuilder.GenerateEmailBody("ForgetPassword",
                new Dictionary<string, string>
                {
                    {"{{UserName}}" , user.FirstName },
                    {"{{AppName}}" , "Survay Basket" },
                    {"{{ResetUrl}}" , $"{origin}/api/auth/forget-password?email={user.Email}&code={code}" }
                });

            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "Survay Basket: Change Password ", emailBody));
            await Task.CompletedTask;
        }
        public async Task<Result> ConfirmEmail(ConfirmEmailRequest confirmEmailRequest)
        {
            var user = await _userManager.FindByIdAsync(confirmEmailRequest.UserId);
            if (user is null)
                return Result.Failure(UserError.UserIsNull);

            var EmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            if (EmailConfirmed)
                return Result.Failure(UserError.DulicatedConfirmarion);

            var code = confirmEmailRequest.Code;

            try 
            {
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
               
            }
            catch(FormatException)
            {
                return Result.Failure(UserError.InvalidCode);
            }
            
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return Result.Success();
            }
            var errors = result.Errors.First();
            return Result.Failure(new Error(errors.Code, errors.Description, StatusCodes.Status400BadRequest));

        }
    }
}
 