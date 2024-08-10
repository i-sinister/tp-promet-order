namespace Pos.UI.Controllers
{
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.OData.Query;
	using Microsoft.Extensions.DependencyInjection;
	using Pos.UI.Commands;
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
		/// <param name="distinct">return distinct result</param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("", Name = "orders_getAll")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrdersListResponse))]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Get(
			ODataQueryOptions<OrderInfo> odataOptions,
			[FromQuery(Name = "$distinct")] bool? distinct,
			CancellationToken cancellationToken)
		{
			var query = HttpContext.RequestServices.GetRequiredService<OrdersQuery>();
			var items = await query.Execute(cancellationToken);
			var result = await items.Apply(odataOptions, distinct, cancellationToken);
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

		/// <summary>
		/// Adds new order
		/// </summary>
		/// <param name="order"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("", Name = "order_create")]
		[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[Transaction]
		public async Task<IActionResult> Create([FromBody]Models.CreateOrderRequest order, CancellationToken cancellationToken)
		{
			if (!await this.Validate(order, default, default, cancellationToken))
			{
				return ValidationProblem();
			}

			var query = HttpContext.RequestServices.GetRequiredService<CreateOrderCommand>();
			var orderID = await query.Execute(new Commands.CreateOrderRequest(order), cancellationToken);
			return Created((string?)null, orderID);
		}

		/// <summary>
		/// Update existing order
		/// </summary>
		/// <param name="orderID">existing order ID</param>
		/// <param name="order"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("{orderID}", Name = "order_update")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[Transaction]
		public async Task<IActionResult> Update(int orderID, [FromBody]Models.UpdateOrderRequest order, CancellationToken cancellationToken)
		{
			if (!await this.Validate(order, nameof(orderID), orderID, cancellationToken))
			{
				return ValidationProblem();
			}

			var query = HttpContext.RequestServices.GetRequiredService<UpdateOrderCommand>();
			var updated = await query.Execute(new Commands.UpdateOrderRequest(orderID, order), cancellationToken);
			return updated ? Ok() : NotFound();
		}

		/// <summary>
		/// Delete existing order
		/// </summary>
		/// <param name="orderID">existing order ID</param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpDelete]
		[Route("{orderID}", Name = "order_delete")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[Transaction]
		public async Task<IActionResult> Delete(int orderID, CancellationToken cancellationToken)
		{
			var query = HttpContext.RequestServices.GetRequiredService<DeleteOrderCommand>();
			var deleted = await query.Execute(new DeleteOrderRequest(orderID), cancellationToken);
			return deleted ? NoContent() : NotFound();
		}
	}
}
