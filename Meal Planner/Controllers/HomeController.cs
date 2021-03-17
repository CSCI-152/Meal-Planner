using Meal_Planner.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Meal_Planner.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
        public IActionResult Dashboard()
        {
            List<DataPoint> dataPoints = new List<DataPoint>();

            dataPoints.Add(new DataPoint("Carbohydrates", 25));    // controller will need to get actual values to fill in for constants
            dataPoints.Add(new DataPoint("Fats", 13));
            dataPoints.Add(new DataPoint("Protein", 8));

            ViewData["DataPoints"] = JsonConvert.SerializeObject(dataPoints);
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
