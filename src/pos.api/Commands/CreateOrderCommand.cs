namespace Pos.Api.Commands
{
	using LinqToDB;
	using Pos.Api.DataLayer;
	using Pos.Api.Models;
	using System;
	using System.Threading;
	using System.Threading.Tasks;

	public class CreateOrderCommand : ICommand<CreateOrderRequest, int>
	{
		private readonly IPosDataConnection dataConnection;

		public CreateOrderCommand(IPosDataConnection dataConnection)
		{
			this.dataConnection = dataConnection ?? throw new ArgumentNullException(nameof(dataConnection));
		}

		public async Task<int> Execute(CreateOrderRequest request, CancellationToken cancellationToken)
		{
			if (request is null)
			{
				throw new ArgumentNullException(nameof(request));
			}

			var order =
				new Order
				{
					Number = request.Order.Number,
					Date = request.Order.Date,
					ProviderID = request.Order.ProviderID,
				};
			var orderID = await dataConnection.InsertWithInt32IdentityAsync(order, token: cancellationToken);
			foreach (var requestItem in request.Order.Items)
			{
				var item =
					new OrderItem
					{
						OrderID = orderID,
						Name = requestItem.Name,
						Quantity = requestItem.Quantity,
						Unit = requestItem.Unit,
					};
				await dataConnection.InsertWithInt32IdentityAsync(item, token: cancellationToken);
			}

			await dataConnection.Commit(cancellationToken);
			return orderID;
		}
	}
}
