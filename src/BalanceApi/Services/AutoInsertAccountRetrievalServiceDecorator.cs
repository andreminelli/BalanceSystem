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
		private readonly IWebHostEnvironment _hostEnvironment;
		private readonly IAccountRepository _accountRepository;

		public AutoInsertAccountRetrievalServiceDecorator(
			IAccountRetrievalService accountRetrievalService,
			IWebHostEnvironment hostEnvironment,
			IAccountRepository accountRepository)
        {
			_accountRetrievalService = accountRetrievalService;
			_hostEnvironment = hostEnvironment;
			_accountRepository = accountRepository;
		}

        public async ValueTask<Account> GetAuthenticatedAsync()
		{
			var account = await _accountRetrievalService.GetAuthenticatedAsync();
			if (_hostEnvironment.IsDevelopment() && account is not null)
			{
				await _accountRepository.AddIfNewAsync(account);
			}
			return account;
		}
	}
}
