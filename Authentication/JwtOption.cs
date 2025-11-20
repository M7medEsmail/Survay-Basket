using Microsoft.EntityFrameworkCore.Query;
using System.ComponentModel.DataAnnotations;

namespace SurvayBacket.Api.Authentication
{
    public class JwtOption
    {

        public static string SectionName { get; } = "Jwt";
        [Required]
        public string Key { get; init; } = string.Empty;
        [Required]
        public string Issuer { get; init; } = string.Empty;
        [Required]
        public string Audience { get; init; } = string.Empty;
        [Range(1,int.MaxValue)]
        public int ExpiryInMinutes { get; init; }
    }
}
