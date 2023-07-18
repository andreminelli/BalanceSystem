using BalanceSystem.Core;

namespace BalanceSystem.DataAccess
{
	public interface IAccountRepository
	{
		Task<Account?> GetByIdAsync(AccountId accountId);
	}
}
