using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meal_Planner.Controllers
{
    public class SingleRecipeController : Controller
    {
        public IActionResult SingleRecipe()
        {
            return View();
        }
    }
}
