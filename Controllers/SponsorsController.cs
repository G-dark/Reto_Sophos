using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reto_sophos2.DBContext;
using Reto_sophos2.Models;

namespace Reto_sophos2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SponsorsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SponsorsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Sponsors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sponsor>>> GetSponsors()
        {
          if (_context.Sponsors == null)
          {
              return NotFound();
          }
            return await _context.Sponsors.ToListAsync();
        }

        // GET: api/Sponsors/5
        [HttpGet("Search")]
        public async Task<ActionResult<Sponsor>> GetSponsor([FromBody] Sponsor sponsors)
        {
          if (_context.Sponsors == null)
          {
              return NotFound();
          }
            var sponsor = await _context.Sponsors.FindAsync(sponsors.TransId);

            if (sponsor == null)
            {
                return NotFound();
            }

            return sponsor;
        }

        // PUT: api/Sponsors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSponsor(int id, Sponsor sponsor)
        {
            if (id != sponsor.TransId)
            {
                return BadRequest();
            }

            _context.Entry(sponsor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SponsorExists(id))
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

        // POST: api/Sponsors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Crear")]
        public async Task<ActionResult<Sponsor>> PostSponsor([FromBody]Sponsor sponsors)
        {
          if (_context.Sponsors == null)
          {
              return Problem("Entity set 'AppDbContext.Sponsors'  is null.");
          }
            _context.Sponsors.Add(sponsors);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSponsor", new { id = sponsors.TransId }, sponsors);
        }

        // DELETE: api/Sponsors/5
        [HttpDelete("Eliminar")]
        public async Task<IActionResult> DeleteSponsor([FromBody]Sponsor sponsors)
        {
            if (_context.Sponsors == null)
            {
                return NotFound();
            }
            var sponsor = await _context.Sponsors.FindAsync(sponsors.TransId);
            if (sponsor == null)
            {
                return NotFound();
            }

            _context.Sponsors.Remove(sponsor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SponsorExists(int id)
        {
            return (_context.Sponsors?.Any(e => e.TransId == id)).GetValueOrDefault();
        }
    }
}
