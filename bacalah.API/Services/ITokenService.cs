using System.Security.Claims;
using bacalah.API.Models;
using bacalah.Entities.Entities;

namespace bacalah.API.Services;

public interface ITokenService
{
    string GenerateJwtToken(User user);
    string GenerateJwtToken(TokenRequestDto tokenRequest);
    ClaimsPrincipal? ValidateToken(string token);
}