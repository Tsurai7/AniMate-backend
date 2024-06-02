using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Animate_backend.Models.Dtos;
using Animate_backend.Models.Entities;
using Animate_backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Animate_backend.Endpoints;

public static class AccountEndpoints
{
    public static void RegisterEndpoint(WebApplication app)
    {
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
                
            var response = new AuthResponse()
            {
                AccessToken = encodedJwt,
                RefreshToken = encodedJwt
            };
         
            return Results.Json(response);
        });


        app.MapPost("/signUp", ([FromBody] SignUpRequest signUpRequest) =>
        {
            User? userInDb = UserRepository.users.FirstOrDefault(x => x.Email == signUpRequest.Email && x.PasswordHash == signUpRequest.Password);
            
            if(userInDb is not null) 
                return Results.BadRequest();
            
            UserRepository.users.Add(new User(signUpRequest.Username, signUpRequest.Email,
                signUpRequest.Password, "https://sun9-62.userapi.com/impf/-TVGNBqEWNAZB--OX_HMFhWqNChiQQr48XA09w/Qf-oDLi-o4Y.jpg?size=340x340&quality=96&sign=17b91c256f67c2232b317fa35b260c9e&type=album",
                new List<string>(){"dsf"}, new List<string>(){"sdf"}));
            
            var claims = new List<Claim>() {new Claim(ClaimTypes.NameIdentifier, signUpRequest.Email) };
            
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(100)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                
            var response = new AuthResponse()
            {
                AccessToken = encodedJwt,
                RefreshToken = encodedJwt
            };
         
            return Results.Json(response);
        });


        app.MapGet("/profile", [Authorize] (HttpContext context) =>
        {
            string email = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            var user = UserRepository.users.FirstOrDefault(user => user.Email == email);

            if (user is not null)
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
    }
}