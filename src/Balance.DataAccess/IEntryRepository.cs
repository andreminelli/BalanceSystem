using BalanceSystem.Core;
using System.Diagnostics.CodeAnalysis;

namespace BalanceSystem.DataAccess
{
	public interface IEntryRepository
	{
		Task AddEntryAsync(Account owner, Entry newEntry);

		[return: NotNull]
		Task<IEnumerable<Entry>> GetEntriesAsync(Account account, DateTimeOffset startDate, DateTimeOffset endDate);
	}
}