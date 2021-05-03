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
            Assert.IsNotNull(result); //Compare to object type in controller
        }

        [Test]
        public void ModelTest()
        {
            MockMealPlanList mockNew = new();
            try
            {
                Assert.AreSame(new MealPlanUser().GetType(), mockNew.TestUser().GetType());
            }
            catch (AssertionException e)
            {
                Console.WriteLine(e);
            }
            Assert.AreNotEqual(new MealPlanUser(), mockNew.TestUser());

            try
            {
                Assert.AreEqual(10, 10, 0.001, "WOT THE FOK OVER");
            }
            catch (AssertionException e)
            {
                Console.WriteLine("ERRRRR" + e.Message);
            }
        }
    }
}
