using Microsoft.AspNetCore.Identity;
using SurvayBacket.Api.Abstractions;
using SurvayBacket.Api.Contracts;
using SurvayBacket.Api.Contracts.Users;

namespace SurvayBacket.Api.Services
{
    public class UserService(UserManager<ApplicationUser> userManager) :IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<Result<UserProfileResponse>> GetProfileAsync(string userId)
        {
            var user = await _userManager.Users  //we can use _userManger.Users instead of dbContext because it return IQueryable<ApplicationUser>
                .Where(u => u.Id == userId)
                .ProjectToType<UserProfileResponse>()
                .SingleOrDefaultAsync();    

            return Result.Success(user!);
        }

        public async Task<Result> UpdateProfileInfo(string userId, UpdateProfileRequest request)
        {
            var user = await _userManager.FindByIdAsync(userId);
            user = request.Adapt(user);

            await _userManager.UpdateAsync(user!);
            return Result.Success();
        }

        public async Task<Result> ChangePasswordAsync(string UserId , ChangePasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            var result = await _userManager.ChangePasswordAsync(user!, request.CurrentPassword, request.NewPassword);
            if (result.Succeeded)
                return Result.Success();
            var errors = result.Errors.First();
            return Result.Failure(new Error(errors.Code, errors.Description, StatusCodes.Status400BadRequest));
        }

    }
}
