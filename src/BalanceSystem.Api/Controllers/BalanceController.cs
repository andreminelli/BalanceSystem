using BalanceSystem.Api.Services;
using BalanceSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BalanceSystem.Api.Controllers
{
	[Authorize]
	[ApiController]
	[Route("balances")]
	public class BalanceController : ControllerBase
	{
		private readonly ILogger<BalanceController> _logger;
		private readonly IAccountRetrievalService _accountRetrievalService;
		private readonly IBalanceService _balanceService;

		public BalanceController(
			ILogger<BalanceController> logger,
			IAccountRetrievalService accountRetrievalService,
			IBalanceService balanceService)
		{
			_logger = logger;
			_accountRetrievalService = accountRetrievalService;
			_balanceService = balanceService;
		}

		[HttpGet("summary")]
		public async Task<IActionResult> GetSummaryAsync(DateTimeOffset? start, DateTimeOffset? end)
		{
			var account = await _accountRetrievalService.GetAuthenticatedAsync();
			var balance = await _balanceService.GetBalanceAsync(account, start, end);
			return Ok(balance);
		}

		[HttpGet("daily")]
		public async Task<IActionResult> GetDailyAsync(DateTimeOffset? start, DateTimeOffset? end)
		{
			var account = await _accountRetrievalService.GetAuthenticatedAsync();
			var balance = await _balanceService.GetDailyBalanceAsync(account, start, end);
			return Ok(balance);
		}
	}
}
