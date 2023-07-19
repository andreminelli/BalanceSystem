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
			var existingAccount = await _accountRepository.GetByIdAsync(account.Id)
				?? throw new InvalidOperationException($"Account with Id {account.Id} was not found");
			entry.Account = existingAccount;
			await _entryRepository.AddEntryAsync(entry);
		}

		public async Task<Balance> GetBalanceAsync(Account account, DateTimeOffset? startDate = null, DateTimeOffset? endDate = null)
		{
			var entries = await _entryRepository.GetEntriesAsync(
				account,
				startDate ?? DateTimeOffset.MinValue,
				endDate ?? DateTimeOffset.MaxValue);

			var balance = new Balance(entries);

			return balance;
		}

		public async Task<IEnumerable<DailyBalance>> GetDailyBalanceAsync(Account account, DateTimeOffset? startDate = null, DateTimeOffset? endDate = null)
		{
			var start = startDate ?? DateTimeOffset.MinValue;
			var end = endDate ?? DateTimeOffset.MaxValue;

			var beforeStartDate = start == DateTimeOffset.MinValue ? start : start.AddSeconds(-1);
			var beforeStartDateBalance = await GetBalanceAsync(account, endDate: beforeStartDate);

			var entries = await _entryRepository.GetEntriesAsync(account, start, end);

			var result = new List<DailyBalance>();

			var currentDate = beforeStartDate.Date;
			var currentDateAmount = beforeStartDateBalance.Amount;
			foreach (var entry in entries.OrderBy(entry => entry.Date))
			{
				if (entry.Date.Date != currentDate)
				{
					if (currentDate != DateTimeOffset.MinValue.Date)
					{
						result.Add(new DailyBalance
						{
							Date = currentDate,
							Amount = currentDateAmount,
						});
					}
					currentDate = entry.Date.Date;
				}
				currentDateAmount = entry.SumTo(currentDateAmount);
			}

			result.Add(new DailyBalance
			{
				Date = currentDate,
				Amount = currentDateAmount,
			});

			return result;
		}
	}
}
