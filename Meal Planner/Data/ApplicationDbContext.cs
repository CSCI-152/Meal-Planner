using Meal_Planner.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Meal_Planner.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserModel>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<MealModel> Meals { get; set; }

        public DbSet<Meal_Planner.Models.RecipeModel> RecipeModel { get; set; }
    }
}
