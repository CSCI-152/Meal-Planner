using Meal_Planner.Controllers;
using Meal_Planner.Data;
using Meal_Planner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Unit_Testing
{
    public class Tests : Controller
    {
        private ApplicationDbContext _context;

        /**
         * Setup is called before each test is run
         */
        [SetUp]
        public void SetUp()
        {
            //Setup a direct connection to the database for controller test methods
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(@"Server=(localdb)\\mssqllocaldb;Database=Meal_Planner-0B3A25A99;Trusted_Connection=True;MultipleActiveResultSets=true")
            .Options;

            // DBContext is used inside of the controllers constructors
            _context = new ApplicationDbContext(contextOptions);
        }

        [Test]
        public void PieChartController_Dashboard_NotNull()
        {
            // Arrange
            PieChartController controller = new();

            // Act
            ViewResult result = controller.Dashboard() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ViewData["DataPoints"]); //Check if ViewData is set as well
        }

        [Test]
        public void MealPlanController_Index_NotNull()
        {
            // Arrange
            MealPlanController controller = new(_context);

            // Act
            var result = controller.IndexAsync(); 

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void BaseTest_AlwaysEqual()
        {
            // This test should always pass
            // The case is also an example on how to use exception handling for test cases
            try
            {
                Assert.AreEqual(10, 10, 0.001, "This should always work");
            }
            catch (AssertionException e)
            {
                Console.WriteLine("ERRRRR" + e.Message);
            }
        }

        [Test]
        public void MealPlanUser_ValidateModel()
        {
            // Arrange
            MockMealPlanList mockNew = new();
            MealPlanUser model = new();

            // Act
            MealPlanUser repo = mockNew.TestUser();

            // Assert
            Assert.AreSame(model.GetType(), repo.GetType());
            Assert.AreNotEqual(new MealPlanUser(), mockNew.TestUser());
        }

        [Test]
        public void MealPlanAddModel_ValidateModel()
        {
            // Arrange
            MockMealPlanList mockNew = new();
            MealPlanAddModel model = new();

            // Act
            MealPlanAddModel repo = mockNew.GetRecipes();

            // Assert
            Assert.AreSame(model.GetType(), repo.GetType());
            Assert.AreNotEqual(model, mockNew.GetRecipes());
        }

        [Test]
        public void RecipeController_SelectAsync_NotNull()
        {
            // Arrange
            DietController controller = new(_context);

            // Act
            var result = controller.DietSelectAsync();

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void MealPlanController_IndexAsync_NotNull()
        {
            // Arrange
            MealPlanController controller = new(_context);

            // Act
            var result = controller.IndexAsync();

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
