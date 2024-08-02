namespace Pos.UI.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.OData.Query;
	using Microsoft.Extensions.DependencyInjection;
	using Pos.UI.Queries;
	using System.Threading;
	using System.Threading.Tasks;

	[ApiController]
	[Route("api/[controller]")]
	public class OrderController : ControllerBase
	{
		[EnableQuery]
		[HttpGet]
		public async Task<IActionResult> Get(CancellationToken cancellationToken)
		{
			var query = HttpContext.RequestServices.GetRequiredService<OrdersQuery>();
			return Ok(await query.Execute(cancellationToken));
		}

		[HttpGet]
		[Route("{orderID}")]
		public async Task<IActionResult> Get(int orderID, CancellationToken cancellationToken)
		{
			var query = HttpContext.RequestServices.GetRequiredService<OrderDetailsQuery>();
			var orderDetails = await query.Execute(orderID, cancellationToken);
			return orderDetails is null ? NotFound() : Ok(orderDetails);
		}
	}
}