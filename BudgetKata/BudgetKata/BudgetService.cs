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
            var result = 0m;
            var dailyAmounts = new Dictionary<string, decimal>();

            foreach (var budget in budgets)
            {
                var date = DateTime.ParseExact(budget.YearMonth, "yyyyMM", CultureInfo.InvariantCulture);
                var daysInThisMonth = DateTime.DaysInMonth(date.Year, date.Month);
                var dailyAmount = (decimal)budget.Amount / daysInThisMonth;
                dailyAmounts.Add(budget.YearMonth, dailyAmount);
            }

            var currentDate = startDate;

            while (currentDate <= endDate)
            {
                var key = currentDate.ToString("yyyyMM");

                if (!dailyAmounts.ContainsKey(key))
                {
                    currentDate = currentDate.AddDays(1);
                    continue;
                }

                result += dailyAmounts[key];
                currentDate = currentDate.AddDays(1);
            }

            return result;
        }

        return 0;
    }
}