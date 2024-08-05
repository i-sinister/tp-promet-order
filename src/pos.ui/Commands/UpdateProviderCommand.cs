namespace Pos.UI.Commands
{
	using LinqToDB;
	using Pos.UI.DataLayer;
	using System;
	using System.Threading;
	using System.Threading.Tasks;

	internal class UpdateProviderCommand : ICommand<UpdateProviderRequest, bool>
	{
		private readonly IPosDataConnection dataConnection;

		public UpdateProviderCommand(IPosDataConnection dataConnection)
		{
			this.dataConnection = dataConnection ?? throw new ArgumentNullException(nameof(dataConnection));
		}

		public async Task<bool> Execute(UpdateProviderRequest request, CancellationToken cancellationToken)
		{
			if (request is null)
			{
				throw new ArgumentNullException(nameof(request));
			}

			var provider = await dataConnection.GetProviders(cancellationToken)
				.FirstOrDefaultAsync(o => request.ProviderID == o.ID, cancellationToken);
			if (provider is null)
			{
				return false;
			}

			provider.Name = request.Provider.Name;
			await dataConnection.UpdateAsync(provider, token: cancellationToken);
			await dataConnection.Commit(cancellationToken);
			return true;
		}

	}
}
