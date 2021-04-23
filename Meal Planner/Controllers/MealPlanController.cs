using Meal_Planner.Data;
using Meal_Planner.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        public SearchViewModel SearchModel = new();

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
        // Multiple models in a single view https://stackoverflow.com/questions/23536299/mvc-5-multiple-models-in-a-single-view
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Plan/Add", Name = "AddMeal")]
        public async Task<JsonResult> Add(SearchViewModel addMeal)
        {
            ///  SearchModel.MealPlanAdd = addMeal;
            ///  
            MealPlanAddModel Model = addMeal.MealPlanAdd;

            if (ModelState.IsValid)
            {
                //Get the current user's Id
                var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var userId = claim.Value;

                //Find the current user's account based on Id
                var currentUser = await _context.Users
                    .Include(s => s.SpoonAccount)
                    .Include(n => n.Meals)
                    .Where(m => m.Id == userId)
                    .FirstOrDefaultAsync();


                //get user's API information
                var user = currentUser.SpoonAccount.Username;
                var hash = currentUser.SpoonAccount.Hash;
                var key = currentUser.SpoonAccount.ApiKey;
                //////////
                ///
                JObject obj = JObject.FromObject(new
                {
                    date = EpochTime.ToUnixTime(Model.Date),
                    slot = Model.Slot,
                    position = 0,
                    type = Model.Type,
                    value = JObject.FromObject(new
                    {
                        title = Model.Meal.Title,
                        id = Model.Meal.Id,
                        servings = Model.Meal.Servings,
                        image = Model.Meal.Image
                    })
                });
                ////////
                //Json the meal object
                var dataJson = JsonConvert.SerializeObject(obj);
                var content = new StringContent(dataJson, Encoding.UTF8, "application/json");

                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://api.spoonacular.com/mealplanner/" + user + "/items?apiKey=" + key + "&hash=" + hash),
                    Content = content
                };
                using (var response = await client.SendAsync(request))
                {

                    //{"status":"success","id":4999759}
                    if (response != null)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        //Convert the response into the MealPlanUser model
                        var id = JObject.Parse(result);
                        Console.WriteLine(id.SelectToken("id").Value<string>());
                        UserMealPlan created = new()
                        {
                            RecipeId = id.SelectToken("id").Value<int>(),
                            Date = DateTime.Now
                        };
                        //Add the data to our account
                        currentUser.Meals.Add(created);

                        //Save the changes to the database
                        _context.Entry(currentUser).State = EntityState.Modified;
                        await _context.SaveChangesAsync();

                        return Json(id.SelectToken("id").Value<int>());
                    }
                }
            }
            return Json("Error");
        }
    }
}
