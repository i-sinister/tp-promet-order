namespace Pos.UI.DataLayer
{
	using LinqToDB;
	using LinqToDB.Data;
	using Pos.UI.Models;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;

	public interface IPosDataConnection : IDataContext
	{
		Task<IPosDataConnection> Open(CancellationToken cancellationToken);

		Task<DataConnectionTransaction> BeginTransaction(CancellationToken cancellationToken);

		IQueryable<Provider> GetProviders(CancellationToken cancellationToken);

		IQueryable<Order> GetOrders(CancellationToken cancellationToken);

		IQueryable<OrderItem> GetOrderItems(CancellationToken cancellationToken);
	}
}