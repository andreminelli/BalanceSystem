using BalanceSystem.Core;

namespace BalanceSystem.Api.Services
{
	public interface IAccountRetrievalService
	{
		Account GetAuthenticated();
	}
}
