using System.Text;
using System.Text.Json;

namespace ExpenseTrackerCLI
{
    public class ExpenseManager
    {
        private const string FilePath = "expenses.json";
        public List<Expense> expenses;

        public ExpenseManager()
        {
            expenses = LoadExpenses();
        }

        private List<Expense> LoadExpenses()
        {
            if (!File.Exists(FilePath))
                return new List<Expense>();

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Expense>>(json) ?? new List<Expense>();
        }

        private void SaveExpenses()
        {
            var json = JsonSerializer.Serialize(expenses);
            File.WriteAllText(FilePath, json);
        }

        public void AddExpense(string description, decimal amount, string category)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                Console.WriteLine("Description cannot be empty.");
                return;
            }

            if (amount <= 0)
            {
                Console.WriteLine("Amount must be positive.");
                return;
            }

            if (string.IsNullOrWhiteSpace(category))
            {
                Console.WriteLine("Category cannot be empty.");
                return;
            }

            var expense = new Expense
            {
                Id = expenses.Any() ? expenses.Max(e => e.Id) + 1 : 1,
                Description = description,
                Amount = amount,
                Category = category
            };
            expenses.Add(expense);
            SaveExpenses();
            Console.WriteLine($"Expense added successfully (ID: {expense.Id})");
        }

        public void UpdateExpense(int id, string description, decimal amount, string category)
        {
            var expense = expenses.FirstOrDefault(e => e.Id == id);
            if (expense == null)
            {
                Console.WriteLine("Expense not found.");
                return;
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                Console.WriteLine("Description cannot be empty.");
                return;
            }

            if (amount <= 0)
            {
                Console.WriteLine("Amount must be positive.");
                return;
            }

            if (string.IsNullOrWhiteSpace(category))
            {
                Console.WriteLine("Category cannot be empty.");
                return;
            }

            expense.Description = description;
            expense.Amount = amount;
            expense.Category = category;
            SaveExpenses();
            Console.WriteLine("Expense updated successfully.");
        }

        public void DeleteExpense(int id)
        {
            if (id <= 0)
            {
                Console.WriteLine("Invalid expense ID.");
                return;
            }

            var expense = expenses.FirstOrDefault(e => e.Id == id);
            if (expense == null)
            {
                Console.WriteLine("Expense not found.");
                return;
            }

            expenses.Remove(expense);
            SaveExpenses();
            Console.WriteLine("Expense deleted successfully.");
        }

        public void ListExpenses(string category = null)
        {
            if (!string.IsNullOrEmpty(category) && !expenses.Any(e => e.Category == category))
            {
                Console.WriteLine("Category not found.");
                return;
            }

            var filteredExpenses = string.IsNullOrEmpty(category) ? expenses : expenses.Where(e => e.Category == category).ToList();

            if (!filteredExpenses.Any())
            {
                Console.WriteLine("No expenses found.");
                return;
            }

            Console.WriteLine("ID  Date       Description  Amount  Category");
            foreach (var expense in filteredExpenses)
            {
                Console.WriteLine($"{expense.Id}   {expense.Date:yyyy-MM-dd}  {expense.Description}  ${expense.Amount}  {expense.Category}");
            }
        }

        public void Summary()
        {
            if (!expenses.Any())
            {
                Console.WriteLine("No expenses found.");
                return;
            }

            var total = expenses.Sum(e => e.Amount);
            Console.WriteLine($"Total expenses: ${total}");
        }

        public void Summary(int month)
        {
            var monthlyExpenses = expenses.Where(e => e.Date.Month == month).ToList();
            if (!monthlyExpenses.Any())
            {
                Console.WriteLine($"No expenses found for month {month}.");
                return;
            }

            var total = monthlyExpenses.Sum(e => e.Amount);
            Console.WriteLine($"Total expenses for {month}: ${total}");
        }

        public List<Expense> GetExpensesForMonth(int month)
        {
            return expenses.Where(e => e.Date.Month == month).ToList();
        }

        public void ExportToCsv(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                Console.WriteLine("File path cannot be empty.");
                return;
            }

            try
            {
                var csv = new StringBuilder();
                csv.AppendLine("ID,Date,Description,Amount,Category");

                foreach (var expense in expenses)
                {
                    csv.AppendLine($"{expense.Id},{expense.Date:yyyy-MM-dd},{expense.Description},{expense.Amount},{expense.Category}");
                }

                File.WriteAllText(filePath, csv.ToString());
                Console.WriteLine($"Expenses exported to {filePath}");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Error: Access to the path is denied.");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Error: The specified path is invalid.");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error: An I/O error occurred while exporting to CSV. {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: An unexpected error occurred. {ex.Message}");
            }
        }
    }
}
