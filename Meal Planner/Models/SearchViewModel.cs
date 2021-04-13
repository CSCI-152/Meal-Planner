using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meal_Planner.Models
{
    public class SearchViewModel
    {
        public SpoonacularApi Spoonacular { get; set; }
        public MealPlanAddModel MealPlanAdd { get; set; }

        public SearchViewModel()
        {
            Spoonacular = new SpoonacularApi();
            MealPlanAdd = new MealPlanAddModel();
        }
    }
}
