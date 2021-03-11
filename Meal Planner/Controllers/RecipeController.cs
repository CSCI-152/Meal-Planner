﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Meal_Planner.Data;
using Meal_Planner.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;

namespace Meal_Planner.Controllers
{
    public class RecipeController : Controller
    {
        private readonly SpoonacularApi _options;
        private readonly ApplicationDbContext _context;

        public RecipeController(IOptions<SpoonacularApi> options, ApplicationDbContext context)
        {
            _options = options.Value;
            _context = context;
        }

        // GET: Recipe
        public IActionResult Index()
        {
            return View(_options);
        }

        public async Task<IActionResult> Index2()
        {
            return View(await _context.RecipeModel.ToListAsync());
        }

        // GET: Recipe/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeModel = await _context.RecipeModel.Include(a => a.ExtendedIngredients)
                .FirstOrDefaultAsync(m => m.Id == id);

            //Prevent calling the API if we exceed the limit
            if (recipeModel == null && _options.Key.RequestLimit == 0)
            {
                return NotFound();
            }
            if (recipeModel == null)
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://spoonacular-recipe-food-nutrition-v1.p.rapidapi.com/recipes/" + id + "/information"),
                    Headers =
                    {
                        { _options.Key.KeyHeader, _options.Key.ApiKey },
                        {_options.Key.HostHeader, _options.Key.Host },
                        //{ "x-rapidapi-key", "4bf3ec1e54msh58354a1c923bbfap1eb315jsn2b0938461fb2" },
                        //{ "x-rapidapi-host", "spoonacular-recipe-food-nutrition-v1.p.rapidapi.com" },
                    },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsStringAsync();

                    //Get header values and save them to our RequestLimit
                    var headerLimit = response.Headers.GetValues("x-ratelimit-requests-remaining").FirstOrDefault();
                    _options.Key.RequestLimit = Int32.Parse(headerLimit);

                    RecipeModel recipeRequest = JsonConvert.DeserializeObject<RecipeModel>(result);

                    //   _context.Add(recipeRequest);//details 966429 and 578451 breaks model         // this saves the data to the DB, removed because it caused crashes
                    //   await _context.SaveChangesAsync();
                    //Add result to our database
                    return View(recipeRequest);
                }
            }
            return View(recipeModel);
        }

        // GET: Recipe/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recipe/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RecipeId,Id,Title,Image,Servings,ReadyInMinutes,SourceName,SourceUrl,AggregateLikes,HealthScore,PricePerServing,DairyFree,GlutenFree,Vegan,Vegetarian,Ketogenic,Instructions")] RecipeModel recipeModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recipeModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recipeModel);
        }

        // GET: Recipe/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeModel = await _context.RecipeModel.FindAsync(id);
            if (recipeModel == null)
            {
                return NotFound();
            }
            return View(recipeModel);
        }

        // POST: Recipe/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RecipeId,Title,Image,Servings,ReadyInMinutes,SourceName,SourceUrl,AggregateLikes,HealthScore,PricePerServing,DairyFree,GlutenFree,Vegan,Vegetarian,Ketogenic,Instructions")] RecipeModel recipeModel)
        {
            if (id != recipeModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipeModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeModelExists(recipeModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(recipeModel);
        }

        // GET: Recipe/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeModel = await _context.RecipeModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipeModel == null)
            {
                return NotFound();
            }

            return View(recipeModel);
        }

        // POST: Recipe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipeModel = await _context.RecipeModel.FindAsync(id);
            _context.RecipeModel.Remove(recipeModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Recipe/UpdateApi/5
        [HttpPost]
        public IActionResult UpdateApi([Bind("RequestLimit")] KeySettings setr)
        {
            if (setr.RequestLimit >= 0)
            {
                _options.Key.RequestLimit = setr.RequestLimit;
                return Json("OK " + setr.RequestLimit);
            }
            else
                return NotFound();
        }

        private bool RecipeModelExists(int id)
        {
            return _context.RecipeModel.Any(e => e.Id == id);
        }
    }
}
