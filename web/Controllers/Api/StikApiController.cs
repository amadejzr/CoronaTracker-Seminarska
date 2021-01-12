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
    public class StikApiController : ControllerBase
    {
        private readonly CoronaContext _context;

        public StikApiController(CoronaContext context)
        {
            _context = context;
        }

        // GET: api/StikApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Stik>>> GetStiki()
        {
            return await _context.Stiki.ToListAsync();
        }

        // GET: api/StikApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Stik>> GetStik(int id)
        {
            var stik = await _context.Stiki.FindAsync(id);

            if (stik == null)
            {
                return NotFound();
            }

            return stik;
        }

        // PUT: api/StikApi/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStik(int id, Stik stik)
        {
            if (id != stik.Id)
            {
                return BadRequest();
            }

            _context.Entry(stik).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StikExists(id))
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

        // POST: api/StikApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Stik>> PostStik(Stik stik)
        {
            _context.Stiki.Add(stik);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStik", new { id = stik.Id }, stik);
        }

        // DELETE: api/StikApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Stik>> DeleteStik(int id)
        {
            var stik = await _context.Stiki.FindAsync(id);
            if (stik == null)
            {
                return NotFound();
            }

            _context.Stiki.Remove(stik);
            await _context.SaveChangesAsync();

            return stik;
        }

        private bool StikExists(int id)
        {
            return _context.Stiki.Any(e => e.Id == id);
        }
    }
}
