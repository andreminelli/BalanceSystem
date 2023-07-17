namespace BalanceSystem.Core
{
	public class Entry
	{
		public Account Account { get; init; }
		public DateTimeOffset Date { get; init; }
		public EntryType Type { get; init; }
        public string Description { get; set; }
        public decimal Amount { get; init; }
	}
}