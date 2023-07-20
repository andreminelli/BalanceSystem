namespace BalanceSystem.Core
{
	public class Entry
	{
        public EntryId Id { get; set; }

		public Account Account { get; set; }

		public DateTimeOffset Date { get; init; }
		public EntryType Type { get; init; }
        public string Description { get; init; }

		private decimal _amount;
        public decimal Amount 
		{ 
			get 
			{ 
				return _amount; 
			}

			init
			{
				if (value < 0) throw new ArgumentException($"{nameof(Amount)} should be positive");
				_amount = value;
			}
		}

		public decimal SumTo(decimal value = 0) 
			=> Type switch
			{
				EntryType.Credit => value + Amount,
				EntryType.Debit => value - Amount,
				_ => throw new ArgumentException("Invalid entry type"),
			};
	}
}