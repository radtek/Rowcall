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
            return _context.Token;
        }

        // GET: api/Tokens/5
        [HttpGet("{id}")]
        public IActionResult GetToken([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tokens = _context.Token.Where(x => x.ClassId == id);

            if (tokens.Count() == 0)
            {
                return NotFound();
            }

            return Ok(tokens);
        }

        // PUT: api/Tokens/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToken([FromRoute] int id, [FromBody] Token token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != token.Id)
            {
                return BadRequest();
            }

            _context.Entry(token).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TokenExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Tokens
        [HttpPost]
        public IActionResult PostToken([FromBody] TokenDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            WebService1SoapClient client = new WebService1SoapClient(
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

        // DELETE: api/Tokens/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToken([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = await _context.Token.SingleOrDefaultAsync(m => m.Id == id);
            if (token == null)
            {
                return NotFound();
            }

            _context.Token.Remove(token);
            await _context.SaveChangesAsync();

            return Ok(token);
        }

        private bool TokenExists(int id)
        {
            return _context.Token.Any(e => e.Id == id);
        }
    }
}