﻿using System.ComponentModel.DataAnnotations;

namespace AuthAPI.Models;

public class LoginModel
{
  [Required]
  [Display(Name = "Username")]
  public string UserName { get; set; }

  [Required]
  [DataType(DataType.Password)]
  [Display(Name = "Password")]
  public string Password { get; set; }
}