using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;

namespace web.Controllers
{
    public class UporabnikController : Controller
    {
        private readonly CoronaContext _context;
        private readonly UserManager<Uporabnik> _usermanager;

        public UporabnikController(CoronaContext context, UserManager<Uporabnik> userManager)
        {
            _context = context;
            _usermanager = userManager;
        }

        // GET: Uporabnik
        [Authorize(Roles = "Administrator,Inspektor")]
        public async Task<IActionResult> Index(string sortOrder,string searchString)
        {

            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Mesto" ? "Mesto_desc" : "Mesto";
            ViewData["CurrentFilter"] = searchString;

            var uporabniks = from m in _context.Uporabniki.Include(u => u.Odloki).Include(c => c.Prebivalisca)
                 select m;

        if (!String.IsNullOrEmpty(searchString))
    {
        uporabniks = uporabniks.Where(s => s.Priimek.Contains(searchString)
                               || s.Ime.Contains(searchString));
    }

        switch (sortOrder)
    {
        case "name_desc":
            uporabniks = uporabniks.OrderByDescending(s => s.Ime);
            break;
        case "Mesto":
            uporabniks = uporabniks.OrderBy(s => s.Prebivalisca.Mesto);
            break;
        case "mesto_desc":
            uporabniks = uporabniks.OrderByDescending(s => s.Prebivalisca.Mesto);
            break;
        default:
            uporabniks = uporabniks.OrderBy(s => s.Prebivalisca.Mesto);
            break;
    }

            
        
        return View(await uporabniks.AsNoTracking().ToListAsync());
            
        }

        // GET: Uporabnik/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uporabnik = await _context.Uporabniki.Include(u => u.Odloki).Include(c => c.Prebivalisca)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uporabnik == null)
            {
                return NotFound();
            }

            return View(uporabnik);
        }

        public async Task<IActionResult> Konec()
        {

            var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["userId"] = userId;
            var lol = await _context.Uporabniki.Include(u => u.Odloki).Include(c => c.Prebivalisca).ToListAsync();
            return View(lol);

        }

        // GET: Uporabnik/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Uporabnik/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Create([Bind("Ime,Priimek,Telefon,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] Uporabnik uporabnik)
        {
            if (ModelState.IsValid)
            {
                _context.Add(uporabnik);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(uporabnik);
        }

        // GET: Uporabnik/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uporabnik = await _context.Uporabniki.FindAsync(id);
            if (uporabnik == null)
            {
                return NotFound();
            }
            return View(uporabnik);
        }

        // POST: Uporabnik/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Ime,Priimek,Telefon,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] Uporabnik uporabnik)
        {
            if (id != uporabnik.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(uporabnik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UporabnikExists(uporabnik.Id))
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
            return View(uporabnik);
        }

        // GET: Uporabnik/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uporabnik = await _context.Uporabniki
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uporabnik == null)
            {
                return NotFound();
            }

            return View(uporabnik);
        }

        // POST: Uporabnik/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var uporabnik = await _context.Uporabniki.FindAsync(id);
            _context.Uporabniki.Remove(uporabnik);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UporabnikExists(string id)
        {
            return _context.Uporabniki.Any(e => e.Id == id);
        }
    }
}
