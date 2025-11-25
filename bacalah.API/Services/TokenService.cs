using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using bacalah.API.Models;
using bacalah.Entities;
using bacalah.Entities.Entities;

namespace bacalah.API.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(User user)
        {
            var tokenRequest = new TokenRequestDto
            {
                UserId = user.Id,
                Email = user.Email,
                Username = user.UserName
            };

            return GenerateJwtToken(tokenRequest);
        }

        public string GenerateJwtToken(TokenRequestDto tokenRequest)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secret = jwtSettings["Secret"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var expiryInMinutes = Convert.ToDouble(jwtSettings["ExpiryInMinutes"]);

            ValidateJwtSettings(secret, issuer, audience);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, tokenRequest.UserId),
                new Claim(JwtRegisteredClaimNames.Email, tokenRequest.Email),
                new Claim(JwtRegisteredClaimNames.UniqueName, tokenRequest.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, tokenRequest.UserId)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            try
            {
                var jwtSettings = _configuration.GetSection("JwtSettings");
                var secret = jwtSettings["Secret"];
                var issuer = jwtSettings["Issuer"];
                var audience = jwtSettings["Audience"];

                ValidateJwtSettings(secret, issuer, audience);

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(secret!);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return principal;
            }
            catch
            {
                return null;
            }
        }

        private void ValidateJwtSettings(string? secret, string? issuer, string? audience)
        {
            if (string.IsNullOrEmpty(secret) || secret.Length < 32)
            {
                throw new ArgumentException("JWT secret key must be at least 32 characters long.");
            }

            if (string.IsNullOrEmpty(issuer))
            {
                throw new ArgumentException("JWT issuer must be configured.");
            }

            if (string.IsNullOrEmpty(audience))
            {
                throw new ArgumentException("JWT audience must be configured.");
            }
        }
    }
}