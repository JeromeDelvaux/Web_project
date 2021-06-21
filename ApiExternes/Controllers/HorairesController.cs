using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MesContexts.Models;
using MesModels.Models;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorairesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HorairesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Horaires
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Horaires>>> GetHoraires()
        {
            return await _context.Horaires.ToListAsync();
        }

        // GET: api/Horaires/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Horaires>> GetHoraires(int id)
        {
            var horaires = await _context.Horaires.FindAsync(id);

            if (horaires == null)
            {
                return NotFound();
            }

            return horaires;
        }

        // PUT: api/Horaires/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHoraires(int id, Horaires horaires)
        {
            if (id != horaires.Id)
            {
                return BadRequest();
            }

            _context.Entry(horaires).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HorairesExists(id))
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

        // POST: api/Horaires
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Horaires>> PostHoraires(Horaires horaires)
        {
            _context.Horaires.Add(horaires);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHoraires", new { id = horaires.Id }, horaires);
        }

        // DELETE: api/Horaires/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Horaires>> DeleteHoraires(int id)
        {
            var horaires = await _context.Horaires.FindAsync(id);
            if (horaires == null)
            {
                return NotFound();
            }

            _context.Horaires.Remove(horaires);
            await _context.SaveChangesAsync();

            return horaires;
        }

        private bool HorairesExists(int id)
        {
            return _context.Horaires.Any(e => e.Id == id);
        }
    }
}
