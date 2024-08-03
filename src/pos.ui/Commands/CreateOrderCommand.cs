namespace Pos.UI.Commands
{
	using LinqToDB;
	using Pos.UI.DataLayer;
	using Pos.UI.Models;
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

			await using var transaction = await dataConnection.BeginTransaction(cancellationToken);
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

			await transaction.CommitAsync(cancellationToken);
			return orderID;
		}
	}
}