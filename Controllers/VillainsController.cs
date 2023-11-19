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
    public class VillainsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VillainsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Villains
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Villain>>> GetVillains()
        {
          if (_context.Villains == null)
          {
              return NotFound();
          }
            return await _context.Villains.ToListAsync();
        }

        // GET: api/Villains/5
        [HttpGet("Search")]
        public async Task<ActionResult<Villain>> GetVillain([FromBody] Villain villains)
        {
          if (_context.Villains == null)
          {
              return NotFound();
          }
            var villain = await _context.Villains.FindAsync(villains.VillainId);

            if (villain == null)
            {
                return NotFound();
            }

            return villain;
        }

        // PUT: api/Villains/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVillain(int id, Villain villain)
        {
            if (id != villain.VillainId)
            {
                return BadRequest();
            }

            _context.Entry(villain).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VillainExists(id))
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

        // POST: api/Villains
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Crear")]
        public async Task<ActionResult<Villain>> PostVillain([FromBody]Villain villains)
        {
          if (_context.Villains == null)
          {
              return Problem("Entity set 'AppDbContext.Villains'  is null.");
          }
            _context.Villains.Add(villains);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVillain", new { id = villains.VillainId }, villains);
        }

        // DELETE: api/Villains/5
        [HttpDelete("Eliminar")]
        public async Task<IActionResult> DeleteVillain([FromBody] Villain villains)
        {
            if (_context.Villains == null)
            {
                return NotFound();
            }
            var villain = await _context.Villains.FindAsync(villains.VillainId);
            if (villain == null)
            {
                return NotFound();
            }

            _context.Villains.Remove(villain);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VillainExists(int id)
        {
            return (_context.Villains?.Any(e => e.VillainId == id)).GetValueOrDefault();
        }
    }
}
