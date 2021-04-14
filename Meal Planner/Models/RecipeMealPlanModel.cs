using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meal_Planner.Models
{
   public class RecipeMealPlanModel
    {
        public RecipeModel Recipe { get; set; }
        public MealPlanAddModel MealPlanAdd { get; set; }

        public RecipeMealPlanModel()
        {
            Recipe = new RecipeModel();
            MealPlanAdd = new MealPlanAddModel();
        }
    }
}
