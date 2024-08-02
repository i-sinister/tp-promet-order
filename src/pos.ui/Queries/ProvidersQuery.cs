namespace Pos.UI.Queries
{
	using Pos.UI.DataLayer;
	using Pos.UI.Models;
	using System;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;

	public class ProvidersQuery : IQuery<IQueryable<Provider>>
	{
		private readonly IPosDataConnection dataConnection;

		public ProvidersQuery(IPosDataConnection dataConnection)
		{
			this.dataConnection = dataConnection ?? throw new ArgumentNullException(nameof(dataConnection));
		}

		public async Task<IQueryable<Provider>> Execute(CancellationToken cancellationToken) =>
			(await dataConnection.Open(cancellationToken)).GetProviders(cancellationToken);
	}
}