namespace Pos.Api.Queries
{
	using Pos.Api.DataLayer;
	using Pos.Api.Models;
	using System;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;

	public class OrdersQuery : IQuery<IQueryable<OrderInfo>>
	{
		private readonly IPosDataConnection dataConnection;

		public OrdersQuery(IPosDataConnection dataConnection)
		{
			this.dataConnection = dataConnection ?? throw new ArgumentNullException(nameof(dataConnection));
		}

		public async Task<IQueryable<OrderInfo>> Execute(CancellationToken cancellationToken)
		{
			await dataConnection.Open(cancellationToken);
			var query =
				from order in dataConnection.GetOrders(cancellationToken)
				join provider in dataConnection.GetProviders(cancellationToken)
					on order.ProviderID equals provider.ID
				select new OrderInfo
				{
					ID = order.ID,
					Number = order.Number,
					Date = order.Date,
					ProviderID = provider.ID,
					ProviderName = provider.Name
				};
			return query;
		}
	}
}
