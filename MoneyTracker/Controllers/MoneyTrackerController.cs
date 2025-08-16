using Microsoft.AspNetCore.Mvc;
using MoneyTracker.Data;
using MoneyTracker.Models;
using System.Diagnostics;

namespace MoneyTracker.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MoneyTrackerController : Controller
    {
        private readonly MoneyTrackerDbContext _context;

        public MoneyTrackerController(MoneyTrackerDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        // GET: api/MoneyTracker/expenses
        [HttpGet("expenses")]
        public ActionResult<IEnumerable<Expense>> GetExpenses()
        {
            var allExpenses = _context.Expenses.ToList();
            return Ok(allExpenses);
        }

        // GET: api/MoneyTracker/expense/{id}
        [HttpGet("expense/{id}")]
        public ActionResult<Expense> GetExpense(int id)
        {
            var expense = _context.Expenses.SingleOrDefault(e => e.Id == id);
            if (expense == null)
                return NotFound();

            return Ok(expense);
        }

        // POST: api/MoneyTracker/expense
        [HttpPost("expense")]
        public IActionResult CreateExpense([FromBody] Expense model)
        {
            if (model == null)
                return BadRequest();

            _context.Expenses.Add(model);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetExpense), new { id = model.Id }, model);
        }

        // PUT: api/MoneyTracker/expense/{id}
        [HttpPut("expense/{id}")]
        public IActionResult UpdateExpense(int id, [FromBody] Expense model)
        {
            if (id != model.Id)
                return BadRequest();

            var expenseInDb = _context.Expenses.SingleOrDefault(e => e.Id == id);
            if (expenseInDb == null)
                return NotFound();

            _context.Entry(expenseInDb).CurrentValues.SetValues(model);
            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/MoneyTracker/expense/{id}
        [HttpDelete("expense/{id}")]
        public IActionResult DeleteExpense(int id)
        {
            var expense = _context.Expenses.SingleOrDefault(e => e.Id == id);
            if (expense == null)
                return NotFound();

            _context.Expenses.Remove(expense);
            _context.SaveChanges();

            return NoContent();
        }

        // GET: api/MoneyTracker/error
        [HttpGet("error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var errorModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return Problem(detail: $"Request ID: {errorModel.RequestId}");
        }
    }


}
