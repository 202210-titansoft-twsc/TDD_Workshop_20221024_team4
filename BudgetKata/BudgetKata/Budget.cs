using System;
using System.Globalization;

namespace BudgetKata;

public class Budget
{
    /// <summary>
    /// YYYYMM
    /// </summary>
    public string YearMonth { get; set; }
    public int Amount { get; set; }

    public DateTime FirstDateInMonth => DateTime.ParseExact(YearMonth, "yyyyMM", CultureInfo.InvariantCulture);
}