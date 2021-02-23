using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Meal_Planner.Models
{
    [Index(nameof(Id))] //Index the Recipe ID so we can search our localDB first before using spoonacular API
    public class RecipeModel
    {
        [Key]
        public Guid RecipeId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [MaxLength(1000)]
        public string Title { get; set; }
        [MaxLength(250)]
        public string Image { get; set; }
        public int Servings { get; set; }
        public int ReadyInMinutes { get; set; }
        [MaxLength(1000)]
        public string SourceName { get; set; }
        [MaxLength(250)]
        public string SourceUrl { get; set; }
        public int AggregateLikes { get; set; }
        [Column(TypeName = "decimal(6,2)")]
        public decimal HealthScore { get; set; }
        [Column(TypeName = "decimal(6,2)")]
        public decimal PricePerServing { get; set; }
        public bool DairyFree { get; set; }
        public bool GlutenFree { get; set; }
        public bool Vegan { get; set; }
        public bool Vegetarian { get; set; }
        public bool Ketogenic { get; set; }
        [MaxLength(4000)]
        public string Instructions { get; set; }
        public List<IngredientsModel> ExtendedIngredients { get; set; }
    }
}
