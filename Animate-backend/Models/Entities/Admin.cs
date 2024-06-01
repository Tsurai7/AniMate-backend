using Microsoft.AspNetCore.Identity;

namespace Animate_backend.Models.Entities;

public class Admin : IdentityUser<long>
{
    public long Id { get; set; }

    public string? Email { get; set; }

    public string? Username { get; set; }
    public string? PasswordHash { get; set; }


    public Admin(string username, string email, string passwordHash)
    {
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
    }
}