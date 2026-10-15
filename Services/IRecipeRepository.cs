using FamilyRecipes.Models;

namespace FamilyRecipes.Services
{
    public interface IRecipeRepository
    {
        IEnumerable<Recipe> GetAll();
        Recipe? GetById(int id);
        void Add(Recipe recipe);
        void Save();
    }
}
