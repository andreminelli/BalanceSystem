using BalanceSystem.Core;
using BalanceSystem.DataAccess;

namespace BalanceSystem.Api.Services
{
	/// <summary>
	/// Automatically add an Account to database
	/// </summary>
	/// <remarks>
	/// This decorator is intented to be used in Development mode only, 
	/// in order to speed up local tests
	/// </remarks>
	public class AutoInsertAccountRetrievalServiceDecorator : IAccountRetrievalService
	{
		private readonly IAccountRetrievalService _accountRetrievalService;
		private readonly IAccountRepository _accountRepository;

		public AutoInsertAccountRetrievalServiceDecorator(
			IAccountRetrievalService accountRetrievalService,
			IAccountRepository accountRepository)
        {
			_accountRetrievalService = accountRetrievalService;
			_accountRepository = accountRepository;
		}

        public async ValueTask<Account> GetAuthenticatedAsync()
		{
			var account = await _accountRetrievalService.GetAuthenticatedAsync();
			if (account is not null)
			{
				await _accountRepository.AddIfNewAsync(account);
			}
			return account;
		}
	}
}
