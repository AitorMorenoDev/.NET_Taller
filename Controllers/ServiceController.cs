using Microsoft.AspNetCore.Mvc;
using Taller.Data;
using Taller.Models;

namespace Taller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        
        public ServiceController(ApplicationDBContext context)
        {
            _context = context;
        }
        
        // POST api/Service
        [HttpPost]
        public async Task<ActionResult<Service>> PostService(Service service)
        {
            service.Status = Status.Accepted;
            service.DateCreated = DateTime.Now;
            
            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            Bill bill = new Bill
            {
                ServiceId = service.Id,
                GeneratedAt = DateTime.Now,
                IsPaid = false
            };
            
            _context.Bills.Add(bill);
            
            return CreatedAtAction("GetService", new { id = service.Id }, service);
        }
        
        // PUT api/Service/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            
            service.Status = Status.Finished;
            service.DateFinished = DateTime.Now;
            
            // GENERAR FACTURA
            
            await _context.SaveChangesAsync();
            return NoContent();

        }
    }
}