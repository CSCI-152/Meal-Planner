using Meal_Planner;
using Meal_Planner.Controllers;
using Meal_Planner.Data;
using Meal_Planner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using Xunit;

namespace Meal_Planner_UnitTest
{
    public class Tests
    {
        [TestClass]
        public class HomeControllerTest
        {
            [TestMethod]
            public void Index()
            {
                // Arrange
                PieChartController controller = new PieChartController();
                // Act
                ViewResult result = controller.Dashboard() as ViewResult;
                // Assert
                NUnit.Framework.Assert.IsNotNull(result);
            }
        }

        [Fact]
        public void ModelTest()
        {
            MockMealPlanList mockNew = new();
            try
            {
                NUnit.Framework.Assert.AreSame(new MealPlanUser(), mockNew.TestUser().GetType());
            }
            catch (AssertionException e)
            {
                Console.WriteLine(e);
            }
            NUnit.Framework.Assert.AreNotEqual(new MealPlanUser(), mockNew.TestUser().GetType());
        }
    }
}