using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;
using System.ComponentModel.DataAnnotations;
using FamilyRecipes.Models;

namespace FamilyRecipes.Pages.Account
{
    public class LoginModel : PageModel
    {
        //Class variables
        [BindProperty, Required]
        public string Username { get; set; } = string.Empty;

        [BindProperty, Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public string? Error { get; set; }

        public void OnGet()
        {
        }

        //Validate credentials and login
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var dataDir = Path.Combine(Directory.GetCurrentDirectory(), "data");
            var usersFile = Path.Combine(dataDir, "users.json");
            var users = JsonSerializer.Deserialize<List<User>>(
                System.IO.File.ReadAllText(usersFile)) ?? new();

            //Hash password
            using var sha = SHA256.Create();
            var hash = Convert.ToHexString(
                sha.ComputeHash(Encoding.UTF8.GetBytes(Password)));

            var user = users.FirstOrDefault(u =>
                u.Username.Equals(Username, StringComparison.OrdinalIgnoreCase)
                && u.PasswordHash == hash);

            if (user == null)
            {
                Error = "Invalid username or password.";
                return Page();
            }

            //Issue auth cookie
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("Cookies", principal);

            return RedirectToPage("/Admin");
        }
    }
}