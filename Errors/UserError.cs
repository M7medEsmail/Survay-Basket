using SurvayBacket.Api.Abstractions;

namespace SurvayBacket.Api.Errors
{
    public static class UserError
    {
        public static  Error InvalidCredentials => new Error("User.InvalidCredentials", "Invalid Email or Password.", StatusCodes.Status401Unauthorized);
        public static  Error UserIsNull => new Error("User.UserIsNull", "This user not found in this app.", StatusCodes.Status404NotFound);
    }
}
