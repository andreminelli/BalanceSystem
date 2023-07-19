using BalanceSystem.Api.Models;
using BalanceSystem.Api.Services;
using BalanceSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

		/// <summary>
		/// Inclui um novo lançamento para o usuário autenticado
		/// </summary>
		/// <param name="newEntry"></param>
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[HttpPost]
		public async Task<IActionResult> AddAsync(NewEntry newEntry)
		{
			var account = await _accountRetrievalService.GetAuthenticatedAsync();
			await _balanceService.AddEntryAsync(account, newEntry.ToEntry());
			return CreatedAtAction(null, null);
		}
	}
}