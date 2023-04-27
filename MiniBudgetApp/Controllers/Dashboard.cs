using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniBudgetApp.Data;
using MiniBudgetApp.Models;
using System.Globalization;

namespace MiniBudgetApp.Controllers
{
    public class Dashboard : Controller
    {

        private readonly AppDbContext _context;
        public Dashboard(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ActionResult> Index()
        {
            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Today;

            List<Transaction> SelectedTransactions = await _context.Transactions
                .Include(x => x.Category)
                .Where(y=>y.Date >= StartDate && y.Date <=EndDate)
                .ToListAsync();

            //income
            int TotalIncome = SelectedTransactions
                .Where(i => i.Category.Type == "Income")
                .Sum(i => i.Amount);
            ViewBag.TotalIncome = TotalIncome.ToString("C2");


            //expense
            int TotalExpense = SelectedTransactions
                .Where(i => i.Category.Type == "Expense")
                .Sum(i => i.Amount);
            ViewBag.TotalExpense = TotalExpense.ToString("C2");

            //balance
            int Balance = TotalIncome - TotalExpense;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            culture.NumberFormat.CurrencyNegativePattern = 1;
            ViewBag.Balance = String.Format(culture, "{0:C2}", Balance);

            return View();
        }
    }
}
