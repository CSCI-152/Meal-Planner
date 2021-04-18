using Meal_Planner.Data;
using Meal_Planner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Meal_Planner.Controllers
{
    public class DietController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DietController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> DietSelectAsync()
        {
            //Get the current user's Id
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claim.Value;

            //Find the current user's account based on Id
            var currentUser = await _context.Users
                .Include(s => s.SpoonAccount)
                .Where(m => m.Id == userId)
                .FirstOrDefaultAsync();

            if (currentUser.DietPreferences != null)
            {
                ViewData["diet"] = currentUser.DietPreferences;
            }
            else
            {
                ViewData["diet"] = "N/A";
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Update([Bind("DietPreferences")] UserModel addMeal)
        {
            string preference = addMeal.DietPreferences;

            if (ModelState.IsValid)
            {
                //Get the current user's Id
                var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var userId = claim.Value;

                //Find the current user's account based on Id
                var currentUser = await _context.Users
                    .Include(s => s.SpoonAccount)
                    .Include(n => n.Meals)
                    .Where(m => m.Id == userId)
                    .FirstOrDefaultAsync();

                currentUser.DietPreferences = preference;

                //Save the changes to the database
                _context.Entry(currentUser).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Json(preference);
            }
            return Json("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Intolerance([Bind("GlutenIntolerance")] UserModel addMeal)
        {
            string preference = addMeal.GlutenIntolerance;

            if (ModelState.IsValid)
            {
                //Get the current user's Id
                var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var userId = claim.Value;

                //Find the current user's account based on Id
                var currentUser = await _context.Users
                    .Include(s => s.SpoonAccount)
                    .Include(n => n.Meals)
                    .Where(m => m.Id == userId)
                    .FirstOrDefaultAsync();

                currentUser.GlutenIntolerance = preference;

                //Save the changes to the database
                _context.Entry(currentUser).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Json(preference);
            }
            return Json("Error");
        }
    }
}
