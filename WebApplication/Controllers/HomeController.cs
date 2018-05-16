﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApplication.Models;
using WebApplication.Models.HomeViewModels;

namespace WebApplication.Controllers
{
    [DataContract]
    public class LoginApiData
    {
        [DataMember(Name = "succeded")]
        public bool Succeded { get; set; }

        [DataMember(Name = "response")]
        public string Response { get; set; } 
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

                using(HttpClient client = new HttpClient())
                {
                    var response = await client.PostAsJsonAsync("http://localhost:11743/account/login", new { email, password });
                    var resultString = await response.Content.ReadAsStringAsync();

                    using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(resultString)))
                    {
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(LoginApiData));
                        LoginApiData obj = (LoginApiData)serializer.ReadObject(stream);

                        if (obj.Succeded)
                        {
                            HttpContext.Session.SetString("token", obj.Response);
                            return RedirectToAction("Index", "Teacher");
                        }
                        else
                        {
                            ViewData["Error"] = obj.Response;
                            return View(model); 
                        }
                    }
                }
            }
            var error = ModelState.Values.FirstOrDefault(x => x.ValidationState == ModelValidationState.Invalid).Errors.First().ErrorMessage;
            ViewData["Error"] = error; 
            return View(model);  
        }

        [HttpGet]
        public IActionResult Student()
        {
            ViewData["Message"] = "Your application description page.";
            var model = new StudentViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Student(StudentViewModel model)
        {
            var email = model.Email;
            var password = model.Password;
            var token = model.Token;
            var longitude = model.Longitude;
            var latitude = model.Latitude;
            return View(model); 
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
