using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PublicTransportAPI.Data.DTOs.Auth;
using PublicTransportAPI.Data.Models.Auth.Enums;

namespace PublicTransportAPI.Helpers;

internal static class AuthHelper
{
    internal static string CreateToken(UserDTO user, Role role, string token)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.Role, role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token));
        var signinCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var jwtToken = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: signinCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}