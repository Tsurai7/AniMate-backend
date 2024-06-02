using Animate_backend.Data;
using Animate_backend.Models.Entities;

namespace Animate_backend.Repositories;

public class UserRepository
{
    private readonly ApplicationContext _context;

    public UserRepository(ApplicationContext context)
    {
        _context = context;
    }
    
    
}