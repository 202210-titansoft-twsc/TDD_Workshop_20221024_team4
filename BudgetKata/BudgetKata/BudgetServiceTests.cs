using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace BudgetKata;

public class BudgetServiceTests
{
    private BudgetService _budgetService;
    private IBudgetRepository _budgetRepository;

    [SetUp]
    public void Setup()
    {
        _budgetRepository = Substitute.For<IBudgetRepository>();
        _budgetService = new BudgetService(_budgetRepository);
    }

    [Test]
    public void no_budget()
    {
        GivenBudgets();
        var totalAmount = WhenQuery(new DateTime(2022, 10, 1), new DateTime(2022, 10, 31));
        totalAmount.Should()!.Be(0m);
    }

    [Test]
    public void whole_month()
    {
        GivenBudgets(
            new Budget
            {
                YearMonth = "202210",
                Amount = 3100
            });

        var totalAmount = WhenQuery(new DateTime(2022, 10, 1), new DateTime(2022, 10, 31));
        totalAmount.Should()!.Be(3100m);
    }

    [Test]
    public void one_day()
    {
        GivenBudgets(
            new Budget
            {
                YearMonth = "202210",
                Amount = 3100
            });

        var totalAmount = WhenQuery(new DateTime(2022, 10, 29), new DateTime(2022, 10, 29));
        totalAmount.Should()!.Be(100m);
    }

    [Test]
    public void multi_days()
    {
        GivenBudgets(
            new Budget
            {
                YearMonth = "202210",
                Amount = 3100
            });

        var totalAmount = WhenQuery(new DateTime(2022, 10, 27), new DateTime(2022, 10, 29));
        totalAmount.Should()!.Be(300m);
    }

    [Test]
    public void cross_month()
    {
        GivenBudgets(
            new Budget
            {
                YearMonth = "202210",
                Amount = 3100
            }, new Budget
            {
                YearMonth = "202211",
                Amount = 300
            });

        var totalAmount = WhenQuery(new DateTime(2022, 10, 1), new DateTime(2022, 11, 5));
        totalAmount.Should()!.Be(3150m);
    }

    [Test]
    public void cross_month_with_lost_budget()
    {
        GivenBudgets(
            new Budget
            {
                YearMonth = "202210",
                Amount = 3100
            }, new Budget
            {
                YearMonth = "202212",
                Amount = 310
            });

        var totalAmount = WhenQuery(new DateTime(2022, 10, 1), new DateTime(2022, 12, 5));
        totalAmount.Should()!.Be(3150m);
    }

    [Test]
    public void cross_year()
    {
        GivenBudgets(
            new Budget
            {
                YearMonth = "202110",
                Amount = 3100
            }, new Budget
            {
                YearMonth = "202112",
                Amount = 31000
            }, new Budget
            {
                YearMonth = "202212",
                Amount = 310
            });

        var totalAmount = WhenQuery(new DateTime(2021, 10, 1), new DateTime(2022, 12, 5));
        totalAmount.Should()!.Be(3100m + 31000m + 50m);
    }

    [Test]
    public void no_budget_in_start_month_and_tail_month()
    {
        GivenBudgets(
            new Budget
            {
                YearMonth = "202211",
                Amount = 31000
            });

        var totalAmount = WhenQuery(new DateTime(2022, 10, 1), new DateTime(2022, 12, 5));
        totalAmount.Should()!.Be(31000m);
    }

    [Test]
    public void invalid_end_date()
    {
        GivenBudgets(new Budget
        {
            YearMonth = "202210",
            Amount = 31000
        });

        var totalAmount = WhenQuery(new DateTime(2022, 10, 1), new DateTime(2021, 12, 5));
        totalAmount.Should()!.Be(0m);
    }


    private void GivenBudgets(params Budget[] budgets)
    {
        _budgetRepository.GetAll().Returns(budgets.ToList());
    }

    private decimal WhenQuery(DateTime startDate, DateTime endDate)
    {
        return _budgetService.Query(startDate, endDate);
    }
}