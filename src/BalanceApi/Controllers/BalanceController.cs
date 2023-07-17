using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BalanceSystem.Api.Controllers
{
	[Authorize]
	[ApiController]
	[Route("balances")]
	public class BalanceController : ControllerBase
	{
		[HttpGet]
		public async Task<IActionResult> GetAsync()
		{
			return Ok();
		}
	}
}
