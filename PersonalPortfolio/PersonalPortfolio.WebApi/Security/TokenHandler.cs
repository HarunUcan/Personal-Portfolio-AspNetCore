using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace PersonalPortfolio.WebApi.Security
{
    public static class TokenHandler
    {
        public static Token CreateToken(IConfiguration configuration)
        {
            Token token = new();

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:SecurityKey"]));

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            token.AccessTokenExpiration = DateTime.Now.AddMinutes(Convert.ToDouble(configuration["Token:AccessTokenExpiration"]));
            token.RefreshTokenExpiration = DateTime.Now.AddDays(Convert.ToDouble(configuration["Token:RefreshTokenExpiration"]));

            JwtSecurityToken jwtSecurityToken = new(
                issuer: configuration["Token:Issuer"],
                audience: configuration["Token:Audience"],
                expires: token.AccessTokenExpiration,
                notBefore: DateTime.Now,
                signingCredentials: signingCredentials
            );

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
            token.AccessToken = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);
            token.RefreshToken = Guid.NewGuid().ToString();

            return token;
        }
    }
}
