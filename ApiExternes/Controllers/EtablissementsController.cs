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
    public class EtablissementsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EtablissementsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Etablissements
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Etablissement>>> GetEtablissements()
        {
            return await _context.Etablissements.ToListAsync();
        }

        // GET: api/Etablissements/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Etablissement>> GetEtablissement(int id)
        {
            var etablissement = await _context.Etablissements.FindAsync(id);

            if (etablissement == null)
            {
                return NotFound();
            }

            return etablissement;
        }

        // PUT: api/Etablissements/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEtablissement(int id, Etablissement etablissement)
        {
            if (id != etablissement.Id)
            {
                return BadRequest();
            }

            _context.Entry(etablissement).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EtablissementExists(id))
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

        // POST: api/Etablissements
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Etablissement>> PostEtablissement(Etablissement etablissement)
        {
            _context.Etablissements.Add(etablissement);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEtablissement", new { id = etablissement.Id }, etablissement);
        }

        // DELETE: api/Etablissements/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Etablissement>> DeleteEtablissement(int id)
        {
            var etablissement = await _context.Etablissements.FindAsync(id);
            if (etablissement == null)
            {
                return NotFound();
            }

            _context.Etablissements.Remove(etablissement);
            await _context.SaveChangesAsync();

            return etablissement;
        }

        private bool EtablissementExists(int id)
        {
            return _context.Etablissements.Any(e => e.Id == id);
        }
    }
}
