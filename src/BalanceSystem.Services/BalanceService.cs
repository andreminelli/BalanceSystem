using BalanceSystem.Core;
using BalanceSystem.DataAccess;

namespace BalanceSystem.Services
{
	public class BalanceService : IBalanceService
	{
		private readonly IAccountRepository _accountRepository;
		private readonly IEntryRepository _entryRepository;

		public BalanceService(
			IAccountRepository accountRepository,
			IEntryRepository entryRepository)
        {
			_accountRepository = accountRepository;
			_entryRepository = entryRepository;
		}
        public async Task AddEntryAsync(Account account, Entry entry)
		{
			var existingAccount = await _accountRepository.GetByIdAsync(account.Id);
			if (existingAccount == null)
			{
				throw new InvalidOperationException($"Account with Id {account.Id} was not found");
			}

			entry.Account = existingAccount;
			await _entryRepository.AddEntryAsync(entry);
		}

		public async Task<Balance> GetBalanceAsync(Account account, DateTimeOffset startDate, DateTimeOffset endDate)
		{
			// TODO Add validations
			var entries = await _entryRepository.GetEntriesAsync(account, startDate, endDate);

			var balance = new Balance(entries);

			return balance;
		}
	}
}
