using SurvayBacket.Api.Abstractions;

namespace SurvayBacket.Api.Errors
{
    public static class UserError
    {
        public static  Error InvalidCredentials => new Error("User.InvalidCredentials", "Invalid Email or Password.");
    }
}
