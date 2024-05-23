using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JWTExample.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{

    private readonly IHttpContextAccessor _contextAccessor;

    public UsersController(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Check()
    {
        return Ok("It is working");
    }


    [HttpPost("example")]
    [Authorize]
    public async Task<IActionResult> Calculate([FromQuery]int a, [FromQuery]  int b)
    {
        var c = a + b;
        return Ok(c);
    }

    [HttpGet("token")]

    public string GetToken()
    {
        var signinKey = System.Text.Encoding.UTF32.GetBytes("qwertyuiop1234567890qwertyuiop");
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.GivenName,"Maruf"),
            new Claim(ClaimTypes.NameIdentifier,"qwert1234"),
            new Claim(ClaimTypes.Role,"SuperAdmin")
        };
        var security = new JwtSecurityToken(issuer: "Blog.Api",audience: "Blog.Client",claims:claims,expires:DateTime.Now.AddMinutes(10),signingCredentials: new SigningCredentials( new SymmetricSecurityKey(signinKey),algorithm: "HS256") );

        var token = new JwtSecurityTokenHandler().WriteToken(security);
        return token;
    }

    [HttpGet("profile")]
    
    [Authorize]
    public async Task<IActionResult> GetProfile()
    {

        var givenName = _contextAccessor.HttpContext!.User.FindFirstValue(claimType: ClaimTypes.GivenName);
        var role = _contextAccessor.HttpContext!.User.FindFirstValue(claimType: ClaimTypes.Role);

        return Ok(new {givenName, role});
    }
}