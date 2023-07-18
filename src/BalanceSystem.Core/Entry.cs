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
		{
			switch (Type)
			{
				case EntryType.Credit:
					return value + Amount;

				case EntryType.Debit:
					return value - Amount;

				default:
					throw new ArgumentException("Invalid entry type");
			}
		}
	}
}