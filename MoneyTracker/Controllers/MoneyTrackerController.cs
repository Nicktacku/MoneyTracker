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

        // View
        [HttpGet]
        [Route("/MoneyTracker")]
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

        // Get all expenses (AJAX)
        [HttpGet("Display")]
        public IActionResult Expenses()
        {
            var allExpenses = _context.Expenses.ToList();
            return Json(allExpenses);
        }

        [HttpPost("CreateEdit")]
        public IActionResult CreateEditExpense([FromForm] Expense model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingExpense = _context.Expenses.FirstOrDefault(e => e.Id == model.Id);

            if (existingExpense == null)
            {
                // ✅ New record
                _context.Expenses.Add(model);
            }
            else
            {
                // ✅ Update only existing
                existingExpense.Value = model.Value;
                existingExpense.Description = model.Description;

                _context.Expenses.Update(existingExpense);
            }

            _context.SaveChanges();

            return Ok(model); // return JSON instead of redirect
        }

        // Delete (AJAX)
        [HttpPost("Delete/{id}")]
        public IActionResult DeleteExpense(int id)
        {
            var expenseInDb = _context.Expenses.SingleOrDefault(expense => expense.Id == id);
            if (expenseInDb == null)
            {
                return NotFound();
            }

            _context.Expenses.Remove(expenseInDb);
            _context.SaveChanges();

            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
