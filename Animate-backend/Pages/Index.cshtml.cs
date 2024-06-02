using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Animate_backend.Repositories;
using Animate_backend.Models.Entities;
using System.Collections.Generic;

namespace Animate_backend.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UserRepository _userRepository;

        public IndexModel(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [BindProperty]
        public List<User> Users { get; set; }

        [BindProperty]
        public User NewUser { get; set; }

        public void OnGet()
        {
            Users = _userRepository.GetAllUsers();
        }

        public IActionResult OnPostAddUser()
        {
            if (ModelState.IsValid && NewUser != null)
            {
                _userRepository.AddUser(NewUser);
                return RedirectToPage();
            }
            return Page();
        }

        public void OnPostDeleteUser(long id)
        {
            _userRepository.RemoveUser(id);
        }
    }
}
