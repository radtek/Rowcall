using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MomensoBackend.Data;
using MomensoBackend.Models;
using RowcallBackend.Models;
using TokenGenerator;

namespace RowcallBackend.Controllers
{
    [Route("api/tokens")]
    public class TokensController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager; 

        public TokensController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager; 
        }

        // GET: api/Tokens/GetToken
        [HttpGet("{classId}")]
        public IActionResult GetToken(int classId)
        {
            WebService1SoapClient client = new WebService1SoapClient(
                 new BasicHttpBinding(BasicHttpSecurityMode.None),
                 new EndpointAddress("http://localhost/SOAPTokenGenerator/TokenGenerator.asmx")
                 );

            Token token = new Token
            {
                Value = client.GenerateToken(),
                Duration = 30,
                CreatedDateTime = DateTime.Now,
                ClassId = classId
            };

            return Ok(token);
        }

    }
}
