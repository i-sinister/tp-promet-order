using LinqToDB;
using Pos.Api.DataLayer;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pos.Api.Commands
{
	public class DeleteOrderCommand : ICommand<DeleteOrderRequest, bool>
	{
		private readonly IPosDataConnection dataConnection;

		public DeleteOrderCommand(IPosDataConnection dataConnection)
		{
			this.dataConnection = dataConnection ?? throw new ArgumentNullException(nameof(dataConnection));
		}

		public async Task<bool> Execute(DeleteOrderRequest request, CancellationToken cancellationToken)
		{
			if (request is null)
			{
				throw new ArgumentNullException(nameof(request));
			}

			// we need to remove only orders, because item deletion is handled by sqlite
			var deletedOrderCount = await dataConnection
				.GetOrders(cancellationToken)
				.Where(order => order.ID == request.OrderID)
				.DeleteAsync(token: cancellationToken);
			await dataConnection.Commit(cancellationToken);
			return 0 < deletedOrderCount;
		}
	}
}
