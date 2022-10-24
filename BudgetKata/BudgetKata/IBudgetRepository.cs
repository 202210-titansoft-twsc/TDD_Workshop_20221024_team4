using System.Collections.Generic;

namespace BudgetKata;

public interface IBudgetRepository
{
    List<Budget> GetAll();
}