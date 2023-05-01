using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AuthAPI.Models;
using AuthAPI.Settings;
using Microsoft.IdentityModel.Tokens;

namespace AuthAPI.Services;

class JwtService : IJwtService
{
  private readonly JwtSettings _jwtSettings;

  public JwtService(JwtSettings jwtSettings)
  {
    _jwtSettings = jwtSettings;
  }

  public string GenerateJwtToken(User user)
  {
    var symmetricKey = Convert.FromBase64String(_jwtSettings.SecretKey);
    var tokenHandler = new JwtSecurityTokenHandler();

    var now = DateTime.UtcNow;
    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(new[]
      {
        new Claim("id", user.Id),
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Email, user.Email)
      }),

      Expires = now.Add(_jwtSettings.TokenLifetime),

      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey),
        SecurityAlgorithms.HmacSha256Signature)
    };

    var stoken = tokenHandler.CreateToken(tokenDescriptor);
    var token = tokenHandler.WriteToken(stoken);

    return token;
  }
}