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
        private readonly IRecipeRepository _repo;
        public CreateModel(IRecipeRepository repo) => _repo = repo;

        [BindProperty]
        public InputModel Form { get; set; } = new();
        


        public class InputModel
        {
            [Required, StringLength(100)]
            public string Name { get; set; } = string.Empty;

            [Required]
            public string Ingredients { get; set; } = string.Empty;

            [Required]
            public string Instructions { get; set; } = string.Empty;
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

            var recipe = new Recipe
            {
                Name = Form.Name.Trim(),
                Ingredients = Form.Ingredients.Trim(),
                Instructions = Form.Instructions.Trim()
            };

            _repo.Add(recipe);
            _repo.Save(); //does nothing on RAM, persists for json

            return RedirectToPage("/Details", new { id = recipe.Id });

        }
    }
}
