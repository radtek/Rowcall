using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MomensoBackend.Data;
using RowcallBackend.Models;
using TokenGenerator;

namespace RowcallBackend.Controllers
{
    [Produces("application/json")]
    [Route("api/Tokens")]
    public class TokensController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TokensController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Tokens
        [HttpGet]
        public IEnumerable<Token> GetToken()
        {
            return _context.Token;
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


        private bool TokenExists(int id)
        {
            return _context.Token.Any(e => e.Id == id);
        }
    }

}