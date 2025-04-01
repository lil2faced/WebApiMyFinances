

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiMyFinances.Core.Interfaces;
using WebApiMyFinances.WebApi.DTO;

namespace WebApiMyFinances.Security.Jwt
{
    public class JwtProvider : IJwtProvider
    {
        private readonly string _sectretKey;
        private readonly string _issuser;
        private readonly string _audience;
        private readonly double _tokenHours;
        public JwtProvider(IConfiguration configuration)
        {
            _sectretKey = configuration["AuthOptions:Key"]
            ?? throw new Exception("Секретный ключ не определен");
            _tokenHours = Double.Parse(configuration["AuthOptions:Hours"]
            ?? throw new Exception("Время жизни токена не определено"));
            _audience = configuration["AuthOptions:Audience"]
            ?? throw new Exception("Аудитория не определена");
            _issuser = configuration["AuthOptions:Issuer"]
            ?? throw new Exception("Издатель не определен");
        }

        public string GenerateToken(DTOUserAPIJwt user)
        {
            Claim[] claims = [new("userId", user.Id.ToString())];
            var SecretKeyGenerated = new SigningCredentials(
            new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_sectretKey)),
            SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
            issuer: _issuser,
            audience: _audience,
            claims: claims,
            signingCredentials: SecretKeyGenerated,
            expires: DateTime.UtcNow.AddHours(_tokenHours)
            );

            var tokenValue = new JwtSecurityTokenHandler()
            .WriteToken(token);
            return tokenValue;
        }
    }
}
