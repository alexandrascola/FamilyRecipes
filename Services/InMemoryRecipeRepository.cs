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
        public void Save() {  /*unusable for storing in RAM */}
    }
}
