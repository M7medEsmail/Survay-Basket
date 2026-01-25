using System.Reflection.Metadata.Ecma335;

namespace SurvayBacket.Api.Entities
{
    [Owned]
    public class RefreshToken
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpireOn { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? RevokedOn { get; set; }

        public bool IsExpired => DateTime.Now >= ExpireOn;
        public bool IsActive => RevokedOn is null && !IsExpired;
    }
}
