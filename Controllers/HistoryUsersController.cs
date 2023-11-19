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
    public class HistoryUsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HistoryUsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/HistoryUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HistoryUser>>> GetHistoryUsers()
        {
          if (_context.HistoryUsers == null)
          {
              return NotFound();
          }
            return await _context.HistoryUsers.ToListAsync();
        }

        // GET: api/HistoryUsers/5
        [HttpGet("Search")]
        public async Task<ActionResult<HistoryUser>> GetHistoryUser([FromBody]HistoryUser histories)
        {
          if (_context.HistoryUsers == null)
          {
              return NotFound();
          }
            var historyUser = await _context.HistoryUsers.FindAsync(histories.Username);

            if (historyUser == null)
            {
                return NotFound();
            }

            return historyUser;
        }

        // PUT: api/HistoryUsers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHistoryUser(string id, HistoryUser historyUser)
        {
            if (id != historyUser.Username)
            {
                return BadRequest();
            }

            _context.Entry(historyUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HistoryUserExists(id))
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

        // POST: api/HistoryUsers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Crear")]
        public async Task<ActionResult<HistoryUser>> PostHistoryUser([FromBody]HistoryUser historyUser)
        {
          if (_context.HistoryUsers == null)
          {
              return Problem("Entity set 'AppDbContext.HistoryUsers'  is null.");
          }
            _context.HistoryUsers.Add(historyUser);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (HistoryUserExists(historyUser.Username))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetHistoryUser", new { id = historyUser.Username }, historyUser);
        }

        // DELETE: api/HistoryUsers/5
        [HttpDelete("Eliminar")]
        public async Task<IActionResult> DeleteHistoryUser([FromBody] HistoryUser histories)
        {
            if (_context.HistoryUsers == null)
            {
                return NotFound();
            }
            var historyUser = await _context.HistoryUsers.FindAsync(histories.Username);
            if (historyUser == null)
            {
                return NotFound();
            }

            _context.HistoryUsers.Remove(historyUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HistoryUserExists(string id)
        {
            return (_context.HistoryUsers?.Any(e => e.Username == id)).GetValueOrDefault();
        }
    }
}
