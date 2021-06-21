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
    public class PhotosEtablissementsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PhotosEtablissementsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/PhotosEtablissements
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhotosEtablissement>>> GetPhotosEtablissements()
        {
            return await _context.PhotosEtablissements.ToListAsync();
        }

        // GET: api/PhotosEtablissements/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PhotosEtablissement>> GetPhotosEtablissement(int id)
        {
            var photosEtablissement = await _context.PhotosEtablissements.FindAsync(id);

            if (photosEtablissement == null)
            {
                return NotFound();
            }

            return photosEtablissement;
        }

        // PUT: api/PhotosEtablissements/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhotosEtablissement(int id, PhotosEtablissement photosEtablissement)
        {
            if (id != photosEtablissement.Id)
            {
                return BadRequest();
            }

            _context.Entry(photosEtablissement).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhotosEtablissementExists(id))
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

        // POST: api/PhotosEtablissements
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<PhotosEtablissement>> PostPhotosEtablissement(PhotosEtablissement photosEtablissement)
        {
            _context.PhotosEtablissements.Add(photosEtablissement);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPhotosEtablissement", new { id = photosEtablissement.Id }, photosEtablissement);
        }

        // DELETE: api/PhotosEtablissements/5

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<PhotosEtablissement>> DeletePhotosEtablissement(int id)
        {
            var photosEtablissement = await _context.PhotosEtablissements.FindAsync(id);
            if (photosEtablissement == null)
            {
                return NotFound();
            }

            _context.PhotosEtablissements.Remove(photosEtablissement);
            await _context.SaveChangesAsync();

            return photosEtablissement;
        }

        private bool PhotosEtablissementExists(int id)
        {
            return _context.PhotosEtablissements.Any(e => e.Id == id);
        }
    }
}
