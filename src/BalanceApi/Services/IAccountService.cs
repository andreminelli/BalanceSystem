using BalanceSystem.Core;

namespace BalanceSystem.Api.Services
{
	public interface IAccountRetrievalService
	{
		ValueTask<Account> GetAuthenticatedAsync();
	}
}
