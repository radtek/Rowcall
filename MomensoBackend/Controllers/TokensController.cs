﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MomensoBackend.Data;
using RowcallBackend.Models;

namespace RowcallBackend.Controllers
{
    public class TokensController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TokensController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tokens
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Token.Include(t => t.ClassRoom);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Tokens/Details/5
        public async Task<IActionResult> Details(int? id)
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

            return View(token);
        }

        // GET: Tokens/Create
        public IActionResult Create()
        {
            ViewData["ClassId"] = new SelectList(_context.ClassRoom, "Id", "Id");
            return View();
        }

        // POST: Tokens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClassId,Duration,CreatedDateTime")] Token token)
        {
            if (ModelState.IsValid)
            {
                _context.Add(token);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassId"] = new SelectList(_context.ClassRoom, "Id", "Id", token.ClassId);
            return View(token);
        }

        // GET: Tokens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var token = await _context.Token.SingleOrDefaultAsync(m => m.Id == id);
            if (token == null)
            {
                return NotFound();
            }
            ViewData["ClassId"] = new SelectList(_context.ClassRoom, "Id", "Id", token.ClassId);
            return View(token);
        }

        // POST: Tokens/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClassId,Duration,CreatedDateTime")] Token token)
        {
            if (id != token.Id)
            {
                return NotFound();
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

            return View(token);
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
