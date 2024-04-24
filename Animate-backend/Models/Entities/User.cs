using Microsoft.AspNetCore.Identity;

namespace Animate_backend.Models.Entities;

public class User : IdentityUser<long>
{
    public long Id { get; set; } 
    public string? Email { get; set; }

    public string? Username { get; set; }
   
    public string? ProfileImage { get; set; }
   
    public string? PasswordHash { get; set; }

    public List<string> WatchedTitles { get; set; }
    
    public List<string> LikedTitles { get; set; }
    
    // Identity
    public string? RefreshToken { get; set; }
    
    public DateTime? TokenExpiryTime { get; set; }

    public User(string username, string email, string passwordHash, string profileImage, List<string> watchedTitles, List<string> likedTitles)
    {
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        ProfileImage = profileImage;
        WatchedTitles = watchedTitles;
        LikedTitles = likedTitles;
    }
}