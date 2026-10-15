using System.ComponentModel.DataAnnotations;

namespace FamilyRecipes.Models
{
    public class Recipe
    {
        public int Id { get; set; }


        [Required, StringLength(100, ErrorMessage = "Name must be 100 characters or less.")]
        public string Name { get; set; } = string.Empty;


        [Required]
        public string Ingredients { get; set; } = string.Empty;


        [Required]
        public string Instructions { get; set; } = string.Empty;


        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}