﻿namespace ExpenseTrackerCLI
{
    public class Expense
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; }
    }

}
