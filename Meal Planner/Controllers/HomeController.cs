using Meal_Planner.Models;
using Meal_Planner.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Meal_Planner.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [AllowAnonymous] //Allow the user to see the page without logging in
        public IActionResult Index()
        {
            return View();
        }

        [Route("Diet-Info")]
        [AllowAnonymous]
        public IActionResult DietInfo()
        {

            return View();
        }

        [Route("Dashboard")]
        public async Task<IActionResult> Dashboard()
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
            else
            {
                //get user's API information
                var user = currentUser.SpoonAccount.Username;
                var hash = currentUser.SpoonAccount.Hash;
                var key = currentUser.SpoonAccount.ApiKey;
                var date = DateTime.Now;

                ////////
                //Json the meal object
                using (var client = new HttpClient())//2020-06-01
                {
                    var _url = "https://api.spoonacular.com/mealplanner/" + user + "/day/" + date.Year + "-" + date.Month + "-" + date.Day + "?hash=" + hash + "&apiKey=" + key;
                    //Console.Write(_url);
                    var response = await client.GetAsync(_url);

                    //If the response is successful
                    if (response != null)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        //Convert the response into the MealPlanUser model
                        var id = JObject.Parse(result);

                        if ((string)id.SelectToken("status") != "failure")  // check that meal plan has data
                        { 
                            Console.Write(result);
                            //Add the data to our account
                            List<DataPoint> dataPoints = new List<DataPoint>();

                            dataPoints.Add(new DataPoint("Carbohydrates", (int)id.SelectToken("nutritionSummary")["nutrients"][2]["amount"]));    // controller will need to get actual values to fill in for constants
                            dataPoints.Add(new DataPoint("Fats", (int)id.SelectToken("nutritionSummary")["nutrients"][1]["amount"]));
                            dataPoints.Add(new DataPoint("Protein", (int)id.SelectToken("nutritionSummary")["nutrients"][3]["amount"]));
                            ViewData["Calories"] = (int)id.SelectToken("nutritionSummary")["nutrients"][0]["amount"];
                            ViewData["DataPoints"] = JsonConvert.SerializeObject(dataPoints);
                        }
                        else
                        {
                            List<DataPoint> dataPoints = new List<DataPoint>();

                            dataPoints.Add(new DataPoint("Carbohydrates", 1));    // controller will need to get actual values to fill in for constants
                            dataPoints.Add(new DataPoint("Fats", 1));
                            dataPoints.Add(new DataPoint("Protein", 1));
                        }
                    }
                }
            }
            return View();
        }

        public IActionResult TestApi()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}