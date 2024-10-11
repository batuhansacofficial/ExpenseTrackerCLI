using ExpenseTrackerCLI;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;

var rootCommand = new RootCommand("Expense Tracker");

var addCommand = new Command("add", "Add a new expense")
   {
       new Option<string>("--description", "Description of the expense"),
       new Option<decimal>("--amount", "Amount of the expense"),
       new Option<string>("--category", "Category of the expense")
   };
addCommand.Handler = CommandHandler.Create<string, decimal, string>((description, amount, category) =>
{
    try
    {
        var manager = new ExpenseManager();
        var budgetManager = new BudgetManager();
        if (budgetManager.IsOverBudget(DateTime.Now.Month, amount))
        {
            Console.WriteLine("Warning: Adding this expense will exceed your budget for the month.");
            return;
        }
        manager.AddExpense(description, amount, category);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
});

var updateCommand = new Command("update", "Update an existing expense")
   {
       new Option<int>("--id", "ID of the expense"),
       new Option<string>("--description", "New description of the expense"),
       new Option<decimal>("--amount", "New amount of the expense"),
       new Option<string>("--category", "New category of the expense")
   };
updateCommand.Handler = CommandHandler.Create<int, string, decimal, string>((id, description, amount, category) =>
{
    try
    {
        var manager = new ExpenseManager();
        manager.UpdateExpense(id, description, amount, category);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
});

var setBudgetCommand = new Command("set-budget", "Set a budget for a month")
{
    new Option<int>("--month", "Month for the budget"),
    new Option<decimal>("--amount", "Budget amount")
};
setBudgetCommand.Handler = CommandHandler.Create<int, decimal>((month, amount) =>
{
    try
    {
        var budgetManager = new BudgetManager();
        budgetManager.SetBudget(month, amount);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
});

var deleteCommand = new Command("delete", "Delete an expense")
   {
       new Option<int>("--id", "ID of the expense")
   };
deleteCommand.Handler = CommandHandler.Create<int>((id) =>
{
    try
    {
        var manager = new ExpenseManager();
        manager.DeleteExpense(id);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
});

var listCommand = new Command("list", "List all expenses")
{
    new Option<string>("--category", "Category to filter expenses")
};
listCommand.Handler = CommandHandler.Create<string>((category) =>
{
    try
    {
        var manager = new ExpenseManager();
        manager.ListExpenses(category);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
});

var summaryCommand = new Command("summary", "Show summary of expenses")
   {
       new Option<int?>("--month", "Month for the summary")
   };
summaryCommand.Handler = CommandHandler.Create<int?>((month) =>
{
    try
    {
        var manager = new ExpenseManager();
        if (month.HasValue)
            manager.Summary(month.Value);
        else
            manager.Summary();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
});

var viewBudgetCommand = new Command("view-budget", "View the budget for a month")
{
    new Option<int>("--month", "Month for the budget")
};
viewBudgetCommand.Handler = CommandHandler.Create<int>((month) =>
{
    try
    {
        var budgetManager = new BudgetManager();
        budgetManager.ViewBudget(month);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
});

var exportCommand = new Command("export", "Export expenses to a CSV file")
{
    new Option<string>("--file", "File path for the CSV export")
};
exportCommand.Handler = CommandHandler.Create<string>((file) =>
{
    try
    {
        var manager = new ExpenseManager();
        manager.ExportToCsv(file);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
});

rootCommand.AddCommand(addCommand);
rootCommand.AddCommand(updateCommand);
rootCommand.AddCommand(deleteCommand);
rootCommand.AddCommand(listCommand);
rootCommand.AddCommand(summaryCommand);
rootCommand.AddCommand(setBudgetCommand);
rootCommand.AddCommand(viewBudgetCommand);
rootCommand.AddCommand(exportCommand);

await rootCommand.InvokeAsync(args);
