using AuthAPI.Models;

namespace AuthAPI.Services
{
  public interface IJwtService
  {
    string GenerateJwtToken(User user);
  }
}
