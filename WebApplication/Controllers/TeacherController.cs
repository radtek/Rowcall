using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Controllers
{
    public class TeacherController : Controller
    {
        public IActionResult Index()
        {
            ViewData["DisplayToken"] = HttpContext.Session.GetString("token"); 
            return View(); 
        }
    }
}
