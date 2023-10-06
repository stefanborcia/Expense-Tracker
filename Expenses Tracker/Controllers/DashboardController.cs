using Expenses_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Expenses_Tracker.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            //Last 7Days
            DateTime startDate = DateTime.Today.AddDays(-6);
            DateTime endDate = DateTime.Today;

            List<Transaction> selectedTransactions = await _context.Transactions
                .Include(c => c.Category)
                .Where(d=>d.Date >= startDate && d.Date <= endDate)
                .ToListAsync();


            //Total income
            int totalIncome = selectedTransactions
                .Where(t => t.Category.Type == "Income")
                .Sum(a => a.Amount);
            ViewBag.totalIncome = totalIncome.ToString("C0"); 
            
            //Total expense
            int totalExpense = selectedTransactions
                .Where(t => t.Category.Type == "Expense")
                .Sum(a => a.Amount);
            ViewBag.totalExpense = totalExpense.ToString("C0");

            //Balance
            int balance = totalIncome - totalExpense;
            ViewBag.balance = balance.ToString("C0");

            return View();
        }
    }
}
