namespace BalanceSystem.Core
{
	public class Entry
	{
        public EntryId Id { get; set; }

		public AccountId AccountId { get; set; }
		public Account Account { get; set; }

		public DateTimeOffset Date { get; init; }
		public EntryType Type { get; init; }
        public string Description { get; init; }
        public decimal Amount { get; init; }
	}
}