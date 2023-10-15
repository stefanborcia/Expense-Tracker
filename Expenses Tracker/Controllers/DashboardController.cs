using Expenses_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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
            ViewBag.totalIncome = totalIncome.ToString("C", CultureInfo.GetCultureInfo("fr-FR")); 
            
            //Total expense
            int totalExpense = selectedTransactions
                .Where(t => t.Category.Type == "Expense")
                .Sum(a => a.Amount);
            ViewBag.totalExpense = totalExpense.ToString("C", CultureInfo.GetCultureInfo("fr-FR"));

            //Balance
            int balance = totalIncome - totalExpense;
            ViewBag.balance = balance.ToString("C", CultureInfo.GetCultureInfo("fr-FR"));

            //Doughnut Chart - Filter Expense By Category
            ViewBag.DoughnutChartData = selectedTransactions
                .Where(e => e.Category.Type == "Expense")
                .GroupBy(c =>c.Category.CategoryId)
                .Select( k => new
                {
                    categoryTitleWithIcon = k.First().Category.Icon + " " + k.First().Category.Title,
                    amount =k.Sum(s => s.Amount),
                    formattedAmount = k.Sum(s => s.Amount).ToString("C", CultureInfo.GetCultureInfo("fr-FR"))
                })
                .OrderByDescending(o=>o.amount)
                .ToList();


            //Spline Chart - Income vs Expense
            //Income
            List<SplineChartData> IncomeSummary = selectedTransactions
                .Where(i => i.Category.Type == "Income")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData()
                {
                    day = k.First().Date.ToString("dd-MMM"),
                    income = k.Sum(l => l.Amount)
                })
                .ToList();

            //Expense
            List<SplineChartData> ExpenseSummary = selectedTransactions
                .Where(i => i.Category.Type == "Expense")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData()
                {
                    day = k.First().Date.ToString("dd-MMM"),
                    expense = k.Sum(l => l.Amount)
                })
                .ToList();

            //Combine Income and Expense
            string[] lastSevenDays = Enumerable.Range(0, 7)
                .Select(i => startDate.AddDays(i).ToString("dd-MMM"))
                .ToArray();

            ViewBag.SplineChartData = from day in lastSevenDays
                join income in IncomeSummary on day equals income.day into dayIncomeJoined
                from income in dayIncomeJoined.DefaultIfEmpty()
                join expense in ExpenseSummary on day equals expense.day into expenseJoined
                from expense in expenseJoined.DefaultIfEmpty()
                select new
                {
                    day = day,
                    income = income == null ? 0 : income.income,
                    expense = expense == null ? 0 : expense.expense
                };

            //Recent Transactions
            ViewBag.RecentTransactions = await _context.Transactions
                .Include(i => i.Category)
                .OrderByDescending(j => j.Date)
                .Take(5)
                .ToListAsync();

            return View();
        }

        public class SplineChartData
        {
            public string day;
            public int income;
            public int expense;
        }
    }
}
