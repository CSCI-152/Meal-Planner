using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Meal_Planner.Models
{
    // Add profile data for application users by adding properties to the User class
    public class UserModel : IdentityUser
    {
        [PersonalData]
        [MaxLength(100)]
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        [MaxLength(100)]
        public string Gender { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        [MaxLength(100)]
        public string DietPreferences { get; set; }
        [MaxLength(100)]
        public string GlutenIntolerance { get; set; }
        public MealPlanUser SpoonAccount { get; set; }
        public ICollection<UserMealPlan> Meals { get; set; }
    }
}
