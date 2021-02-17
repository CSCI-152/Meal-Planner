using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Meal_Planner.Models
{
    public class MealModel
    {
        public int Id { get; set; }
        [MaxLength(1000)]
        public string title { get; set; }
        [MaxLength(4000)]
        public string instructions { get; set; }

        //public List<ingredients> {get;set;}
    }
}
