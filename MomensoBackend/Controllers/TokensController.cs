using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MomensoBackend.Data;
using MomensoBackend.Models;
using RowcallBackend.Models;
using TokenGenerator;

namespace RowcallBackend.Controllers
{
    [Produces("application/json")]
    [Route("api/Tokens")]
    public class TokensController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager; 

        public TokensController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager; 
        }

        // GET: api/Tokens
        [HttpGet]
        public async Task<IEnumerable<Token>> GetToken()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var tokens = _context.Token.Include(x => x.UserToken).Where(x => x.UserToken.Any(p => p.ApplicationUserId == currentUser.Id)); 
            return tokens;
        }

        // GET: api/Tokens/5
        [HttpGet("{id}")]
        public IActionResult GetToken([FromRoute] int id, int classId)
        {
            WebService1SoapClient client = new WebService1SoapClient(
                new BasicHttpBinding(BasicHttpSecurityMode.None),
                new EndpointAddress("http://localhost/SOAPTokenGenerator/TokenGenerator.asmx")
                );

            Token token = new Token
            {
                Value = client.GenToken(),
                Duration = 30,
                CreatedDateTime = DateTime.Now,
                ClassId = classId
            };

            return Ok(token);
        }

        [HttpPost]
        public IActionResult CreateToken([FromBody] TokenDto tokenDto)
        {
            WebService1SoapClient client = new WebService1SoapClient(
                new BasicHttpBinding(BasicHttpSecurityMode.None),
                new EndpointAddress("http://localhost/SOAPTokenGenerator/TokenGenerator.asmx")
                );

            Token token = new Token
            {
                Value = client.GenToken().ToString(),
                Duration = 30,
                CreatedDateTime = DateTime.Now,
                ClassId = tokenDto.ClassId
            };
            _context.Token.Add(token);
            _context.SaveChanges();
            return Ok(token); 
        }


        private bool TokenExists(int id)
        {
            return _context.Token.Any(e => e.Id == id);
        }
    }

}