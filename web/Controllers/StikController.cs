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
using System.ComponentModel.DataAnnotations;


namespace web.Controllers
{
    public class StikController : Controller
    {
        private readonly CoronaContext _context;
        private readonly UserManager<Uporabnik> _userManager;


        public StikController(CoronaContext context,UserManager<Uporabnik> userManager)
        {
            _context = context;
            _userManager = userManager;
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
        public async Task<IActionResult> Create([Bind("Id,Ime,Priimek,Email,IdUser")] Stik stik)
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ime,Priimek,Email,IdUser")] Stik stik)
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
        public async Task<IActionResult> Confirm(int id)
        {

            var stik = await _context.Stiki
                .FirstOrDefaultAsync(m => m.Id == id);
            
            var odlok = new Odlok {DatumZacetka = DateTime.Now, DatumKonca = DateTime.Now.AddDays(14)};
            var user = new Uporabnik { UserName = stik.Email, Email = stik.Email,Ime = stik.Ime,Priimek = stik.Priimek,Odloki = odlok};
                
                var result = await _userManager.CreateAsync(user, "Abc-123");
        

            _context.Stiki.Remove(stik);
            
            //uporabnik.PasswordHash = hashed("Vaje123?");

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        private bool StikExists(int id)
        {
            return _context.Stiki.Any(e => e.Id == id);
        }

        public string hashed(string password){
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            //Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");
    
            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            //Console.WriteLine($"Hashed: {hashed}");
            return hashed;
        
        }
    
    }
}
