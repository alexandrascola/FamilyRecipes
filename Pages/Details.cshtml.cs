using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FamilyRecipes.Models;
using FamilyRecipes.Services;

namespace FamilyRecipes.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IRecipeRepository _repo;

        public DetailsModel(IRecipeRepository repo) => _repo = repo;

        public Recipe? Recipe { get; private set; }

        public IActionResult OnGet(int id)
        {
            Recipe = _repo.GetById(id);
            if (Recipe is null) return NotFound();
            return Page();
        }
    }
}
