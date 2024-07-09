using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeRecollection.Areas.Identity.Data;
using RecipeRecollection.Models;

namespace RecipeRecollection.Data;

public class RecipeRecollectionContext : IdentityDbContext<RecipeApplicant>
{
    public RecipeRecollectionContext(DbContextOptions<RecipeRecollectionContext> options) : base(options)
    {
    }

    public DbSet<Recipe> Recipes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

    }
}