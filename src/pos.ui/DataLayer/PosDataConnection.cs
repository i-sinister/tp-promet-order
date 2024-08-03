namespace Pos.UI.DataLayer
{
	using LinqToDB;
	using LinqToDB.Data;
	using Pos.UI.Models;
	using System.Data;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;

	public class PosDataConnection : DataConnection, IPosDataConnection
	{
		public PosDataConnection(DataOptions<PosDataConnection> options)
			: base(options.Options)
		{
		}

		public async Task<IPosDataConnection> Open(CancellationToken cancellationToken)
		{
			if (ConnectionState.Closed == Connection.State)
			{
				await Connection.OpenAsync(cancellationToken);
			}

			return this;
		}

		public Task<DataConnectionTransaction> BeginTransaction(CancellationToken cancellallationToken) =>
			BeginTransactionAsync(cancellallationToken);

		public IQueryable<OrderItem> GetOrderItems(CancellationToken cancellationToken) =>
			this.GetTable<OrderItem>();

		public IQueryable<Order> GetOrders(CancellationToken cancellationToken) =>
			this.GetTable<Order>();

		public IQueryable<Provider> GetProviders(CancellationToken cancellationToken) =>
			this.GetTable<Provider>();

		public Task<bool> DoesOrderExist(int orderID, int providerID, string number, CancellationToken cancellationToken) =>
			GetOrders(cancellationToken)
				.AnyAsync(
					order => order.ID != orderID && order.Number == number && order.ProviderID == providerID,
					cancellationToken);

		public Task<bool> DoesProviderExist(int providerID, CancellationToken cancellationToken) =>
			GetProviders(cancellationToken).AnyAsync(provider => provider.ID == providerID, cancellationToken);

		public Task<bool> DoesProviderExist(int providerID, string name, CancellationToken cancellationToken) =>
			GetProviders(cancellationToken)
				.AnyAsync(
					provider => provider.ID != providerID && provider.Name == name,
					cancellationToken);

		public Task<bool> IsProviderUsed(int providerID, CancellationToken cancellationToken) =>
			GetOrders(cancellationToken)
				.AnyAsync(order => order.ProviderID != providerID, cancellationToken);
	}
}