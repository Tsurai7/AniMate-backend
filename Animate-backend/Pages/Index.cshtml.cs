using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Animate_backend.Repositories;
using Animate_backend.Models.Entities;

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

        public async void OnGet()
        {
            Users =  await _userRepository.GetAllUsersAsync();
        }

        public IActionResult OnPostAddUser()
        {
            if (ModelState.IsValid && NewUser != null)
            {
                _userRepository.AddUserAsync(NewUser);
                return RedirectToPage();
            }
            return Page();
        }

        public async void OnPostDeleteUser(long id)
        {
            await _userRepository.RemoveUserAsync(id);
        }
    }
}
