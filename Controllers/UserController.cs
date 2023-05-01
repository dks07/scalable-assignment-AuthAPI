using Microsoft.AspNetCore.Mvc;
using AuthAPI.Models;
using AuthAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace AuthAPI.Controllers
{
  [ApiController]
  [AllowAnonymous]
  [Route("api/[controller]")]
  public class UserController : ControllerBase
  {
    private readonly IUserService _userService;
    private readonly IJwtService _jwtService;

    public UserController(IUserService userService, IJwtService jwtService)
    {
      _userService = userService;
      _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterModel model)
    {
      if (ModelState.IsValid)
      {
        var result = await _userService.CreateUserAsync(model);

        if (result.Succeeded)
        {
          return Created("", null);
        }
        return BadRequest(result.Errors);
      }

      return BadRequest(ModelState);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel model)
    {
      if (ModelState.IsValid)
      {
        var user = await _userService.LogInAsync(model.UserName, model.Password);
        if (user == null)
        {
          return Unauthorized();
        }

        var token = _jwtService.GenerateJwtToken(user);

        return Ok(new { token });
      }

      return BadRequest(ModelState);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
      await _userService.LogOutAsync();
      return Ok("Logout successful.");
    }
  }
}