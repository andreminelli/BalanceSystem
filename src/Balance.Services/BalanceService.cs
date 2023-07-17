using BalanceSystem.Core;
using BalanceSystem.DataAccess;

namespace BalanceSystem.Services
{
	public class BalanceService : IBalanceService
	{
		private readonly IEntryRepository _entryRepository;

		public BalanceService(IEntryRepository entryRepository)
        {
			_entryRepository = entryRepository;
		}
        public async Task AddEntryAsync(Account account, Entry entry)
		{
			// TODO Add validations
			await _entryRepository.AddEntryAsync(account, entry);
		}

		public async Task<Balance> GetBalanceAsync(Account account, DateTimeOffset startDate, DateTimeOffset endDate)
		{
			// TODO Add validations
			var entries = await _entryRepository.GetEntriesAsync(account, startDate, endDate);

			var balance = new Balance();
			balance.ProcessEntries(entries);

			return balance;
		}
	}
}
