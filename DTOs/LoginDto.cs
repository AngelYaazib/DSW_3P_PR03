
using System.ComponentModel.DataAnnotations;

namespace DSW_3P_PR03.DTOs;

public class LoginDto
{
    [Required]
    public string Usuario { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
