using SurvayBacket.Api.Abstractions;

namespace SurvayBacket.Api.Errors
{
    public static class UserError
    {
        public static  Error InvalidCredentials => new Error("User.InvalidCredentials", "Invalid Email or Password.", StatusCodes.Status401Unauthorized);
        public static  Error UserIsNull => new Error("User.UserIsNull", "This user not found in this app.", StatusCodes.Status404NotFound);
        public static  Error DublicateEmail => new Error("User.DublicateEmail", "Another user with same email is olready exist.", StatusCodes.Status404NotFound);
        public static  Error EmailNotConfirmed => new Error("User.EmailNotConfirmed", "Email is Not Confirmed.", StatusCodes.Status404NotFound);
        public static  Error InvalidCode => new Error("User.InvalidCode", "InvalidCode.", StatusCodes.Status404NotFound);
        public static  Error DulicatedConfirmarion => new Error("User.EmailNotConfirmed", "Email is Not Confirmed.", StatusCodes.Status404NotFound);
    }
}
