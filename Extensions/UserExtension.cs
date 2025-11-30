using System.Security.Claims;

namespace SurvayBacket.Api.Extensions
{
    public static class UserExtension
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
