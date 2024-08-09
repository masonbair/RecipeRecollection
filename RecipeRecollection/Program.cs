using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecipeRecollection.Areas.Identity.Data;
using RecipeRecollection.Data;
using System;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("RecipeRecollectionContextConnection") ?? throw new InvalidOperationException("Connection string 'RecipeRecollectionContextConnection' not found.");

builder.Services.AddDbContext<RecipeRecollectionContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<RecipeApplicant>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<RecipeRecollectionContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();

var app = builder.Build();

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var db = serviceScope.ServiceProvider.GetRequiredService<RecipeRecollectionContext>().Database;

    logger.LogInformation("Migrating database...");

    try
    {
        // Ensure database is created
        db.EnsureCreated();

        // Apply pending migrations
        db.Migrate();

        logger.LogInformation("Database created and migrated successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while creating and migrating the database.");
    }
}


    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();
