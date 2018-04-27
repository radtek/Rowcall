using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MomensoBackend.Data;
using MomensoBackend.Models;
using RowcallBackend.Models;

namespace RowcallBackend.Controllers
{
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

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(token);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TokenExists(token.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassId"] = new SelectList(_context.ClassRoom, "Id", "Id", token.ClassId);
            return View(token);
        }

        // GET: Tokens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var token = await _context.Token
                .Include(t => t.ClassRoom)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (token == null)
            {
                return NotFound();
            }

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

        // POST: Tokens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var token = await _context.Token.SingleOrDefaultAsync(m => m.Id == id);
            _context.Token.Remove(token);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TokenExists(int id)
        {
            return _context.Token.Any(e => e.Id == id);
        }
    }
}
