using System.ComponentModel.DataAnnotations;

namespace PublicTransportAPI.Data.DTOs.Auth;

public class UserDTO
{
    [Required] public string? UserName { get; set; }
    [Required] public string? Password { get; set; }
}