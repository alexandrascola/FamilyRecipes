using FamilyRecipes.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FamilyRecipes.Services
{
    public class JsonRecipeRepository : IRecipeRepository
    {
        //Class variables
        private readonly string _dataDir;
        private readonly string _dataFile;
        private readonly object _lock = new();


        //JSON options
        private readonly JsonSerializerOptions _json = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        //List and ID
        private List<Recipe> _recipes = new();
        private int _nextId = 1;

        //Constructor
        public JsonRecipeRepository(IWebHostEnvironment env)
        {
            _dataDir = Path.Combine(env.ContentRootPath, "data");
            _dataFile = Path.Combine(_dataDir, "recipes.json");
            Directory.CreateDirectory(_dataDir);
            LoadFromDisk();
        }

        //Get All Method
        public IEnumerable<Recipe> GetAll()
        {
            lock (_lock) return _recipes.OrderByDescending(p => p.CreatedUtc).ToList();
        }

        //Get By Id Method
        public Recipe? GetById(int id)
        {
            lock (_lock) return _recipes.FirstOrDefault(p => p.Id == id);
        }

        //Add Method
        public void Add(Recipe recipe)
        {
            lock (_lock)
            {
                recipe.Id = _nextId++;
                recipe.CreatedUtc = DateTime.UtcNow;
                _recipes.Add(recipe);
                SaveToDisk(); //save on each write 
            }

        }

        //Save Method
        public void Save()
        {
            lock (_lock) SaveToDisk();
        }

        //Load from disk
        private void LoadFromDisk()
        {
            if (!File.Exists(_dataFile))
            {
                _recipes = new(); _nextId = 1; return;
            }

            try
            {
                var text = File.ReadAllText(_dataFile);
                _recipes = JsonSerializer.Deserialize<List<Recipe>>(text, _json) ?? new();
                _nextId = _recipes.Count == 0 ? 1 : _recipes.Max(p => p.Id + 1);
            }
            catch
            {
                _recipes = new();
                _nextId = 1;
            }
        }

        //Save to disk
        private void SaveToDisk()
        {
            var text = JsonSerializer.Serialize(_recipes, _json);
            File.WriteAllText(_dataFile, text);
        }

    }
}
