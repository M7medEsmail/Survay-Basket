using Microsoft.AspNetCore.Identity;

namespace SurvayBacket.Api.Entities
{
    public sealed class ApplicationUser :IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
