using Microsoft.AspNetCore.Mvc;
using FamilyRecipes.Models;
using FamilyRecipes.Services;
using Microsoft.AspNetCore.Authorization;

namespace FamilyRecipes.Controllers
{
    public class RecipesController : Controller
    {
        //Class variables
        private readonly IRecipeRepository _repo;
        private readonly IWebHostEnvironment _env;
        private const long MaxFileSizeBytes = 2 * 1024 * 1024; //limit image to 2mb

        //Constructor
        public RecipesController(IRecipeRepository repo, IWebHostEnvironment env)
        {
            _repo = repo;
            _env = env;
        }



        //Index Method
        public IActionResult Index()
        {
            var recipes = _repo.GetAll()
                .OrderBy(recipe => recipe.CreatedUtc)
                .ToList();
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
        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }




        //Edit Method
        [Authorize]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var recipe = _repo.GetById(id);

            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }



        //Post Method
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Recipe recipe, IFormFile? image)
        {
            //Set the image path only after saving an uploaded file
            recipe.ImagePath = null;

            //Validate the image if one was selected
            if (image != null)
            {
                if (image.Length == 0)
                {
                    ModelState.AddModelError("image", "Please choose a non-empty image file.");
                }

                if (image.Length >= MaxFileSizeBytes)
                {
                    ModelState.AddModelError("image", "Image must be less than 2 MB.");
                }

                var extension = Path.GetExtension(image.FileName).ToLowerInvariant();

                if (extension != ".jpg" && extension != ".png")
                {
                    ModelState.AddModelError("image", "Only .jpg and .png files are allowed.");
                }
            }

            //Validate the model
            if (!ModelState.IsValid)
            {
                return View(recipe);
            }

            //Save the image if one was selected
            if (image != null)
            {
                var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsPath);

                var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
                var fileName = $"{Guid.NewGuid():N}{extension}";
                var filePath = Path.Combine(uploadsPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                recipe.ImagePath = $"/uploads/{fileName}";
            }

            //Save the recipe to JSON
            _repo.Add(recipe);

            //Return to the index page
            return RedirectToAction(nameof(Index));
        }

        //Save recipe edits
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Recipe recipe, IFormFile? image)
        {
            if (id != recipe.Id)
            {
                return NotFound();
            }

            var existing = _repo.GetById(id);

            if (existing == null)
            {
                return NotFound();
            }

            //Keep the current image unless a new one is uploaded
            recipe.ImagePath = existing.ImagePath;

            //Validate the image if one was selected
            if (image != null)
            {
                if (image.Length == 0)
                {
                    ModelState.AddModelError("image", "Please choose a non-empty image file.");
                }

                if (image.Length >= MaxFileSizeBytes)
                {
                    ModelState.AddModelError("image", "Image must be less than 2 MB.");
                }

                var extension = Path.GetExtension(image.FileName).ToLowerInvariant();

                if (extension != ".jpg" && extension != ".png")
                {
                    ModelState.AddModelError("image", "Only .jpg and .png files are allowed.");
                }
            }

            //Validate the model before saving anything
            if (!ModelState.IsValid)
            {
                return View(recipe);
            }

            //Save the image if one was selected
            if (image != null)
            {
                var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsPath);

                var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
                var fileName = $"{Guid.NewGuid():N}{extension}";
                var filePath = Path.Combine(uploadsPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                recipe.ImagePath = $"/uploads/{fileName}";
            }

            //Save changes to JSON
            if (!_repo.Update(recipe))
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }



        //Delete Method
        [Authorize]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var recipe = _repo.GetById(id);

            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        [Authorize]
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            //Delete the recipe and save to JSON
            if (!_repo.Delete(id))
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}