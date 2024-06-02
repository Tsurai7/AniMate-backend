using Animate_backend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Animate_backend.Data;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
 
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}