using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using RecipeRecollection.Areas.Identity.Data;
using WebMatrix.WebData;

namespace RecipeRecollection.Models
{
    public class Recipe
    {
        [Key]
        public int recipeID { get; set; }
        public string? User { get; set; }
        public string? Name { get; set; }
        public string? Ingredients { get; set; }
        public string? Steps { get; set; }
        [Required]
        public string Url { get; set; }

    }
}
