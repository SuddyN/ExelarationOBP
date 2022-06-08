using ExelarationOBPAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExelarationOBPAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController: ControllerBase {
        private readonly CountryStateContext _context;

        public CountryController(CountryStateContext context) {
            _context = context;
        }

        // GET: api/Country
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Country>>> GetCountrys() {
            if (_context.Countrys == null) {
                return NotFound();
            }
            return await _context.Countrys.ToListAsync();
        }

        // GET: api/Country/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Country>> GetCountry(long id) {
            if (_context.Countrys == null) {
                return NotFound();
            }
            var country = await _context.Countrys.FindAsync(id);

            if (country == null) {
                return NotFound();
            }

            return country;
        }

        // GET: api/Country/5/States
        [HttpGet("{countryID}/States")]
        public async Task<ActionResult<IEnumerable<State>>> GetStatesByCountry(long id) {
            if (_context.States == null || _context.Countrys == null) {
                return NotFound();
            }
            return await _context.States.Where(state => state.countryId.Equals(id)).ToListAsync();
        }

        // PUT: api/Country/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(long id, Country country) {
            if (id != country.id) {
                return BadRequest();
            }

            _context.Entry(country).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!CountryExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Country
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(Country country) {
            if (_context.Countrys == null) {
                return Problem("Entity set 'CountryContext.Countrys'  is null.");
            }
            _context.Countrys.Add(country);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCountry), new { id = country.id }, country);
        }

        // DELETE: api/Country/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(long id) {
            if (_context.Countrys == null) {
                return NotFound();
            }
            var country = await _context.Countrys.FindAsync(id);
            if (country == null) {
                return NotFound();
            }

            _context.Countrys.Remove(country);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CountryExists(long id) {
            return (_context.Countrys?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
