using AuthAPI.Models;
using AuthAPI.Settings;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace AuthAPI.Services
{
  public class UserService : IUserService
  {
    private readonly IMongoCollection<User> _users;
    public UserService(IMongoClient mongoClient, IAuthDatabaseSettings dbSettings)
    {
      var database = mongoClient.GetDatabase(dbSettings.DatabaseName);
      _users = database.GetCollection<User>(dbSettings.UsersCollectionName);
    }
    
    public async Task<User?> GetUserByIdAsync(string id)
    {
      var user = await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
      return user;
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
      var user = await _users.Find(u => u.UserName == username).FirstOrDefaultAsync();
      return user;
    }

    public async Task<IdentityResult> CreateUserAsync(RegisterModel registerModel)
    {
      // check if a user with the same username exists
      var existingUser = await _users.Find(u => u.UserName == registerModel.UserName).FirstOrDefaultAsync();
      if (existingUser != null)
      {
        throw new ArgumentException("A user with the same username already exists.");
      }

      // check if a user with the same email exists
      existingUser = await _users.Find(u => u.Email == registerModel.Email).FirstOrDefaultAsync();
      if (existingUser != null)
      {
        throw new ArgumentException("A user with the same email already exists.");
      }

      var user = new User { UserName = registerModel.UserName, Email = registerModel.Email };

      user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerModel.Password);
      await _users.InsertOneAsync(user);
      return IdentityResult.Success;
    }

    public async Task<User?> LogInAsync(string username, string password)
    {
      User? user = await GetUserByUsernameAsync(username);

      if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
      {
        return user;
      }

      return null;
    }

    public async Task LogOutAsync()
    {
      // No sign-out needed for Mongo-based auth.
      await Task.CompletedTask;
    }
  }
}