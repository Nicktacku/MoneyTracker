using Microsoft.AspNetCore.Mvc;
using MoneyTracker.Data;
using MoneyTracker.Models;
using System.Diagnostics;

namespace MoneyTracker.Controllers
{
    public class MoneyTrackerController : Controller
    {
        private readonly MoneyTrackerDbContext _context;

        public MoneyTrackerController( MoneyTrackerDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Expenses()
        {
            var allExpenses = _context.Expenses.ToList();
            return View(allExpenses);
        }

        public IActionResult CreateEditExpense(int? id)
        {
            if (id != null)
            {
                var expenseInDb = _context.Expenses.SingleOrDefault(expense => expense.Id == id);


                return View(expenseInDb);
            }


            return View();
        }

        public IActionResult DeleteExpense(int id)
        {
            var expenseIdDb = _context.Expenses.SingleOrDefault(expense => expense.Id == id);
            _context.Expenses.Remove(expenseIdDb);
            _context.SaveChanges();

            return RedirectToAction("Expenses");
        }

        public IActionResult CreateEditExpenseForm(Expense model)
        {
            if (model.Id == 0)
            {
                _context.Expenses.Add(model);

            }
            else
            {
                _context.Expenses.Update(model);
            }

            _context.SaveChanges();



            return RedirectToAction("Expenses");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
