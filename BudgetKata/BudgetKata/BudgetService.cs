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

        if (startDate == endDate)
        {
            return (decimal)budgets.First().Amount / daysInMonth;
        }
        
        if (budgets.Any())
        {
            return budgets.First().Amount;
        }
        return 0;
    }
}