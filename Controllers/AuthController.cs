using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SurvayBacket.Api.Abstractions;
using SurvayBacket.Api.Contracts.Authentication;
using LoginRequest = SurvayBacket.Api.Contracts.Authentication.LoginRequest;
using RegisterRequest = SurvayBacket.Api.Contracts.Authentication.RegisterRequest;


namespace SurvayBacket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService ,ILogger<AuthController> logger) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly ILogger<AuthController> _logger = logger;

        [HttpPost("login")]
        public async Task <IActionResult> Login(LoginRequest loginRequest ,CancellationToken cancellationToken)
        {
            _logger.LogInformation("Login Attempt for {Email}", loginRequest.Email);
            var AuthRequest =  await _authService.GenerateJwtToken(loginRequest.Email, loginRequest.Password, cancellationToken);

            return AuthRequest.IsSuccess? Ok(AuthRequest) :AuthRequest.ToProblem();

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest, CancellationToken cancellationToken)
        {       
            var result = await _authService.RegisterAsync(registerRequest, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody]ConfirmEmailRequest confirmEmailRequest)
        {
            var result = await _authService.ConfirmEmail(confirmEmailRequest);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }

        [HttpPost("resend-confirmation-email")]
        public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendInformationEmailRequest resendInformationEmailRequest)
        {
            var result = await _authService.ResendConfirmationEmail(resendInformationEmailRequest);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest forgetPasswordRequest)
        {
            var result = await _authService.SendResetPasswordCode(forgetPasswordRequest);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] Contracts.Authentication.ResetPasswordRequest resetPasswordRequest)
        {
            var result = await _authService.ResetPasswordAsync(resetPasswordRequest);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }

    }
}
