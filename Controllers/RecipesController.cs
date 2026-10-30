using Microsoft.AspNetCore.Mvc;
using FamilyRecipes.Models;
using FamilyRecipes.Services;

namespace FamilyRecipes.Controllers
{
    public class RecipesController : Controller
    {
        //Class variables
        private readonly IRecipeRepository _repo;

        //Constructor
        public RecipesController(IRecipeRepository repo)
        {
            _repo = repo;
        }

        //Index Method
        public IActionResult Index()
        {
            var recipes = _repo.GetAll();
            return View(recipes);
        }

        //Details Method
        public IActionResult Details(int id)
        {
            var recipe = _repo.GetById(id);

            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }
        
        //Create Method
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Recipe recipe)
        {
            //Validate the model
            if (!ModelState.IsValid)
            {
                return View(recipe);
            }

            //Save the recipe to JSON
            _repo.Add(recipe);

            //Return to the index page
            return RedirectToAction(nameof(Index));
        }
    }
}
   