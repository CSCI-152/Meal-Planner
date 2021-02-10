using Meal_Planner.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Meal_Planner.Controllers
{
    [Route("api/meals")]
    [ApiController]
    [Produces("application/json")]
    public class MealApiController : ControllerBase
    {
        // GET: api/meals
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/meals/5
        [HttpGet("{id}.{format?}")]
        public ActionResult Get(int id)
        {
            return Ok(id);
        }

        // POST api/meals
        [HttpPost]
        public ActionResult Post([FromBody] MealModel value)
        {
            return Ok(value);
        }

        // PUT api/meals/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/meals/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
