using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PIM.Api.Infra.AuthEngine.Interface;
using PIM.Api.Models;

namespace PIM.Api.Infra.AuthEngine.Provider
{
    public class JwtProvider: ITokenProvider
    {
        private readonly int _expireTime;

        public JwtProvider(IConfiguration configuration) =>
            _expireTime = configuration.GetValue<int>("token:expireTime");

        public virtual string GenerateToken(string hash, SystemUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(hash);

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName)
            };

            var claimsIdentity = new ClaimsIdentity(claims);

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddSeconds(_expireTime),
                SigningCredentials = signingCredentials
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
