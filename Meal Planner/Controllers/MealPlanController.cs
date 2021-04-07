using Meal_Planner.Data;
using Meal_Planner.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Meal_Planner.Controllers
{
    public class MealPlanController : Controller
    {
        private readonly ApplicationDbContext _context;
        public MealPlanController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("Plan")]
        public async Task<IActionResult> IndexAsync()
        {
            //Get the current user's Id
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claim.Value;

            //Find the current user's account based on Id
            var currentUser = await _context.Users
                .Include(s => s.SpoonAccount)
                .Where(m => m.Id == userId)
                .FirstOrDefaultAsync();

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
            //Send the current user to the view to access API data
            return View();
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
