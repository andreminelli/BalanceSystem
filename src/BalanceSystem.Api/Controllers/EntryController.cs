using BalanceSystem.Api.Services;
using BalanceSystem.Core;
using BalanceSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BalanceSystem.Api.Controllers
{
	[Authorize]
	[ApiController]
	[Route("entries")]
	public class EntryController : ControllerBase
	{
		private readonly ILogger<EntryController> _logger;
		private readonly IAccountRetrievalService _accountRetrievalService;
		private readonly IBalanceService _balanceService;

		public EntryController(
			ILogger<EntryController> logger,
			IAccountRetrievalService accountRetrievalService,
			IBalanceService balanceService)
		{
			_logger = logger;
			_accountRetrievalService = accountRetrievalService;
			_balanceService = balanceService;
		}

		[HttpPost]
		public async Task<IActionResult> AddAsync([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] Entry newEntry)
		{
			var account = await _accountRetrievalService.GetAuthenticatedAsync();
			await _balanceService.AddEntryAsync(account, newEntry);
			return CreatedAtAction(null, null);
		}
	}
}