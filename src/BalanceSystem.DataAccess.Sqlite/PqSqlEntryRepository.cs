using BalanceSystem.Core;
using Microsoft.EntityFrameworkCore;
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
		public async Task<IEnumerable<Entry>> GetEntriesAsync(Account account, DateTimeOffset startDate, DateTimeOffset endDate)
		{
			var entries = await _dbContext.Entries
				.Where(entry => 
					entry.Account == account &&
					entry.Date >= startDate &&
					entry.Date <= endDate)
				.ToListAsync();

			return entries;
		}
	}
}
