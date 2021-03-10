using Meal_Planner.Data;
using Meal_Planner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Meal_Planner.Controllers
{
    public class MealPlanController : Controller
    {
        private readonly SpoonacularApi _options;
        private readonly ApplicationDbContext _context;
        public MealPlanController(IOptions<SpoonacularApi> options, ApplicationDbContext context)
        {
            _options = options.Value;
            _context = context;
        }

        [Route("Plan")]
        public IActionResult Index()
        {
            RandomSpoonacularApiKey d = new();
            ViewData["key"] = d.Generate();
            return View(_options);
        }

        // POST: Recipe/UpdateApi/5
        [HttpPost]
        [Route("Plan/Initialize")]
        public async Task<JsonResult> Register([Bind("Username,Hash,ApiKey")] MealPlanUser user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return Json(user);
            }
            return Json("User Already Exists");
        }

        [HttpPost]
        [Route("Plan/Add")]
        public async Task<JsonResult> Add(int? id)
        {
            if (id > 0)
            {
                var key = new RandomSpoonacularApiKey().Generate();
                var data = new MockMealPlanList().GetRecipes();//Mock Data
                var dataJson = JsonConvert.SerializeObject(data);
                var content = new StringContent(dataJson, Encoding.UTF8, "application/json");

                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://api.spoonacular.com/mealplanner/api-76891-pizza/items?apiKey=cc3a1f5670574b379c682e1e1ce052af&hash=ce93ada0e3fda7ef5a211cc5bdb5ddb33f9257e8"),
                    Content = content
                };
                using (var response = await client.SendAsync(request))
                {
                   // response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsStringAsync();
                    //Get header values and save them to our RequestLimit
                    //var headerLimit = response.Headers.GetValues("x-ratelimit-requests-remaining").FirstOrDefault();

                   // RecipeModel recipeRequest = JsonConvert.DeserializeObject<RecipeModel>(result);

                    //   _context.Add(recipeRequest);//details 966429 and 578451 breaks model         // this saves the data to the DB, removed because it caused crashes
                    //   await _context.SaveChangesAsync();
                    //Add result to our database
                    return Json(result);
                }
            }
            return Json("Error");
        }
    }
}
