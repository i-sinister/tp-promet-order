namespace Pos.Api.Queries
{
	using LinqToDB;
	using Pos.Api.DataLayer;
	using Pos.Api.Models;
	using System;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;

	internal class OrderDetailsQuery : IQuery<int, OrderDetails?>
	{
		private readonly IPosDataConnection dataConnection;

		public OrderDetailsQuery(IPosDataConnection dataConnection)
		{
			this.dataConnection = dataConnection ?? throw new ArgumentNullException(nameof(dataConnection));
		}

		public async Task<OrderDetails?> Execute(int request, CancellationToken cancellationToken)
		{
			await dataConnection.Open(cancellationToken);
			var order = await dataConnection
				.GetOrders(cancellationToken)
				.Where(order => order.ID == request)
				.FirstOrDefaultAsync(cancellationToken);
			if (order is null)
			{
				return null;
			}

			var items = await dataConnection
				.GetOrderItems(cancellationToken)
				.Where(item => item.OrderID == request)
				.ToArrayAsync(cancellationToken);
			var provider = await dataConnection
				.GetProviders(cancellationToken)
				// data consistency is guaranteed by the database
				.SingleAsync(p => p.ID == order.ProviderID);
			var orderDetails =
				new OrderDetails
				{
					ID = order.ID,
					Number = order.Number,
					Date = order.Date,
					ProviderID = provider.ID,
					ProviderName = provider.Name,
					Items = items
				};
			return orderDetails;
		}
	}
}
