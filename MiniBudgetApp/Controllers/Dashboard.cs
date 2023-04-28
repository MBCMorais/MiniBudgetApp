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

            //Doughnut Chart - Expense By Category
            ViewBag.DoughnutChartData = SelectedTransactions
                .Where(i => i.Category.Type == "Expense")
                .GroupBy(j => j.Category.CategoryId)
                .Select(k => new
                {
                    categoryTitleWithIcon = k.First().Category.Icon + " " + k.First().Category.Name,
                    amount = k.Sum(j => j.Amount),
                    formattedAmount = k.Sum(j => j.Amount).ToString("C0"),
                })
                .OrderByDescending(l => l.amount)
                .ToList();

            // line chart

            //income
            List<SplineChartData> IncomeSummary = SelectedTransactions
                .Where(i => i.Category.Type == "Income")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData()
                {
                    day = k.First().Date.ToString("dd/MM"),
                    income = k.Sum(l => l.Amount)
                })
                .ToList();

            //expense
            List<SplineChartData> ExpenseSummary = SelectedTransactions
                .Where(i => i.Category.Type == "Income")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData()
                {
                    day = k.First().Date.ToString("dd/MM"),
                    expense = k.Sum(l => l.Amount)
                })
                .ToList();

            //combine income & expense
            string[] Last7Days = Enumerable.Range(0, 7)
                .Select(i => StartDate.AddDays(i).ToString("dd/MM"))
                .ToArray();

            ViewBag.SplineChartData = from day in Last7Days
                                      join income in IncomeSummary on day equals income.day into dayIncomejoined
                                      from income in dayIncomejoined.DefaultIfEmpty()
                                      join expense in ExpenseSummary on day equals expense.day into expensejoined
                                      from expense in expensejoined.DefaultIfEmpty()
                                      select new
                                      {
                                          day = day,
                                          income = income == null ? 0 : income.income,
                                          expense = expense == null ? 0 : expense.expense,
                                      };

            //recent transactions
            ViewBag.RecentTransactions = await _context.Transactions
                .Include(i => i.Category)
                .OrderByDescending(k => k.Date)
                //.Select(k => k.Date.ToString("dd/MM"))
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
