using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meal_Planner.Models
{
    public class UserMealPlan
    {
        public Guid Id { get; set; }
        public int RecipeId { get; set; }
        public DateTime Date { get; set; }
    }
}
