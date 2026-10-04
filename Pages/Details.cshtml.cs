using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FamilyRecipes.Models;
using FamilyRecipes.Services;

namespace FamilyRecipes.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IBlogRepository _repo;

        public DetailsModel(IBlogRepository repo) => _repo = repo;

        public Post? Post { get; private set; }

        public IActionResult OnGet(int id)
        {
            Post = _repo.GetById(id);
            if (Post is null) return NotFound();
            return Page();
        }
    }
}
