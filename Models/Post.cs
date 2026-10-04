using System.ComponentModel.DataAnnotations;
using System.Numerics;
namespace FamilyRecipes.Models
{
    public class Post
    {
        public int Id { get; set; }


        [Required, StringLength(100, ErrorMessage = "Title must be 100 characters or less.")]
        public string Title { get; set; } = string.Empty;


        [Required]
        public string Body { get; set; } = string.Empty;


        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;


    }
}
