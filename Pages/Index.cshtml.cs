using FamilyRecipes.Services;
using FamilyRecipes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamilyRecipes.Pages;

public class IndexModel : PageModel
{
    private readonly IRecipeRepository _repo;
    public List<Recipe> Recipes { get; private set; } = new();
    public IndexModel(IRecipeRepository repo) => _repo = repo;


    public void OnGet()
    {
        Recipes = _repo.GetAll().ToList();

    }
}
