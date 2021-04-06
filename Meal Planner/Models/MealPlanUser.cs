using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Meal_Planner.Models
{
    public class MealPlanUser
    {
        public Guid Id { get; set; }
        [MaxLength(100)]
        [Required]
        public string Username { get; set; }
        [MaxLength(100)]
        [Required]
        public string Hash { get; set; }
        [MaxLength(100)]
        public string ApiKey { get; set; }
    }
}