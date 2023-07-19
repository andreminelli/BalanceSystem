namespace BalanceSystem.Core
{
	public class Entry
	{
        public EntryId Id { get; set; }

		public Account Account { get; set; }

		public DateTimeOffset Date { get; init; }
		public EntryType Type { get; init; }
        public string Description { get; init; }
        public decimal Amount { get; init; }

		public decimal SumTo(decimal value = 0) 
			=> Type switch
			{
				EntryType.Credit => value + Amount,
				EntryType.Debit => value - Amount,
				_ => throw new ArgumentException("Invalid entry type"),
			};
	}
}