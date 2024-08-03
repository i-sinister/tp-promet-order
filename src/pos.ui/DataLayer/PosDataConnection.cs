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
	}
}