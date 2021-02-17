using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Meal_Planner.Models
{
    public class IngredientsModel
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Aisle { get; set; }
        public int Quantity { get; set; }
        [MaxLength(100)]
        public string Consistency { get; set; }
        [MaxLength(100)]
        public string Image { get; set; }
        public int MetricAmount { get; set; }
        [MaxLength(100)]
        public string MetricAmountUnit { get; set; }
        public int ImperialAmount { get; set; }
        [MaxLength(100)]
        public string ImperialUnit { get; set; }
        [MaxLength(100)]
        public string Original { get; set; }
    }
}
