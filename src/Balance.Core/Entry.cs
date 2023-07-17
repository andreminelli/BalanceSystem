namespace BalanceSystem.Core
{
	public class Entry
	{
		public DateTimeOffset Date { get; init; }
		public EntryType Type { get; init; }
		public decimal Amount { get; init; }
	}
}