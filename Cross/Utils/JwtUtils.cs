using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MyApp.Cross.Utils
{
    public static class JwtUtils
    {
        public static string GenerateToken(string username, IConfiguration config)
        {
            var key = config["JwtSettings:Key"];
            var issuer = config["JwtSettings:Issuer"];

            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(issuer))
                throw new Exception("JWT key or issuer not found in configuration.");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
