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
            var user = new ApplicationUser
            {
                UserName = registerRequest.Email,
                Email = registerRequest.Email,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName

            };
            var result = await _authService.RegisterAsync(registerRequest, cancellationToken);
            return Ok(registerRequest);
        }

    }
}
