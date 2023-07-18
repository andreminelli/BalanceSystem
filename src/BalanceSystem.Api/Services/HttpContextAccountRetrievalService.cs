using BalanceSystem.Core;
using BalanceSystem.DataAccess;
using System.Security.Claims;

namespace BalanceSystem.Api.Services
{
	public class HttpContextAccountRetrievalService : IAccountRetrievalService
	{
		private readonly IWebHostEnvironment _hostEnvironment;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IAccountRepository _accountRepository;

		public HttpContextAccountRetrievalService(
			IWebHostEnvironment hostEnvironment,
			IHttpContextAccessor httpContextAccessor,
			IAccountRepository accountRepository)
        {
			_hostEnvironment = hostEnvironment;
			_httpContextAccessor = httpContextAccessor;
			_accountRepository = accountRepository;
		}

        public ValueTask<Account> GetAuthenticatedAsync()
		{
			var user = _httpContextAccessor.HttpContext?.User ?? throw new InvalidOperationException("Can't find valid user in Httpcontext");
			Account account = new()
			{
				Id = (AccountId)user.FindFirstValue(ClaimTypes.NameIdentifier),
				Name = user.FindFirstValue(ClaimTypes.Name)
			};

			return ValueTask.FromResult(account);
		}
	}
}
