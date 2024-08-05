namespace Pos.UI.Commands
{
	using LinqToDB;
	using Pos.UI.DataLayer;
	using Pos.UI.Models;
	using System;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;

	public class UpdateOrderCommand : ICommand<UpdateOrderRequest, bool>
	{
		private readonly IPosDataConnection dataConnection;

		public UpdateOrderCommand(IPosDataConnection dataConnection)
		{
			this.dataConnection = dataConnection ?? throw new ArgumentNullException(nameof(dataConnection));
		}

		public async Task<bool> Execute(UpdateOrderRequest request, CancellationToken cancellationToken)
		{
			if (request is null)
			{
				throw new ArgumentNullException(nameof(request));
			}

			var order = await dataConnection.GetOrders(cancellationToken)
				.FirstOrDefaultAsync(o => request.OrderID == o.ID, cancellationToken);
			if (order is null)
			{
				return false;
			}

			order.Number = request.Order.Number;
			order.Date = request.Order.Date;
			order.ProviderID = request.Order.ProviderID;
			await dataConnection.UpdateAsync(order, token: cancellationToken);
			var itemsToKeep = request.Order.Items.Select(item => item.ID).ToHashSet();
			await dataConnection
				.GetOrderItems(cancellationToken)
				.Where(item => item.OrderID == request.OrderID && !itemsToKeep.Contains(item.ID))
				.DeleteAsync();
			var items = await dataConnection
				.GetOrderItems(cancellationToken)
				.Where(item => item.OrderID == request.OrderID)
				.ToArrayAsync(cancellationToken);
			foreach (var requestItem in request.Order.Items)
			{
				// there wont be many items anyway so linear search is ok
				var item = items.FirstOrDefault(i => i.ID == requestItem.ID);
				if (item is null)
				{
					item =
						new OrderItem
						{
							OrderID = order.ID,
							Name = requestItem.Name,
							Quantity = requestItem.Quantity,
							Unit = requestItem.Unit,
						};
					await dataConnection.InsertAsync(item, token: cancellationToken);
				}
				else
				{
					item.Name = requestItem.Name;
					item.Quantity = requestItem.Quantity;
					item.Unit = requestItem.Unit;
					await dataConnection.UpdateAsync(item, token: cancellationToken);
				}
			}

			await dataConnection.Commit(cancellationToken);
			return true;
		}
	}
}