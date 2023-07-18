using BalanceSystem.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
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

		public async Task AddEntryAsync(Entry newEntry)
		{
			_dbContext.Entries.Add(newEntry);
			await _dbContext.SaveChangesAsync();
		}

		[return: NotNull]
		public async Task<IEnumerable<Entry>> GetEntriesAsync(Account account, DateTimeOffset startDate, DateTimeOffset endDate)
		{
			var entries = await _dbContext.Entries
				.Where(entry => 
					entry.Account.Id == account.Id &&
					entry.Date >= startDate &&
					entry.Date <= endDate)
				.ToListAsync();

			return entries;
		}
	}
}
