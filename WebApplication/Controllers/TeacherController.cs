using ClassroomAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using WebApplication.Models.TeacherViewModels;

namespace WebApplication.Controllers
{
    public class TeacherController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("token")))
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                    var response = await client.GetStringAsync("http://localhost:11800/api/classroom");
                    var result = JsonConvert.DeserializeObject<ICollection<ClassRoom>>(response);
                    var model = new IndexViewModel() { ClassRooms = result };
                    return View(model);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> CreateClass(string name)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                var response = await client.PostAsJsonAsync("http://localhost:11800/api/classroom", new { name });
            }
            return RedirectToAction("Index", "Teacher");
        }

        [HttpGet]
        public async Task<IActionResult> Token(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                var url = "http://localhost:11173/api/tokens/" + id; 
                var response = await client.GetStringAsync(url);
                var result = JsonConvert.DeserializeObject<ICollection<Token>>(response);
                var model = new TokenViewModel() { Tokens = result, ClassId = id };
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken(string classid)
        {
            using (HttpClient client = new HttpClient()) 
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                var response = await client.PostAsJsonAsync("http://localhost:11173/api/tokens", new { ClassId = classid });
                return RedirectToAction("Index", "Teacher"); 
            }
        }

        [HttpGet]
        public async Task<IActionResult> Students(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                var url = "http://localhost:11800/api/classroom/getstudentsforclass?classroomid=" + id;
                var response = await client.GetStringAsync(url);
                var result = JsonConvert.DeserializeObject<ICollection<string>>(response);
                var model = new StudentViewModel() { ClassroomId = id, Users = result }; 
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent(string email, string classroomid)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                var response = await client.PostAsJsonAsync("http://localhost:11800/api/classroom/AddUserByEmail", new { ClassRoomId = classroomid, email = email });
                return RedirectToAction("Index", "Teacher");
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("token");
            return RedirectToAction("Index", "Home");
        }
    }
}
