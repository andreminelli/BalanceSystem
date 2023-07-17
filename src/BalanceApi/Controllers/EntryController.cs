using BalanceSystem.Core;
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

		public EntryController(ILogger<EntryController> logger)
		{
			_logger = logger;
		}

		[HttpPost]
		public async Task<IActionResult> AddAsync([FromBody] Entry newEntry)
		{
			return Ok();
		}
	}
}