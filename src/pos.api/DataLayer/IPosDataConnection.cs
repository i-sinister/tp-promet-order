namespace Pos.Api.DataLayer
{
	using LinqToDB;
	using LinqToDB.Data;
	using Pos.Api.Models;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;

	public interface IPosDataConnection : IDataContext
	{
		Task<IPosDataConnection> Open(CancellationToken cancellationToken);

		Task<DataConnectionTransaction> BeginTransaction(CancellationToken cancellationToken);

		Task Commit(CancellationToken cancellationToken);

		IQueryable<Provider> GetProviders(CancellationToken cancellationToken);

		IQueryable<Order> GetOrders(CancellationToken cancellationToken);

		IQueryable<OrderItem> GetOrderItems(CancellationToken cancellationToken);

		Task<bool> DoesOrderExist(int orderID, int providerID, string number, CancellationToken cancellationToken);

		Task<bool> DoesProviderExist(int providerID, CancellationToken cancellationToken);

		Task<bool> DoesProviderExist(int providerID, string name, CancellationToken cancellationToken);

		Task<bool> IsProviderUsed(int providerID, CancellationToken cancellationToken);
	}
}
