
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SurvayBacket.Api.Authentication
{
    public class JwtProvider(IOptions<JwtOption> options) : IJwtProvider
    {
        private readonly JwtOption _options = options.Value;

        public (string Token, int Expiration) GenerateJwtToken(ApplicationUser user )
        {
            Claim[] claims = [
                new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new (JwtRegisteredClaimNames.Email, user.Email!),
                new (JwtRegisteredClaimNames.GivenName, user.FirstName!),
                new (JwtRegisteredClaimNames.FamilyName, user.LastName!),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                ];
           
            var symetricSecurityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_options.Key));

            var signingCredentials = new SigningCredentials(symetricSecurityKey, SecurityAlgorithms.HmacSha256);

            int expirationInMinutes = 30;
            var jwtSecurityToken = new JwtSecurityToken(
                issuer:_options.Issuer ,
                audience: _options.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(_options.ExpiryInMinutes),
                signingCredentials: signingCredentials
                );


            return(token: new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken), expirein:_options.ExpiryInMinutes * 60);    
        }

        public string ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var symetricSecurityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_options.Key));
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = symetricSecurityKey,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken; // these to decode token to get claims

                return jwtToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value; 
            }
            catch 
            {
                return null;
            }

        }
    }
}
