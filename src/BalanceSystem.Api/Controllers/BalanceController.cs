﻿using BalanceSystem.Api.Services;
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

		/// <summary>
		/// Retorna o saldo consolidado apenas dos lançamentos no período especificado. 
		/// </summary>
		/// <param name="start">Data de início do periodo (inclusive)</param>
		/// <param name="end">Data de fim do periodo (inclusive)</param>
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[HttpGet("summary")]
		public async Task<IActionResult> GetSummaryAsync(DateTimeOffset? start, DateTimeOffset? end)
		{
			var account = await _accountRetrievalService.GetAuthenticatedAsync();
			var balance = await _balanceService.GetBalanceAsync(account, start, end);
			return Ok(balance);
		}

		/// <summary>
		/// Retorna o saldo diário consolidado no período, a partir do saldo imediatamente anterior ao início.
		/// </summary>
		/// <param name="start">Data de início do periodo (inclusive)</param>
		/// <param name="end">Data de fim do periodo (inclusive)</param>
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[HttpGet("daily")]
		public async Task<IActionResult> GetDailyAsync(DateTimeOffset? start, DateTimeOffset? end)
		{
			var account = await _accountRetrievalService.GetAuthenticatedAsync();
			var balance = await _balanceService.GetDailyBalanceAsync(account, start, end);
			return Ok(balance);
		}
	}
}
