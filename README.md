# Expense Tracker CLI

## Description

Expense Tracker CLI is a command-line application for managing personal expenses and budgets. It allows users to add, update, delete, list expenses, and set/view monthly budgets.<br>
Project Idea: https://roadmap.sh/projects/expense-tracker

## Features

- Add new expenses with description, amount, and category.
- Update existing expenses.
- Delete expenses by ID.
- List all expenses or filter by category.
- Set and view monthly budgets.
- Export expenses to a CSV file.
- Check if adding an expense exceeds the monthly budget.

## Getting Started

### Clone the repository:
```bash
git clone https://github.com/batuhansacofficial/ExpenseTrackerCLI.git
```
or download the ZIP file and extract it.

### Navigate to Project Directory
```bash
cd path/to/ExpenseTrackerCLI
```

### Restore Dependencies
```bash
dotnet restore
```

### Build the Application
```bash
dotnet build
```

## Usage

### Add a New Expense
```bash
dotnet run -- add --description "Lunch" --amount 15.50 --category "Food"
```
### Update an Existing Expense
```bash
dotnet run -- update --id 1 --description "Dinner" --amount 20.00 --category "Food"
```
### Delete an Expense
```bash
dotnet run -- delete --id 1
```
### List All Expenses
```bash
dotnet run -- list
```
### List Expenses by Category
```bash
dotnet run -- list --category "Food"
```
### Set a Monthly Budget
```bash
dotnet run -- set-budget --month 5 --amount 1000
```
### View a Monthly Budget
```bash
dotnet run -- view-budget --month 5
```
### Export Expenses to CSV
```bash
dotnet run -- export --file "expenses.csv"
```
### Show Summary of Expenses
```bash
dotnet run -- summary
```
### Show Summary of Expenses for a Specific Month
```bash
dotnet run -- summary --month 5
```

## Project Structure

- `Program.cs`: Entry point of the application, handles command-line arguments.
- `ExpenseManager.cs`: Manages expenses (add, update, delete, list).
- `BudgetManager.cs`: Manages monthly budgets (set, view, check if over budget).

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any improvements or bug fixes.

## License

This project is licensed under the MIT License. See the [LICENSE](https://github.com/batuhansacofficial/ExpenseTrackerCLI/tree/master?tab=MIT-1-ov-file) file for details.

