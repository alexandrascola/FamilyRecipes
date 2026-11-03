using FamilyRecipes.Models;

namespace FamilyRecipes.Services
{
    public interface IRecipeRepository
    {
        IEnumerable<Recipe> GetAll();
        Recipe? GetById(int id);
        void Add(Recipe recipe);
        bool Update(Recipe recipe);
        bool Delete(int id);
        void Save();
    }
}
