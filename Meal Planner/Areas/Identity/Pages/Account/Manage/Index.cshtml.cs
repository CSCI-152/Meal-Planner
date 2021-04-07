using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Meal_Planner.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Meal_Planner.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;

        public IndexModel(
            UserManager<UserModel> userManager,
            SignInManager<UserModel> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        //Add additional usermodel parts here
        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Full name")]
            public string Name { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [Display(Name="Date of birth")]
            public DateTime Birthday { get; set; }

            [Required]
            [Display(Name = "Height in cm")]
            public int Height { get; set; }

            [Required]
            [Display(Name = "Weight in kg")]
            public int Weight { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Gender")]
            public string Gender { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Diet Preference")]
            public string Diet { get; set; }
        }

        private async Task LoadAsync(UserModel user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                Name = user.Name,
                Birthday = user.Birthday,
                Gender = user.Gender,
                Height = user.Height,
                Weight = user.Weight,
                Diet = user.DietPreferences
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            //Not using phone number
            //var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            /*if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }*/

            if (Input.Name != user.Name)
            {
                user.Name = Input.Name;
            }

            if (Input.Birthday != user.Birthday) //TODO Compare dates properly
            {
                user.Birthday = Input.Birthday;
            }

            if(Input.Gender != user.Gender)
            {
                user.Gender = Input.Gender;
            }

            if (Input.Height != user.Height)
            {
                user.Height = Input.Height;
            }

            if (Input.Weight != user.Weight)
            {
                user.Weight = Input.Weight;
            }

            if (Input.Diet != user.DietPreferences)
            {
                user.DietPreferences = Input.Diet;
            }

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
