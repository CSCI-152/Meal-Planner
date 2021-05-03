using Meal_Planner;
using Meal_Planner.Controllers;
using Meal_Planner.Data;
using Meal_Planner.Models;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;

namespace Meal_Planner_UnitTest
{
    public class Tests
    {
        [Test]
        public void PieChartTest()
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
        public void BaseTest()
        {
            try
            {
                Assert.AreEqual(10, 10, 0.001, "WOT THE FOK OVER");
            }
            catch (AssertionException e)
            {
                Console.WriteLine("ERRRRR" + e.Message);
            }
        }

        [Test]
        public void ModelTest()
        {
            // Arrange
            MockMealPlanList mockNew = new();
            MealPlanUser model = new();

            // Act
            MealPlanUser repo = mockNew.TestUser();

            // Assert
            Assert.AreSame(model.GetType(), repo.GetType());
            // Assert.AreNotEqual(new MealPlanUser(), mockNew.TestUser());
        }
    }
}
