using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Meal_Planner.Models;

namespace Meal_Planner.Models
{
    public class PieChartController : Controller
    {
		public ActionResult Index()
		{
			List<ChartDataPoint> dataPoints = new List<ChartDataPoint>();

			dataPoints.Add(new ChartDataPoint("Carbohydrates", 25));    // controller will need to get actual values to fill in for constants
			dataPoints.Add(new ChartDataPoint("Fats", 13));
			dataPoints.Add(new ChartDataPoint("Protein", 8));

			ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

			return View();
		}
	}
}
