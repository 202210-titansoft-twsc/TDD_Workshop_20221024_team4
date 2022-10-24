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

        if (budgets.Any())
        {
            return budgets.Sum(b => b.Amount);
        }
        return 0;
    }
}