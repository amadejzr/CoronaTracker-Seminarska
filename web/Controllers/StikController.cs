using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Web;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;


namespace web.Controllers
{
    public class StikController : Controller
    {
        private readonly CoronaContext _context;

        public StikController(CoronaContext context)
        {
            _context = context;
        }

        // GET: Stik
        public async Task<IActionResult> Index()
        {
            var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["userId"] = userId;
            return View(await _context.Stiki.ToListAsync());
        }

        // GET: Stik/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stik = await _context.Stiki
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stik == null)
            {
                return NotFound();
            }

            return View(stik);
        }

        // GET: Stik/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stik/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ime,Priimek,Email,Telefon,IdUser")] Stik stik)
        {
            if (ModelState.IsValid)
            {
                var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);
                stik.IdUser = userId;
                _context.Add(stik);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stik);
        }

        // GET: Stik/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stik = await _context.Stiki.FindAsync(id);
            if (stik == null)
            {
                return NotFound();
            }
            return View(stik);
        }

        // POST: Stik/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ime,Priimek,Email,Telefon,IdUser")] Stik stik)
        {
            if (id != stik.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StikExists(stik.Id))
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
            return View(stik);
        }

        // GET: Stik/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stik = await _context.Stiki
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stik == null)
            {
                return NotFound();
            }

            return View(stik);
        }

        // POST: Stik/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stik = await _context.Stiki.FindAsync(id);
            _context.Stiki.Remove(stik);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StikExists(int id)
        {
            return _context.Stiki.Any(e => e.Id == id);
        }
    }
}
