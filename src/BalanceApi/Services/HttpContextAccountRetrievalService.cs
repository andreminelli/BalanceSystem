using BalanceSystem.Core;
using System.Security.Claims;

namespace BalanceSystem.Api.Services
{
	public class HttpContextAccountRetrievalService : IAccountRetrievalService
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public HttpContextAccountRetrievalService(IHttpContextAccessor httpContextAccessor)
        {
			_httpContextAccessor = httpContextAccessor;
		}

        public Account GetAuthenticated()
		{
			var user = _httpContextAccessor.HttpContext?.User ?? throw new InvalidOperationException("Can't find valid user in Httpcontext");
			Account account = new()
			{
				Id = (AccountId)user.FindFirstValue(ClaimTypes.NameIdentifier)
			};
			return account;
		}
	}
}
