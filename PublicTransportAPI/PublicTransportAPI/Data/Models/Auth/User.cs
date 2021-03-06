using System.ComponentModel.DataAnnotations;
using PublicTransportAPI.Data.Models.Auth.Enums;

namespace PublicTransportAPI.Data.Models.Auth;

public class User
{
    public int Id { get; set; }
    [Required] public string? UserName { get; set; }
    [Required] public byte[]? PasswordHash { get; set; }
    [Required] public byte[]? PasswordSalt { get; set; }
    public Role Role { get; set; }
}