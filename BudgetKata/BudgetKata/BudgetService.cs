using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace BudgetKata;

public class BudgetService
{
    private readonly IBudgetRepository _budgeRepo;

    public BudgetService(IBudgetRepository budgeRepo)
    {
        _budgeRepo = budgeRepo;
    }

    public decimal Query(DateTime startDate, DateTime endDate)
    {
        var budgets = _budgeRepo.GetAll();

        if (budgets.Any())
        {
            var dailyAmounts = new Dictionary<string, decimal>();

            foreach (var budget in budgets)
            {
                var date = DateTime.ParseExact(budget.YearMonth, "yyyyMM", CultureInfo.InvariantCulture);
                var daysInThisMonth = DateTime.DaysInMonth(date.Year, date.Month);
                var dailyAmount = (decimal)budget.Amount / daysInThisMonth;
                dailyAmounts.Add(budget.YearMonth, dailyAmount);
            }

            var startMonthFirstDate = new DateTime(startDate.Year, startDate.Month, 1);
            var endMonthLastDate = new DateTime(endDate.Year, endDate.Month, 1).AddMonths(1).AddDays(-1);
            var headExcludedDays = (startDate - startMonthFirstDate).TotalDays;
            var tailExcludedDays = (endMonthLastDate - endDate).TotalDays;

            var totalAmount = budgets.Where(b => DateTime.ParseExact(b.YearMonth, "yyyyMM", CultureInfo.InvariantCulture) >= startMonthFirstDate &&
                                         DateTime.ParseExact(b.YearMonth, "yyyyMM", CultureInfo.InvariantCulture) <= endMonthLastDate).Sum(b => b.Amount);

            var headExcludedAmount = (decimal) headExcludedDays * dailyAmounts[startDate.ToString("yyyyMM")];
            var tailExcludedAmount = (decimal) tailExcludedDays * dailyAmounts[endDate.ToString("yyyyMM")];

            return totalAmount - headExcludedAmount - tailExcludedAmount;
        }

        return 0;
    }
}