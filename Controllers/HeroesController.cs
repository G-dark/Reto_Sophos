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
    public class HeroesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HeroesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Heroes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hero>>> GetHeroes()
        {
          if (_context.Heroes == null)
          {
              return NotFound();
          }
            return await _context.Heroes.ToListAsync();
        }

        // GET: api/Heroes/search
        [HttpGet("Search")]
        public async Task<ActionResult<Hero>> GetHero([FromBody] Hero heroes)
        {
          if (_context.Heroes == null)
          {
              return NotFound();
          }
            var hero = await _context.Heroes.FindAsync(heroes.HeroId);

            if (hero == null)
            {
                return NotFound();
            }

            return hero;
        }

        // PUT: api/Heroes/
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHero(int id, Hero hero)
        {
            if (id != hero.HeroId)
            {
                return BadRequest();
            }

            _context.Entry(hero).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HeroExists(id))
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

        // POST: api/Heroes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Hero>> PostHero([FromBody]Hero hero)
        {
          if (_context.Heroes == null)
          {
              return Problem("Entity set 'AppDbContext.Heroes'  is null.");
          }
            try
            {
                _context.Heroes.Add(hero);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex){ 
            
            }
            

            return CreatedAtAction("GetHero", new { id = hero.HeroId }, hero);
        }

        // DELETE: api/Heroes/5
        [HttpDelete("Eliminar")]
        public async Task<IActionResult> DeleteHero([FromBody]Hero heroes)
        {
            if (_context.Heroes == null)
            {
                return NotFound();
            }
            var hero = await _context.Heroes.FindAsync(heroes.HeroId);
            if (hero == null)
            {
                return NotFound();
            }

            _context.Heroes.Remove(hero);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HeroExists(int id)
        {
            return (_context.Heroes?.Any(e => e.HeroId == id)).GetValueOrDefault();
        }
    }
}
