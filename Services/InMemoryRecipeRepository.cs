using FamilyRecipes.Models;

namespace FamilyRecipes.Services
{
    public class InMemoryRecipeRepository : IRecipeRepository
    {
        private readonly List<Recipe> _posts = new();
        private int _nextId = 1;

        public IEnumerable<Recipe> GetAll() => _posts.OrderByDescending(p => p.Id);

        public Recipe? GetById(int id) => _posts.FirstOrDefault(p => p.Id == id);

        public void Add(Recipe recipe)
        {
            recipe.Id = _nextId++;
            recipe.CreatedUtc = DateTime.UtcNow;
            _posts.Add(recipe);
        }

        public bool Update(Recipe recipe)
        {
            var existing = _posts.FirstOrDefault(p => p.Id == recipe.Id);

            if (existing == null)
            {
                return false;
            }

            existing.Name = recipe.Name;
            existing.Ingredients = recipe.Ingredients;
            existing.Instructions = recipe.Instructions;

            return true;
        }

        public bool Delete(int id)
        {
            var existing = _posts.FirstOrDefault(p => p.Id == id);

            if (existing == null)
            {
                return false;
            }

            _posts.Remove(existing);
            return true;
        }
        public void Save() {  /*unusable for storing in RAM */}
    }
}
