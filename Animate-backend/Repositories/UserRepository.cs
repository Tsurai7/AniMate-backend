using Animate_backend.Data;
using Animate_backend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Animate_backend.Repositories;

public class UserRepository : IDisposable
{
    private readonly ApplicationContext _context;
    
    private bool _disposed = false;

    public UserRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllUsersAsync() =>
        await _context.Users.ToListAsync();
    
    public async Task<User> GetUserByIdAsync(long id) =>
        await _context.Users.FindAsync(new object[] {id});

    public async void AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User> RemoveUserAsync(long id)
    {
        var user = await _context.Users.FindAsync(id);
        
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        return user;
    }
    
    public void UpdateUser(long id, User updatedUser)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        
        if (user != null)
        {
            user.Username = updatedUser.Username;
            user.Email = updatedUser.Email;
            user.ProfileImage = updatedUser.ProfileImage;
            user.PasswordHash = updatedUser.PasswordHash;
            user.WatchedTitles = updatedUser.WatchedTitles;
            user.LikedTitles = updatedUser.LikedTitles;
        }
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if(!_disposed)
        {
            if(disposing)
            {
                _context.Dispose();
            }
        }
        _disposed = true;
    }
    
    public void Dispose() 
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    } 
}