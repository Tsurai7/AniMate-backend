using Animate_backend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Animate_backend.Data;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Admin> Admins { get; set; } = null!;
 
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        Database.EnsureCreated();
        
        InitDb();
    }
    
    private void InitDb()
    {
        if (!Users.Any())
        {
            Users.AddAsync(new User("Ken Kaneki", "test@test.com", "123", 
                "https://i.pinimg.com/originals/e6/94/e9/e694e991d7ec9af6330657eb6ee479f5.jpg",
                new List<string>() {"kizumonogatari-iii-reiketsu-hen", "anatsu-no-taizai-kamigami-no-gekirin"},
                new List<string>() {"kizumonogatari-iii-reiketsu-hen"}));
            
            SaveChangesAsync();
        }

        if (!Admins.Any())
        {
            Admins.AddAsync(new Admin("Vladiii", "123@123.com", "123"));
                
            SaveChangesAsync();
        }
    }
}