using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PublicTransportAPI.Data;
using PublicTransportAPI.Data.DTOs.Auth;
using PublicTransportAPI.Data.Models.Auth;
using PublicTransportAPI.Data.Models.Auth.Enums;
using PublicTransportAPI.Helpers;

namespace PublicTransportAPI.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IConfiguration _configuration;
    
    public AuthController(ApplicationDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(UserDTO userDto)
    {
        if (string.IsNullOrWhiteSpace(userDto.UserName) || string.IsNullOrWhiteSpace(userDto.Password))
        {
            return new BadRequestObjectResult("Username and Password must be provided.");
        }
        
        var registerUser = new User();
        
        using var hmac = new HMACSHA512();

        registerUser.UserName = userDto.UserName;
        registerUser.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDto.Password));
        registerUser.PasswordSalt = hmac.Key;
        registerUser.Role = Role.User;

        try
        {
            if (await _dbContext.Users.FirstOrDefaultAsync(user => user.UserName!.Equals(userDto.UserName)) is not null)
            {
                return new BadRequestObjectResult("Username exists.");
            }
            
            _ = await _dbContext.Users.AddAsync(registerUser);
            _ = await _dbContext.SaveChangesAsync();

            return new OkResult();
        }
        catch (Exception e)
        {
            return new BadRequestObjectResult($"Could not register new user. Exception occured. Message: {e.Message}");
        }
    }
    
    [HttpPost("login")]
    public async Task<ActionResult> Login(UserDTO userDto)
    {
        if (string.IsNullOrWhiteSpace(userDto.UserName) || string.IsNullOrWhiteSpace(userDto.Password))
        {
            return new BadRequestObjectResult("Username and Password must be provided.");
        }

        try
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.UserName!.Equals(userDto.UserName));

            if (user is null)
            {
                return new BadRequestObjectResult("Invalid data provided. Check username and password.");
            }
            
            using var hmac = new HMACSHA512(user.PasswordSalt!);
            var computedUserDtoHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDto.Password));

            if (!user.PasswordHash!.SequenceEqual(computedUserDtoHash))
            {
                return new BadRequestObjectResult("Invalid data provided. Check username and password.");
            }

            var token = AuthHelper.CreateToken(userDto, user.Role, _configuration.GetSection("AuthConfiguration:Token").Value);
            return new OkObjectResult(token);

        }
        catch (Exception e)
        {
            return new BadRequestObjectResult($"Could not register new user. Exception occured. Message: {e.Message}");
        }
    }
}