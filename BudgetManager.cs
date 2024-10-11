using System.Text.Json;

namespace ExpenseTrackerCLI
{
    public class BudgetManager
    {
        private const string FilePath = "budgets.json";
        private Dictionary<int, decimal> budgets;

        public BudgetManager()
        {
            budgets = LoadBudgets();
        }

        private Dictionary<int, decimal> LoadBudgets()
        {
            if (!File.Exists(FilePath))
                return new Dictionary<int, decimal>();

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<Dictionary<int, decimal>>(json) ?? new Dictionary<int, decimal>();
        }

        private void SaveBudgets()
        {
            var json = JsonSerializer.Serialize(budgets);
            File.WriteAllText(FilePath, json);
        }

        public void SetBudget(int month, decimal amount)
        {
            if (month < 1 || month > 12)
            {
                Console.WriteLine("Invalid month. Please enter a value between 1 and 12.");
                return;
            }

            if (amount <= 0)
            {
                Console.WriteLine("Budget amount must be positive.");
                return;
            }

            budgets[month] = amount;
            SaveBudgets();
            Console.WriteLine($"Budget for month {month} set to ${amount}");
        }

        public decimal GetBudget(int month)
        {
            if (budgets.ContainsKey(month))
            {
                return budgets[month];
            }
            return 0;
        }

        public void ViewBudget(int month)
        {
            if (month < 1 || month > 12)
            {
                Console.WriteLine("Invalid month. Please enter a value between 1 and 12.");
                return;
            }

            if (budgets.TryGetValue(month, out var amount))
            {
                Console.WriteLine($"Budget for month {month}: ${amount}");
            }
            else
            {
                Console.WriteLine($"No budget set for month {month}.");
            }
        }

        public bool IsOverBudget(int month, decimal amount)
        {
            if (budgets.TryGetValue(month, out var budget))
            {
                var totalExpenses = new ExpenseManager().expenses.Where(e => e.Date.Month == month).Sum(e => e.Amount);
                return totalExpenses + amount > budget;
            }
            return false;
        }
    }
}