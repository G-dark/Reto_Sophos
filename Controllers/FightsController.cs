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
    public class FightsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FightsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Fights
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fight>>> GetFights()
        {
            if (_context.Fights == null)
            {
                return NotFound();
            }
            return await _context.Fights.ToListAsync();
        }

        // GET: api/Fights/5
        [HttpGet("Search")]
        public async Task<ActionResult<Fight>> GetFight([FromBody] Fight fights)
        {
            if (_context.Fights == null)
            {
                return NotFound();
            }
            var fight = await _context.Fights.FindAsync(fights.FightId);

            if (fight == null)
            {
                return NotFound();
            }

            return fight;
        }

        // PUT: api/Fights/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFight(int id, Fight fight)
        {
            if (id != fight.FightId)
            {
                return BadRequest();
            }

            _context.Entry(fight).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FightExists(id))
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

        // POST: api/Fights
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Crear")]
        public async Task<ActionResult<Fight>> PostFight([FromBody] Fight fights)
        {
            if (_context.Fights == null)
            {
                return Problem("Entity set 'AppDbContext.Fights'  is null.");
            }
            _context.Fights.Add(fights);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFight", new { id = fights.FightId }, fights);
        }

        // DELETE: api/Fights/5
        [HttpDelete("Elimininar")]
        public async Task<IActionResult> DeleteFight([FromBody] Fight fights)
        {
            if (_context.Fights == null)
            {
                return NotFound();
            }
            var fight = await _context.Fights.FindAsync(fights.FightId);
            if (fight == null)
            {
                return NotFound();
            }

            _context.Fights.Remove(fight);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FightExists(int id)
        {
            return (_context.Fights?.Any(e => e.FightId == id)).GetValueOrDefault();
        }
    }
}
