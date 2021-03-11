using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Meal_Planner.Models;

namespace Meal_Planner.Controllers
{
    public class PieChartController : Controller
    {
		public ActionResult Dashboard()
		{
			List<DataPoint> dataPoints = new List<DataPoint>();

			dataPoints.Add(new DataPoint("Carbohydrates", 25));    // controller will need to get actual values to fill in for constants
			dataPoints.Add(new DataPoint("Fats", 13));
			dataPoints.Add(new DataPoint("Protein", 8));

			ViewData["DataPoints"] = JsonConvert.SerializeObject(dataPoints);

			return View();
		}
	}
}
