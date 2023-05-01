using AuthAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthAPI.Services
{
  public interface IUserService
  {
    Task<User?> GetUserByIdAsync(string id);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<IdentityResult> CreateUserAsync(RegisterModel registerModel);
    Task<User?> LogInAsync(string username, string password);
    Task LogOutAsync();
  }
}