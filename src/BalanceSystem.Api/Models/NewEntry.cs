using BalanceSystem.Core;

namespace BalanceSystem.Api.Models
{
	public class NewEntry
	{
		public DateTimeOffset Date { get; init; }
		public EntryType Type { get; init; }
		public string Description { get; init; }
		public decimal Amount { get; init; }

		public Entry ToEntry()
		{
			return new Entry
			{
				Date = Date,
				Type = Type,
				Description = Description,
				Amount = Amount
			};
		}
	}
}
