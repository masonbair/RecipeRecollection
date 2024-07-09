using System.ComponentModel.DataAnnotations;

namespace RecipeRecollection.Models
{
    public class Recipe
    {
        [Key]
        public int recipeID { get; set; }
        public string User { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Ingredients { get; set; }
        [Required]
        public string Steps { get; set; }
        [Required]
        public string Url { get; set; }
        
    }
}
