using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IConfiguration _config;

    public AuthenticationController(IConfiguration config)
    {
        this._config = config;
    }
    public record AuthenticationData(string? UserName, string? Password);
    
    public record UserData(int UserId, string UserName, string Title, string EmployeeId);

    // api/Authentication/token
   [HttpPost("token")]
    [AllowAnonymous]
    public ActionResult<string> Authenticate([FromBody] AuthenticationData data)
    {
        var user = ValidateCredentials(data);

        if (user is null)
        {
            return Unauthorized();
        }

        var token = GenerateToken(user);

        return Ok(token);

    }

    private string GenerateToken(UserData user)
    {
        var secretKey = new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes(
                _config.GetValue<string>("Authentication:SecretKey")));

        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256 );

        List<Claim> claims = new()
        {
            // standard claims
            new(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName),
            // Custom Claims (lowercase to follow javascipt/json/jwt standard)
            new("title", user.Title),
            new("employeeId", user.EmployeeId)
        };

        var token = new JwtSecurityToken(
            _config.GetValue<string>("Authentication:Issuer"),
            _config.GetValue<string>("Authentication:Audience"),
            claims,
            DateTime.UtcNow, // When this token becomes valid
            DateTime.UtcNow.AddMinutes(5), // When the token will expire
            signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private UserData? ValidateCredentials(AuthenticationData data)
    {
        // THIS IS NOT PRODUCTION CODE - THIS IS ONLY A DEMO - DO NOT USE IN REAL LIFE
        // Replace with call to Azure AD or similar
        if (CompareValues(data.UserName, "ndangelo") &&
            CompareValues(data.Password, "Test123"))
        {
            return new UserData(1, data.UserName!, "BusinessOwner", "E001");
        }
        if (CompareValues(data.UserName, "tcorey") &&
            CompareValues(data.Password, "Test123"))
        {
            return new UserData(2, data.UserName!, "Head of Security", "E005");
        }
        return null;
    }

    private bool CompareValues(string? actual, string expected)
    {
        if (actual is not null)
        {
            if (actual.Equals(expected))
            {
                return true;
            }

        }
            return false;
    }
}
