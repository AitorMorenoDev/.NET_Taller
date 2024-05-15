using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        
        /**
         * POST: api/Service
         * Adds a new service to the database
         * @param service The service to be added
         * @return The newly created service
         */
        [HttpPost]
        public async Task<ActionResult<Service>> PostService(Service service)
        {
            // Set the status of the service to accepted and the date created to now
            service.Status = Status.Accepted;
            service.DateCreated = DateTime.UtcNow;
            
            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            // Create a new bill for the service
            // The total amount is the same as the service amount and the bill is not paid
            var bill = new Bill
            {
                GeneratedAt = DateTime.UtcNow,
                IsPaid = false,
                TotalAmount = service.Amount
            };
            _context.Bills.Add(bill);
            bill.Services.Add(service);
        
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
        
        /**
         * POST api/Service/Add
         * Adds a new service to an existing bill
         * @param billId The id of the bill to add the service to
         * @param service The service to be added
         * @return The newly created service
         */
        [HttpPost("Add/{billId:int}")]
        public async Task<ActionResult<Service>> AddService(int billId, Service service)
        {
            // Set the status of the service to accepted and the date created to now
            service.Status = Status.Accepted;
            service.DateCreated = DateTime.UtcNow;
            
            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            // Search for the bill we want to add the service to
            var bill = await _context.Bills.FirstOrDefaultAsync(b => b != null && b.Id == billId);
            if (bill == null)
            {
                return NotFound();
            }

            // Add the service to the bill and update the total amount
            bill.Services.Add(service);
            bill.TotalAmount += service.Amount;

            await _context.SaveChangesAsync();
            
            return NoContent();
        }

        /**
         * PUT api/Service/5
         * Updates a service
         * @param id The id of the service to update
         * @param service The updated service
         * @return No content
         */
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateService(int id, Service updatedService)
        {
            // Find the service by id
            var service = await _context.Services.FindAsync(id);
            
            if (service == null)
            {
                return NotFound();
            }
            
            // Update the client
            _context.Entry(updatedService).State = EntityState.Modified;

            // Save the changes
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            { // Check if the service exists
                if (_context.Services.Any(s => s.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            
            await _context.SaveChangesAsync();
            return NoContent();

        }
        
        /**
         * PUT api/Service/Finish/5
         * Finishes a service, but does not close the bill
         * @param id The id of the service to finish
         * @return No content
         */
        [HttpPut("Finish/{id:int}")]
        public async Task<IActionResult> FinishService(int id)
        {
            // Find the service by id
            var service = await _context.Services.FindAsync(id);
            
            if (service == null)
            {
                return NotFound();
            }

            // Set the service as finished and set the date finished to now
            service.Status = Status.Finished;
            service.DateFinished = DateTime.Now;
            
            // Save the changes
            await _context.SaveChangesAsync();
            return NoContent();
        }
        
        /**
         * DELETE api/Service/5
         * Deletes a service
         * @param id The id of the service to delete
         * @return No content
         */
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            // Find the service by id
            var service = await _context.Services.FindAsync(id);
            
            if (service == null)
            {
                return NotFound();
            }

            // Remove the service from the bill
            var bill = await _context.Bills.Include(b => b!.Services).FirstOrDefaultAsync(b => b != null && b.Id == service.BillId);
            bill?.Services.Remove(service);
            if (bill != null) bill.TotalAmount -= service.Amount;

            // Remove the service from the database
            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        
    }
}

