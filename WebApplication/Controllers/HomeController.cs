using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApplication.Models;
using WebApplication.Models.HomeViewModels;

namespace WebApplication.Controllers
{
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; } 
    }

    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var model = new IndexViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(IndexViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = model.Email;
                var password = model.Password;
                var jsonObj = new { email, password }; 



                // Verify here...

                HttpContext.Session.SetString("token", "MyLoginToken"); 

                return RedirectToAction("Index", "Teacher");
            }
            var error = ModelState.Values.FirstOrDefault(x => x.ValidationState == ModelValidationState.Invalid).Errors.First().ErrorMessage;
            ViewData["Error"] = error; 
            return View(model);  
        }

        public IActionResult Student()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
