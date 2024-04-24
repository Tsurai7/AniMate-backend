using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Animate_backend.Data;
using Animate_backend.Models.Dtos;
using Animate_backend.Models.Entities;
using Animate_backend.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection")));

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.ISSUER,
            ValidateAudience = true,
            ValidAudience = AuthOptions.AUDIENCE,
            ValidateLifetime = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
        }; 
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

 
app.UseAuthentication();  
app.UseAuthorization();   

app.MapGet("/test", () =>
{
    return "Test endpoint";
});
 
 
app.MapPost("/signIn",  ([FromBody] SignInRequest signInRequest) =>
{
    User? userInDb = UserRepository.users.FirstOrDefault(x => x.Email == signInRequest.Email && x.PasswordHash == signInRequest.Password);
    
    if(userInDb is null) 
        return Results.Unauthorized();
    
    var claims = new List<Claim>() {new Claim(ClaimTypes.NameIdentifier, signInRequest.Email) };
    
    var jwt = new JwtSecurityToken(
        issuer: AuthOptions.ISSUER,
        audience: AuthOptions.AUDIENCE,
        claims: claims,
        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(100)),
        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
    
    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        
    var response = new
    {
        access_token = encodedJwt,
        email = signInRequest.Email
    };
 
    return Results.Json(response);
});


app.MapPost("/signUp", ([FromBody] SignUpRequest signUpRequest) =>
{
    User? userInDb = UserRepository.users.FirstOrDefault(x => x.Email == signUpRequest.Email && x.PasswordHash == signUpRequest.PasswordHash);
    
    if(userInDb is not null) 
        return Results.BadRequest();
    
    var claims = new List<Claim>() {new Claim(ClaimTypes.NameIdentifier, signUpRequest.Email) };
    
    var jwt = new JwtSecurityToken(
        issuer: AuthOptions.ISSUER,
        audience: AuthOptions.AUDIENCE,
        claims: claims,
        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(100)),
        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
    
    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        
    var response = new
    {
        access_token = encodedJwt,
        username = signUpRequest.Username,
        email = signUpRequest.Email
    };
 
    return Results.Json(response);
});


app.MapGet("/profile", [Authorize] (HttpContext context) =>
{
    string email = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    
    var user = UserRepository.users.FirstOrDefault(user => user.Email == email);

    if (user != null)
        return Results.Ok(new 
        {
            username = user.Username,
            email = user.Email,
            profile_image = user.ProfileImage,
            watched_titles = user.WatchedTitles,
            liked_titles = user.LikedTitles
        });
    
    return Results.NotFound();
});

app.MapGet("/data", [Authorize] (HttpContext context) => $"Authorized!");

app.Run();

public class AuthOptions
{
    public const string ISSUER = "MyAuthServer"; 
    public const string AUDIENCE = "MyAuthClient";
    const string KEY = "mysupersecret_secretsecretsecretkey!123";
    public static SymmetricSecurityKey GetSymmetricSecurityKey() => 
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}