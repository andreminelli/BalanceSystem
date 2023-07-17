namespace BalanceSystem.Core
{
	public class Balance
	{
		public DateTimeOffset StartDate { get; private set; }
		public DateTimeOffset EndDate { get; private set; }
		public decimal Amount { get; private set; }

		public Balance()
		{
			Amount = 0;
			StartDate = DateTimeOffset.MaxValue;
			EndDate = DateTimeOffset.MinValue;
		}

		public void ProcessEntries(IEnumerable<Entry> entries)
		{
			foreach (var entry in entries)
			{
				ProcessEntry(entry);
			}
		}

		public void ProcessEntry(Entry entry)
		{
			switch (entry.Type)
			{
				case EntryType.Credit:
					ProcessCredit(entry);
					break;

				case EntryType.Debit:
					ProcessDebit(entry);
					break;

				default:
					throw new ArgumentException("Invalid entry type");
			}
		}

		private void ProcessCredit(Entry entry)
		{
			Amount += entry.Amount;
			CheckDates(entry);
		}

		private void ProcessDebit(Entry entry)
		{
			Amount -= entry.Amount;
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
