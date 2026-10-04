using FamilyRecipes.Services;
using FamilyRecipes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace FamilyRecipes.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IBlogRepository _repo;
        public CreateModel(IBlogRepository repo) => _repo = repo;

        [BindProperty]
        public InputModel Form { get; set; } = new();
        


        public class InputModel
        {

            [Required, StringLength(100)]
            public string Title { get; set; } = string.Empty;

            [Required]
            public string Body { get; set; } = string.Empty;
        }
        
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            var post = new Post
            {
                Title = Form.Title.Trim(),
                Body = Form.Body.Trim()
            };

            _repo.Add(post);
            _repo.Save(); //does nothing on RAM, persists for json

            return RedirectToPage("/Details", new { id = post.Id });

        }
    }
}
