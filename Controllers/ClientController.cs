using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Taller.Data;
using Taller.Models;


namespace Taller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public ClientController(ApplicationDBContext context)
        {
            _context = context;
        }

        /**
         * GET: api/Client
         * Gets all clients
         * @return A list of clients
         */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            return await _context.Clients.ToListAsync();
        }

        /**
         * GET: api/Client/5
         * Gets a client by id
         * @param id The id of the client
         * @return The client
         */
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Client>> GetClientById(int id)
        {
            // Find the client by id
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            // Return the client
            return client;
        }

        /**
         * PUT: api/Client/5
         * Updates a client
         * @param id The id of the client
         * @param client The updated client
         * @return No content
         */
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateClient(int id, Client client)
        {
            // Check if the id is different from the client id
            if (id != client.Id)
            {
                Console.WriteLine("Id cannot be updated. Enter the same id.");
                return BadRequest();
            }

            // Update the client
            _context.Entry(client).State = EntityState.Modified;

            // Save the changes
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            { // Check if the client exists
                if (!ClientExists(id))
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
         * POST: api/Client
         * Adds a client
         * @param client The client to add
         * @return The client
         */
        [HttpPost]
        public async Task<ActionResult<Client>> AddClient(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClientById", new { id = client.Id }, client);
        }

        /**
         * DELETE: api/Client/5
         * Deletes a client
         * @param id The id of the client
         * @return No content
         */
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            // Find the client by id
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            // Remove the client
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /**
         * Auxiliary method
         * Checks if a client exists
         * @param id The id of the client
         * @return True if the client exists, false otherwise
         */
        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
