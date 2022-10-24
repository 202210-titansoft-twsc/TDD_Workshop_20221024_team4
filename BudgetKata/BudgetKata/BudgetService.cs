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
        var daysInMonth = DateTime.DaysInMonth(startDate.Year, startDate.Month);

        
        if (budgets.Any())
        {
            var result = 0m;
            var dailyAmounts = new Dictionary<DateTime, decimal>();

            foreach (var budget in budgets)
            {
                var date = DateTime.ParseExact(budget.YearMonth, "yyyyMM", CultureInfo.InvariantCulture);
                var daysInThisMonth = DateTime.DaysInMonth(date.Year, date.Month);
                var dailyAmount = (decimal)budget.Amount / daysInThisMonth;
                dailyAmounts.Add(date, dailyAmount);
            }

            var currentDate = startDate;
            while (currentDate <= endDate)
            {

                result+= dailyAmounts[new DateTime(currentDate.Year, currentDate.Month, 1)];
                currentDate = currentDate.AddDays(1);
            }

            return result;
            var totalDays = (decimal)(endDate - startDate).TotalDays + 1;

            return (decimal)budgets.First().Amount / daysInMonth * totalDays;
        }

        return 0;
    }
}