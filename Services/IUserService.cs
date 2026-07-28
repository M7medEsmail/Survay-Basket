using SurvayBacket.Api.Abstractions;
using SurvayBacket.Api.Contracts;
using SurvayBacket.Api.Contracts.Users;

namespace SurvayBacket.Api.Services
{
    public interface IUserService
    {
        Task<Result<UserProfileResponse>> GetProfileAsync(string userId);
        Task<Result> UpdateProfileInfo(string userId, UpdateProfileRequest request);
        Task<Result> ChangePasswordAsync(string UserId, ChangePasswordRequest request);
    }
}
