using BalanceSystem.Core;
using Microsoft.EntityFrameworkCore;

namespace BalanceSystem.DataAccess.PostgreSql
{
	public class PqSqlAccountRepository : IAccountRepository
	{
		private readonly BalanceDbContext _dbContext;

		public PqSqlAccountRepository(BalanceDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Account?> GetByIdAsync(AccountId accountId)
		{
			return await _dbContext.Accounts
				.FirstOrDefaultAsync(account => account.Id == accountId);
		}
	}
}
