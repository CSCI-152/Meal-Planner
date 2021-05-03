using Meal_Planner.Controllers;
using Meal_Planner.Data;
using Meal_Planner.Models;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;

namespace Unit_Testing
{
    public class Tests
    {
        [Test]
        public void PieChartTest_NotNull()
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
        public void BaseTest_AlwaysEqual()
        {
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
        public void MealPlanUser_ValidModel()
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
        public void MealPlanAddModel_ValidModel()
        {
            // Arrange
            MockMealPlanList mockNew = new();
            MealPlanAddModel model = new();

            // Act
            MealPlanAddModel repo = mockNew.GetRecipes();

            // Assert
            Assert.AreSame(model.GetType(), repo.GetType());
            Assert.AreNotEqual(new MealPlanAddModel(), mockNew.GetRecipes());
        }
    }
}
