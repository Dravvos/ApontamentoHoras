using ApontamentoHoras.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApontamentoHoras.IdentityServer.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _conf;

        public TokenService(IConfiguration conf)
        {
            _conf = conf;
        }

        public string GenerateTokenAsync(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.GivenName, user.Nome),
                new Claim(JwtRegisteredClaimNames.FamilyName,user.Sobrenome),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email)
            };
            var jwtSettings = _conf.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims:claims,
                expires:DateTime.Now.AddHours(3),
                signingCredentials:creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
