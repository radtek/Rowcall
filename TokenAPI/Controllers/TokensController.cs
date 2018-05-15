using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TokenAPI.Models;
using TokenAPI.Data;
using SOAPservice;



namespace TokenAPI.Controllers
{
    public class TokenAPI : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TokenAPI(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // POST: api/Tokens
        [HttpPost]
        [Route("/api/Tokens/PostToken")]
        public IActionResult PostToken([FromBody] TokenDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            WebService1SoapClient client = new WebService1SoapClient
                (
                 new BasicHttpBinding(BasicHttpSecurityMode.None),
                 new EndpointAddress("http://localhost/SOAPTokenGenerator/TokenGenerator.asmx")
                 );


            var token = new Token()
            {
                Duration = 30,
                ClassId = dto.ClassId,
                CreatedDateTime = DateTime.Now,
                TokenValue = client.GenerateToken()
            };

            _context.Token.Add(token);
            _context.SaveChanges();

            return CreatedAtAction("GetToken", new { id = token.Id }, token);
        }
    }
}