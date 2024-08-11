namespace Pos.Api.Commands
{
	using System;

	public class CreateOrderRequest
	{
		public CreateOrderRequest(Models.CreateOrderRequest order)
		{
			Order = order ?? throw new ArgumentNullException(nameof(order));
		}

		public Models.CreateOrderRequest Order { get; }
	}
}
