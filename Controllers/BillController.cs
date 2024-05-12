using Microsoft.AspNetCore.Mvc;
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
        
        // GET: api/Bill/pending
        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<Bill>>> GetPendingBills()
        {
            return await GetBills(false);
        }
        
        // GET: api/Bill/paid
        [HttpGet("paid")]
        public async Task<ActionResult<IEnumerable<Bill>>> GetPaidBills()
        {
            return await GetBills(true);
        }

        private Task<ActionResult<IEnumerable<Bill>>> GetBills(bool isPaid)
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            
            var monthBills = from Bill in _context.Bills
                where Bill.IsPaid == isPaid && Bill.GeneratedAt.Month == currentMonth && Bill.GeneratedAt.Year == currentYear
                select Bill;
            
            return Task.FromResult<ActionResult<IEnumerable<Bill>>>(monthBills.ToList());
        }
    }
}