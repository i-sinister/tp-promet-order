namespace Pos.UI.Controllers
{
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.OData.Query;
	using Microsoft.Extensions.DependencyInjection;
	using Pos.UI.Models;
	using Pos.UI.Queries;
	using System.Threading;
	using System.Threading.Tasks;

	/// <summary>
	/// API to manage orders
	/// </summary>
	[ApiController]
	[Route("api/orders")]
	public class OrderController : ControllerBase
	{
		/// <summary>
		/// Returns all orders
		/// </summary>
		/// <param name="odataOptions"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("", Name = "orders_getAll")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrdersListResponse))]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Get(ODataQueryOptions<OrderInfo> odataOptions, CancellationToken cancellationToken)
		{
			var query = HttpContext.RequestServices.GetRequiredService<OrdersQuery>();
			var items = await query.Execute(cancellationToken);
			var result = odataOptions.ApplyTo(items).ToListResult();
			return Ok(result);
		}

		/// <summary>
		/// Returns order details by order id
		/// </summary>
		/// <param name="orderID">Unique order identifier</param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("{orderID}", Name = "order_getByID")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDetails))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Get(int orderID, CancellationToken cancellationToken)
		{
			var query = HttpContext.RequestServices.GetRequiredService<OrderDetailsQuery>();
			var orderDetails = await query.Execute(orderID, cancellationToken);
			return orderDetails is null ? NotFound() : Ok(orderDetails);
		}
	}
}