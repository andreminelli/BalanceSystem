using System.ComponentModel.DataAnnotations;

namespace BalanceSystem.Core
{
	public class Balance
	{
		public DateTimeOffset StartDate { get; private set; }
		public DateTimeOffset EndDate { get; private set; }
		public decimal Amount { get; private set; }

		public Balance(IEnumerable<Entry> entries)
		{
			Amount = 0;
			StartDate = DateTimeOffset.MaxValue;
			EndDate = DateTimeOffset.MinValue;

			if (entries == null) return;

			foreach (var entry in entries)
			{
				Amount = entry.SumTo(Amount);
				CheckDates(entry);
			}
		}

		private void CheckDates(Entry entry)
		{
			if (entry.Date < StartDate)
			{
				StartDate = entry.Date;
			}

			if (entry.Date > EndDate)
			{
				EndDate = entry.Date;
			}
		}
	}
}
