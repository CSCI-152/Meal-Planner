using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Meal_Planner.Models
{
    public class IngredientsModel //Rework to handle a nested model https://stackoverflow.com/questions/50653807/retrieve-value-from-nested-json
    {
        [Key]
        public Guid IngredientId { get; set; }
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Aisle { get; set; }
        [Column(TypeName = "decimal(6,2)")]
        public decimal Amount { get; set; }
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
