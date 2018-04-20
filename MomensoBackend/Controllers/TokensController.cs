using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MomensoBackend.Data;
using RowcallBackend.Models;

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
        public async Task<IActionResult> GetToken([FromRoute] int id)
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

            return Ok(token);
        }
        
        private bool TokenExists(int id)
        {
            return _context.Token.Any(e => e.Id == id);
        }
    }

}