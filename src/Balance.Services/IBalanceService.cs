using BalanceSystem.Core;

namespace BalanceSystem.Services
{
	public interface IBalanceService
	{
		Task AddEntryAsync(Account account, Entry entry);

		Task<Balance> GetBalanceAsync(Account account, DateTimeOffset startDate, DateTimeOffset endDate);
	}
}
