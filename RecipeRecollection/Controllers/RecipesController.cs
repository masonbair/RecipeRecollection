using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecipeRecollection.Data;
using RecipeRecollection.Models;

//This is the using statements for 
using System;
using System.Collections;
using Python.Runtime;

namespace RecipeRecollection.Controllers
{
    public class RecipesController : Controller
    {
        private readonly RecipeRecollectionContext _context;

        public RecipesController(RecipeRecollectionContext context)
        {
            _context = context;
        }

        // GET: Recipes
        public async Task<IActionResult> Index()
        {
            return _context.Recipes != null ?
                        View(await _context.Recipes.ToListAsync()) :
                        Problem("Entity set 'RecipeRecollectionContext.Recipes'  is null.");
        }

        // GET: Recipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .FirstOrDefaultAsync(m => m.recipeID == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // GET: Recipes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Url")] Recipe recipe)
        {
            Environment.SetEnvironmentVariable("PYTHONHOME", "/usr/lib/python3.9");
            Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", "/usr/lib/x86_64-linux-gnu/libpython3.9.so");

            if (!PythonEngine.IsInitialized)
            {
                PythonEngine.Initialize();
                Console.WriteLine("Python Engine is running");
            }

            using (Py.GIL())
            {
                var pythonScript = Py.Import("\\RecipeRecollection\\Controllers\\findRecipe");
                var url = new PyString(recipe.Url);
                Console.WriteLine("B4 Soup is running");
                var result = pythonScript.InvokeMethod("RUNTHESOUP", new PyObject[] { url });
                Console.WriteLine(result);
            }

            recipe.User = User.FindFirstValue(ClaimTypes.NameIdentifier);
            System.Diagnostics.Debug.WriteLine(recipe.User);
            if (!ModelState.IsValid)
            {
                // Print out model state errors
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
                    }
                }
                return View(recipe);
            }
            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("Is valid");
                _context.Add(recipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recipe);
        }

        // GET: Recipes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }
            return View(recipe);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("recipeID,User,Name,Ingredients,Steps,Url")] Recipe recipe)
        {
            if (id != recipe.recipeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.recipeID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(recipe);
        }

        // GET: Recipes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .FirstOrDefaultAsync(m => m.recipeID == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Recipes == null)
            {
                return Problem("Entity set 'RecipeRecollectionContext.Recipes'  is null.");
            }
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe != null)
            {
                _context.Recipes.Remove(recipe);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeExists(int id)
        {
            return (_context.Recipes?.Any(e => e.recipeID == id)).GetValueOrDefault();
        }
    }
}