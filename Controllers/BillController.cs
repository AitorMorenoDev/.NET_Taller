using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Taller.Data;
using Taller.Models;

namespace Taller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController: ControllerBase {
        private readonly ApplicationDBContext _context;

        public BillController(ApplicationDBContext context)
        {
            _context = context;
        }
        
        /**
         * GET: api/Bill/pending
         * Gets all bills that are not paid for the current month
         * @return A list of bills
         */
        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<Bill>>> GetPendingBills()
        {
            return await GetBills(false);
        }
        
        /**
         * GET: api/Bill/paid
         * Gets all bills that are paid for the current month
         * @return A list of bills
         */
        [HttpGet("paid")]
        public async Task<ActionResult<IEnumerable<Bill>>> GetPaidBills()
        {
            return await GetBills(true);
        }
        
        /**
         * Auxiliary method for getting bills endpoints
         * Gets all bills that are paid or not paid for the current month
         * @param isPaid True if the bills are paid, false otherwise
         * @return A list of bills
         */
        private Task<ActionResult<IEnumerable<Bill>>> GetBills(bool isPaid)
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            
            var monthBills = from Bill in _context.Bills
                where Bill.IsPaid == isPaid && Bill.GeneratedAt.Month == currentMonth && Bill.GeneratedAt.Year == currentYear
                select Bill;
            
            return Task.FromResult<ActionResult<IEnumerable<Bill>>>(monthBills.ToList());
        }
        
        /**
         * PUT: api/Bill/5
         * Closes a bill by setting it as paid and setting all services as finished
         * @param id The id of the bill to close
         * @return No content
         */
        [HttpPut("{id:int}")]
        public async Task<IActionResult> CloseBill(int id)
        {
            // Find the bill by id
            var bill = await _context.Bills.Include(b => b!.Services).FirstOrDefaultAsync(b => b != null && b.Id == id);
            
            if (bill == null)
            {
                return NotFound();
            }
            
            // Set the bill as paid
            bill.IsPaid = true;
            
            
            // Set all services as finished
            foreach (var service in bill.Services)
            {
                service.Status = Status.Finished;
                service.DateFinished = DateTime.Now;
            }
            
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /**
         * DELETE: api/Bill/5
         * Deletes a bill
         * @param id The id of the bill to delete
         * @return No content
         */
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBill(int id)
        {
            // Find the bill by id
            var bill = await _context.Bills.FindAsync(id);

            if (bill == null)
            {
                return NotFound();
            }

            // Remove the bill from the database
            _context.Bills.Remove(bill);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}