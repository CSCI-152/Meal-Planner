using Meal_Planner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meal_Planner.Data
{
    public class MockMealPlanList
    {
        public MealPlanAddModel GetRecipes() => new() { Date = DateTime.Now, Position = 0, Slot = 1, Type = "RECIPE", value = Recipe() };

        private ICollection<RecipeInfo> Recipe()
        {
            var list = new List<RecipeInfo>
            {
                new RecipeInfo { Id = 296213, Servings = 2 }
            };
            return list;
        }
    }
}
