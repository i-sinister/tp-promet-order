namespace Pos.UI.Commands
{
	using LinqToDB;
	using Pos.UI.DataLayer;
	using System;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;

	internal class DeleteProviderCommand : ICommand<DeleteProviderRequest, bool>
	{
		private readonly IPosDataConnection dataConnection;

		public DeleteProviderCommand(IPosDataConnection dataConnection)
		{
			this.dataConnection = dataConnection ?? throw new ArgumentNullException(nameof(dataConnection));
		}

		public async Task<bool> Execute(DeleteProviderRequest request, CancellationToken cancellationToken)
		{
			if (request is null)
			{
				throw new ArgumentNullException(nameof(request));
			}

			await using var transaction = await dataConnection.BeginTransaction(cancellationToken);

			// we need to remove only providers, because order deletion is handled by sqlite
			var deletedProviderCount = await dataConnection
				.GetProviders(cancellationToken)
				.Where(order => order.ID == request.ProviderID)
				.DeleteAsync(token: cancellationToken);
			await transaction.CommitAsync(cancellationToken);
			return 0 < deletedProviderCount;
		}
	}
}