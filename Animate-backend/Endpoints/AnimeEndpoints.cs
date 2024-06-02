using System.Security.Claims;
using Animate_backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Animate_backend.Endpoints;

public class AnimeEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        var dbContextOptions = app.Services.GetRequiredService<DbContextOptions<ApplicationContext>>();
        _context = new ApplicationContext(dbContextOptions);
        
        app.MapPatch("/addTitleToHistory", [Authorize](HttpContext context,  [FromHeader] string titleCode) =>
        {
            string email = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    
            var user = UserRepository.users.FirstOrDefault(user => user.Email == email);

            if (user is not null)
            {
                user.WatchedTitles.Add(titleCode);
                return Results.Ok(); 
            }
    
            return Results.NotFound();
        });


        app.MapPatch("/addTitleToLiked", [Authorize](HttpContext context, [FromHeader] string titleCode) =>
        {
            string email = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    
            var user = UserRepository.users.FirstOrDefault(user => user.Email == email);

            if (user is not null)
            {
                user.LikedTitles.Add(titleCode);
                return Results.Ok();
            }
    
            return Results.NotFound();
        });
    }
}