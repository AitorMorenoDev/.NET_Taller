using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Taller.Data;
using Taller.Models;


namespace Taller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public WorkerController(ApplicationDBContext context)
        {
            _context = context;
        }

        /**
         * GET: api/Worker
         * Gets all workers
         * @return A list of workers
         */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Worker>>> GetWorkers()
        {
            return await _context.Workers.ToListAsync();
        }

        /**
         * GET: api/Worker/5
         * Gets a worker by id
         * @param id The id of the worker
         * @return The worker
         */
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Worker>> GetWorkerById(int id)
        {
            // Find the worker by id
            var worker = await _context.Workers.FindAsync(id);

            if (worker == null)
            {
                return NotFound();
            }

            // Return the worker
            return worker;
        }

        /**
         * PUT: api/Worker/5
         * Updates a worker
         * @param id The id of the worker
         * @param worker The updated worker
         * @return No content
         */
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateWorker(int id, Worker worker)
        {
            // Check if the id is different from the worker id
            if (id != worker.Id)
            {
                return BadRequest();
            }

            // Update the worker properties
            _context.Entry(worker).State = EntityState.Modified;

            // Save the changes
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            { // Check if the worker exists
                if (!WorkerExists(id))
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

        /**
         * POST: api/Worker
         * Adds a worker
         * @param worker The worker to add
         * @return The worker
         */
        [HttpPost]
        public async Task<ActionResult<Worker>> AddWorker(Worker worker)
        {
            _context.Workers.Add(worker);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWorkerById", new { id = worker.Id }, worker);
        }

        /**
         * DELETE: api/Worker/5
         * Deletes a worker
         * @param id The id of the worker
         * @return No content
         */
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteWorker(int id)
        {
            // Find the worker by id
            var worker = await _context.Workers.FindAsync(id);
            if (worker == null)
            {
                return NotFound();
            }

            // Remove the worker from the database
            _context.Workers.Remove(worker);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /**
         * Auxiliary method
         * Checks if a worker exists
         * @param id The id of the worker
         * @return True if the worker exists, false otherwise
         */
        private bool WorkerExists(int id)
        {
            return _context.Workers.Any(e => e.Id == id);
        }
    }
}
