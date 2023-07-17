using BalanceSystem.Core;
using System.Diagnostics.CodeAnalysis;

namespace BalanceSystem.DataAccess.PostgreSql
{
	public class PqSqlEntryRepository : IEntryRepository
	{
		private readonly BalanceDbContext _dbContext;

		public PqSqlEntryRepository(BalanceDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public Task AddEntryAsync(Account owner, Entry newEntry)
		{
			throw new NotImplementedException();
		}

		[return: NotNull]
		public Task<IEnumerable<Entry>> GetEntriesAsync(Account account, DateTimeOffset startDate, DateTimeOffset endDate)
		{
			throw new NotImplementedException();
		}
	}
}
