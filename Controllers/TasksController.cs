﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reto_sophos2.DBContext;
using Reto_sophos2.Models;
using Task = Reto_sophos2.Models.Task;

namespace Reto_sophos2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Task>>> GetTasks()
        {
          if (_context.Tasks == null)
          {
              return NotFound();
          }
            return await _context.Tasks.ToListAsync();
        }

        // GET: api/Tasks/5
        [HttpGet("Search")]
        public async Task<ActionResult<Task>> GetTask([FromBody]Task tasks)
        {
          if (_context.Tasks == null)
          {
              return NotFound();
          }
            var task = await _context.Tasks.FindAsync(tasks.TaskId);

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

        // PUT: api/Tasks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, Task task)
        {
            if (id != task.TaskId)
            {
                return BadRequest();
            }

            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
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

        // POST: api/Tasks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Task>> PostTask([FromBody]Task tasks)
        {
           
                if (_context.Tasks == null)
                {
                    return Problem("Entity set 'AppDbContext.Tasks'  is null.");
                }
                _context.Tasks.Add(tasks);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (TaskExists(tasks.TaskId))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }

            
            
            return CreatedAtAction("GetTask", new { id = tasks.TaskId }, tasks);
        }

        // DELETE: api/Tasks/5
        [HttpDelete("Eliminar")]
        public async Task<IActionResult> DeleteTask([FromBody]Task tasks)
        {
            if (_context.Tasks == null)
            {
                return NotFound();
            }
            var task = await _context.Tasks.FindAsync(tasks.TaskId);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskExists(int id)
        {
            return (_context.Tasks?.Any(e => e.TaskId == id)).GetValueOrDefault();
        }
    }
}
