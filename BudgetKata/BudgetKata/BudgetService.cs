using System;
using System.Linq;

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
            var totalDays = (decimal)(endDate - startDate).TotalDays + 1;

            return (decimal)budgets.First().Amount / daysInMonth * totalDays;
        }

        return 0;
    }
}