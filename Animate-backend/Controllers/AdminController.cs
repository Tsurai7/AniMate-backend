using Animate_backend.Models.Entities;
using Animate_backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Animate_backend.Controllers;

[Route("[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly UserRepository _userRepository;
    
    public AdminController(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost("add")]
    public IActionResult AddUser(User newUser)
    {
        try
        {
            _userRepository.AddUserAsync(newUser);
            return Ok("User added successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateUser(long id, User updatedUser)
    {
        try
        {
            var existingUser = await _userRepository.GetUserByIdAsync(id);
            
            if (existingUser == null)
            {
                return NotFound("User not found.");
            }

            existingUser.Username = updatedUser.Username;
            existingUser.Email = updatedUser.Email;
            existingUser.ProfileImage = updatedUser.ProfileImage;
            existingUser.PasswordHash = updatedUser.PasswordHash;
            existingUser.WatchedTitles = updatedUser.WatchedTitles;
            existingUser.LikedTitles = updatedUser.LikedTitles;

            return Ok("User updated successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteUser(long id)
    {
        try
        {
            var existingUser = _userRepository.RemoveUserAsync(id);
            
            if (existingUser == null)
            {
                return NotFound("User not found.");
            }

            _userRepository.RemoveUserAsync(id);
            return Ok("User deleted successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
}