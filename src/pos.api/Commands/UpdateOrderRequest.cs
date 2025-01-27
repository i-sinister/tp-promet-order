namespace Pos.Api.Commands
{
	using Pos.Api.Models;
	using System;

	public class UpdateOrderRequest
	{
		public UpdateOrderRequest(int orderID, Models.UpdateOrderRequest order)
		{
			OrderID = orderID;
			Order = order ?? throw new ArgumentNullException(nameof(order));
		}

		public int OrderID { get; }

		public Models.UpdateOrderRequest Order { get; }
	}
}
