using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Animate_backend.Models.Dtos;
using Animate_backend.Models.Entities;
using Animate_backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Animate_backend.Controllers;

[Route("[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IConfiguration _configuration;
    
    private readonly UserRepository _userRepository;
    
    public AccountController(IConfiguration configuration, UserRepository userRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
    }

    [HttpPost("signUp")]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequest signUpRequest)
    {
        var userInDb = (await _userRepository.GetAllUsersAsync()).FirstOrDefault(x => x.Email == signUpRequest.Email && x.PasswordHash == signUpRequest.Password);
            
        if(userInDb is not null) 
            return BadRequest();
            
        _userRepository.AddUserAsync(new User(signUpRequest.Username, signUpRequest.Email,
            signUpRequest.Password, "https://sun9-62.userapi.com/impf/-TVGNBqEWNAZB--OX_HMFhWqNChiQQr48XA09w/Qf-oDLi-o4Y.jpg?size=340x340&quality=96&sign=17b91c256f67c2232b317fa35b260c9e&type=album",
            new List<string>(){"dsf"}, new List<string>(){"sdf"}));
            
        var claims = new List<Claim>() {new Claim(ClaimTypes.NameIdentifier, signUpRequest.Email) };
            
        var jwt = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(100)),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(          
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)), SecurityAlgorithms.HmacSha256));
            
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
         
        return Ok(new AuthResponse
        (
            encodedJwt,
            encodedJwt
        ));
    }
    
    [HttpPost("signIn")]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest signInRequest)
    {
        var userInDb = (await _userRepository.GetAllUsersAsync()).FirstOrDefault(x => x.Email == signInRequest.Email && x.PasswordHash == signInRequest.Password);
            
        if(userInDb is null) 
            return Unauthorized();
            
        var claims = new List<Claim>() {new Claim(ClaimTypes.NameIdentifier, signInRequest.Email) };
            
        var jwt = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(100)),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(          
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)), SecurityAlgorithms.HmacSha256));
            
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                
        return Ok(new AuthResponse
        (
            encodedJwt,
            encodedJwt
        ));
    }
    
    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> Profile(HttpContext context)
    {
        string email = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
        var user = (await _userRepository.GetAllUsersAsync()).FirstOrDefault(u => u.Email == email);

        if (user is not null)
            return Ok(new ProfileDto
            (
                user.Username,
                user.ProfileImage,
                user.Email,
                user.WatchedTitles,
                user.LikedTitles
            ));
            
        return NotFound();
    }
}