using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Meal_Planner.Data;
using Meal_Planner.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;

namespace Meal_Planner.Controllers
{
    public class RecipeController : Controller
    {
        private readonly SpoonacularApi _options;
        private readonly ApplicationDbContext _context;
        public SearchViewModel Options2 = new(); //Why does this work and not {get;set;} ????
        public RecipeMealPlanModel RpmModel = new();

        public RecipeController(IOptions<SpoonacularApi> options, ApplicationDbContext context)
        {
            _options = options.Value;
            Options2.Spoonacular = _options;
            _context = context;
        }

        // GET: Recipe
        public async Task<IActionResult> IndexAsync()
        {
            /**
             * Check if the user has a spoon account before showing them the search page
             * This is needed so quick add will function properly
             */
            //Get the current user's Id
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claim.Value;

            //Find the current user's account based on Id
            var currentUser = await _context.Users
                .Include(s => s.SpoonAccount)
                .Where(m => m.Id == userId)
                .FirstOrDefaultAsync();

            var currentDiet = currentUser.DietPreferences;
            var glutenIntolerance = currentUser.GlutenIntolerance;

            if (currentDiet != null)
                ViewData["diet"] = currentDiet;

            if (glutenIntolerance != null)
                ViewData["gluten"] = "Gluten";

            //If the SpoonAccount column is null--register an account
            if (currentUser.SpoonAccount == null)
            {
                //Get a Random Spoonacular Key
                var key = new RandomSpoonacularApiKey().Generate();

                //Build data to send
                string json = JsonConvert.SerializeObject(new { username = User.Identity.Name });

                //Api Registration Request
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync(
                        "https://api.spoonacular.com/users/connect?apiKey=" + key,
                         new StringContent(json, Encoding.UTF8, "application/json"));

                    //If the response is successful
                    if (response != null)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        //return Ok(data);
                        //Convert the response into the MealPlanUser model
                        MealPlanUser created = JsonConvert.DeserializeObject<MealPlanUser>(data);
                        created.ApiKey = key;
                        //Add the data to our account
                        currentUser.SpoonAccount = created;

                        //Save the changes to the database
                        _context.Entry(currentUser).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                }
            }
            return View(Options2);
        }

        public async Task<IActionResult> Index2()
        {
            return View(await _context.RecipeModel.ToListAsync());
        }

        // GET: Recipe/Details/5
        public async Task<IActionResult> Details(int? id, string title)
        {
            if (id == null)
            {
                return NotFound();
            }

            RecipeMealPlanModel doubleModel = new();
            var recipeModel = await _context.RecipeModel.Include(a => a.ExtendedIngredients)
                .FirstOrDefaultAsync(m => m.Id == id);

            //Prevent calling the API if we exceed the limit
            if (recipeModel == null && _options.Key.RequestLimit == 0)
            {
                return NotFound();
            }
            if (recipeModel == null)
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://spoonacular-recipe-food-nutrition-v1.p.rapidapi.com/recipes/" + id + "/information"),
                    Headers =
                    {
                        { _options.Key.KeyHeader, _options.Key.ApiKey },
                        {_options.Key.HostHeader, _options.Key.Host },
                        //{ "x-rapidapi-key", "4bf3ec1e54msh58354a1c923bbfap1eb315jsn2b0938461fb2" },
                        //{ "x-rapidapi-host", "spoonacular-recipe-food-nutrition-v1.p.rapidapi.com" },
                    },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsStringAsync();

                    //Get header values and save them to our RequestLimit
                    var headerLimit = response.Headers.GetValues("x-ratelimit-requests-remaining").FirstOrDefault();
                    _options.Key.RequestLimit = Int32.Parse(headerLimit);

                    RecipeModel recipeRequest = JsonConvert.DeserializeObject<RecipeModel>(result);

                    string realTitle = URLFunctions.URLFriendly(recipeRequest.Title);
                    string urlTitle = (title ?? "").Trim().ToLower();

                    if (realTitle != urlTitle)
                    {
                        string url = "/Recipe/" + realTitle + "/" + id;
                        return new RedirectResult(url, true);
                    }
                    doubleModel.Recipe = recipeRequest;
                    //   _context.Add(recipeRequest);//details 966429 and 578451 breaks model         // this saves the data to the DB, removed because it caused crashes
                    //   await _context.SaveChangesAsync();
                    //Add result to our database
                    return View(doubleModel);
                }
            }
            doubleModel.Recipe = recipeModel;
            return View(doubleModel);
        }

        // GET: Recipe/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recipe/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RecipeId,Id,Title,Image,Servings,ReadyInMinutes,SourceName,SourceUrl,AggregateLikes,HealthScore,PricePerServing,DairyFree,GlutenFree,Vegan,Vegetarian,Ketogenic,Instructions")] RecipeModel recipeModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recipeModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recipeModel);
        }

        // GET: Recipe/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeModel = await _context.RecipeModel.FindAsync(id);
            if (recipeModel == null)
            {
                return NotFound();
            }
            return View(recipeModel);
        }

        // POST: Recipe/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RecipeId,Title,Image,Servings,ReadyInMinutes,SourceName,SourceUrl,AggregateLikes,HealthScore,PricePerServing,DairyFree,GlutenFree,Vegan,Vegetarian,Ketogenic,Instructions")] RecipeModel recipeModel)
        {
            if (id != recipeModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipeModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeModelExists(recipeModel.Id))
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
            return View(recipeModel);
        }

        // GET: Recipe/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeModel = await _context.RecipeModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipeModel == null)
            {
                return NotFound();
            }

            return View(recipeModel);
        }

        // POST: Recipe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipeModel = await _context.RecipeModel.FindAsync(id);
            _context.RecipeModel.Remove(recipeModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Recipe/UpdateApi/5
        [HttpPost]
        public IActionResult UpdateApi([Bind("RequestLimit")] KeySettings setr)
        {
            if (setr.RequestLimit >= 0)
            {
                _options.Key.RequestLimit = setr.RequestLimit;
                return Json("OK " + setr.RequestLimit);
            }
            else
                return NotFound();
        }

        private bool RecipeModelExists(int id)
        {
            return _context.RecipeModel.Any(e => e.Id == id);
        }
    }
}
