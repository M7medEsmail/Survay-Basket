using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurvayBacket.Api.Contracts;
using SurvayBacket.Api.Contracts.Users;
using SurvayBacket.Api.Extensions;

namespace SurvayBacket.Api.Controllers
{
    [Route("me")]
    [ApiController]
    [Authorize]
    public class AccountController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        [HttpGet]
        public async Task<IActionResult> info()
        {
           var result = await _userService.GetProfileAsync(User.GetUserId()!);
            return Ok(result.Value);
        }

        [HttpPut("info")]
        public async Task<IActionResult> UpdateInfo([FromBody] UpdateProfileRequest request)
        {
            var result = await _userService.UpdateProfileInfo(User.GetUserId()!, request);
            if (result.IsFailure)
                return BadRequest(result.Error);
            return NoContent();
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var result = await _userService.ChangePasswordAsync(User.GetUserId()!, request);
            if (result.IsFailure)
                return BadRequest(result.Error);
            return NoContent();
        }

    }
}
