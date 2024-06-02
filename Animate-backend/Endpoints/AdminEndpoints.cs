﻿using Microsoft.AspNetCore.Mvc;
using Animate_backend.Repositories;
using Animate_backend.Models.Entities;
using System;
using System.Collections.Generic;

namespace Animate_backend.Endpoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminEndpoints : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public AdminEndpoints(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("add")]
        public IActionResult AddUser(User newUser)
        {
            try
            {
                _userRepository.AddUser(newUser);
                return Ok("User added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("update/{id}")]
        public IActionResult UpdateUser(long id, User updatedUser)
        {
            try
            {
                var existingUser = _userRepository.GetAllUsers().Find(u => u.Id == id);
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
        public IActionResult DeleteUser(long id)
        {
            try
            {
                var existingUser = _userRepository.GetAllUsers().Find(u => u.Id == id);
                if (existingUser == null)
                {
                    return NotFound("User not found.");
                }

                _userRepository.RemoveUser(id);
                return Ok("User deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
