using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Meal_Planner.Models
{
    public class MealPlanAddModel
    {
        public DateTime Date { get; set; }
        public int Slot { get; set; }
        public int Position { get; set; }
        public string Type { get; set; }
        public RecipeInfo Meal { get; set; }
    }
    public class RecipeInfo
    {
        public int Id { get; set; }
        public int Servings { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
    }
}
