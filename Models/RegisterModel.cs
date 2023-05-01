using System.ComponentModel.DataAnnotations;

public class RegisterModel
{
  [Required]
  public string UserName { get; set; }

  [Required]
  [EmailAddress]
  public string Email { get; set; }

  [Required]
  [DataType(DataType.Password)]
  public string Password { get; set; }

  [Compare("Password")]
  [DataType(DataType.Password)]
  public string ConfirmPassword { get; set; }
}