using System;
using FluentAssertions;
using NUnit.Framework;

namespace BudgetKata;

public class BudgetServiceTests
{
    private BudgetService _budgetService;

    [SetUp]
    public void Setup()
    {
        _budgetService = new BudgetService();
    }

    [Test]
    public void no_budget()
    {
        var totalAmount = WhenQuery(new DateTime(2022, 10, 1), new DateTime(2022, 10, 31));
        totalAmount.Should()!.Be(0m);
    }
    

    private decimal WhenQuery(DateTime startDate, DateTime endDate)
    {
        return _budgetService.Query(startDate, endDate);
    }
}