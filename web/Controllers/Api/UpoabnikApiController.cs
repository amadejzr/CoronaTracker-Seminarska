using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;
using web.Filters;

namespace web.Controllers_Api
{
    [Route("api/[controller]")]
    [ApiController]
     [ApiKeyAuth]
    public class UpoabnikApiController : ControllerBase
    {
        private readonly CoronaContext _context;

        public UpoabnikApiController(CoronaContext context)
        {
            _context = context;
        }

        // GET: api/UpoabnikApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Uporabnik>>> GetUporabniki()
        {
            return await _context.Uporabniki.ToListAsync();
        }

        // GET: api/UpoabnikApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Uporabnik>> GetUporabnik(string id)
        {
            var uporabnik = await _context.Uporabniki.FindAsync(id);

            if (uporabnik == null)
            {
                return NotFound();
            }

            return uporabnik;
        }

        // PUT: api/UpoabnikApi/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUporabnik(string id, Uporabnik uporabnik)
        {
            if (id != uporabnik.Id)
            {
                return BadRequest();
            }

            _context.Entry(uporabnik).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UporabnikExists(id))
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

        // POST: api/UpoabnikApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Uporabnik>> PostUporabnik(Uporabnik uporabnik)
        {
            _context.Uporabniki.Add(uporabnik);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UporabnikExists(uporabnik.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUporabnik", new { id = uporabnik.Id }, uporabnik);
        }

        // DELETE: api/UpoabnikApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Uporabnik>> DeleteUporabnik(string id)
        {
            var uporabnik = await _context.Uporabniki.FindAsync(id);
            if (uporabnik == null)
            {
                return NotFound();
            }

            _context.Uporabniki.Remove(uporabnik);
            await _context.SaveChangesAsync();

            return uporabnik;
        }

        private bool UporabnikExists(string id)
        {
            return _context.Uporabniki.Any(e => e.Id == id);
        }
    }
}
