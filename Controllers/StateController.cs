using ExelarationOBPAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExelarationOBPAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class StateController: ControllerBase {
        private readonly CountryStateContext _context;

        public StateController(CountryStateContext context) {
            _context = context;
        }

        // GET: api/State
        [HttpGet]
        public async Task<ActionResult<IEnumerable<State>>> GetStates() {
            if (_context.States == null) {
                return NotFound();
            }
            return await _context.States.ToListAsync();
        }

        // GET: api/State/5
        [HttpGet("{id}")]
        public async Task<ActionResult<State>> GetState(long id) {
            if (_context.States == null) {
                return NotFound();
            }
            var state = await _context.States.FindAsync(id);

            if (state == null) {
                return NotFound();
            }

            return state;
        }

        // PUT: api/State/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutState(long id, State state) {
            if (id != state.id) {
                return BadRequest();
            }

            _context.Entry(state).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!StateExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/State
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<State>> PostState(State state) {
            if (_context.States == null) {
                return Problem("Entity set 'CountryContext.States'  is null.");
            }
            _context.States.Add(state);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetState), new { id = state.id }, state);
        }

        // DELETE: api/State/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteState(long id) {
            if (_context.States == null) {
                return NotFound();
            }
            var state = await _context.States.FindAsync(id);
            if (state == null) {
                return NotFound();
            }

            _context.States.Remove(state);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StateExists(long id) {
            return (_context.States?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
