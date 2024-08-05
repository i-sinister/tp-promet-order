namespace Pos.UI.Commands
{
	using LinqToDB;
	using Pos.UI.DataLayer;
	using System;
	using System.Threading;
	using System.Threading.Tasks;

	public class CreateProviderCommand : ICommand<CreateProviderRequest, int>
	{
		private readonly IPosDataConnection dataConnection;

		public CreateProviderCommand(IPosDataConnection dataConnection)
		{
			this.dataConnection = dataConnection ?? throw new ArgumentNullException(nameof(dataConnection));
		}

		public async Task<int> Execute(CreateProviderRequest request, CancellationToken cancellationToken)
		{
			if (request is null)
			{
				throw new ArgumentNullException(nameof(request));
			}

			var provider =
				new Models.Provider
				{
					Name = request.Provider.Name,
				};
			var providerID = await dataConnection.InsertWithInt32IdentityAsync(provider, token: cancellationToken);
			await dataConnection.Commit(cancellationToken);
			return providerID;
		}
	}
}