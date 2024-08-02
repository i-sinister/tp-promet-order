namespace Pos.UI.DataLayer
{
	using LinqToDB;
	using LinqToDB.AspNet;
	using LinqToDB.AspNet.Logging;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;

	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddPosDataConnection(
			this IServiceCollection serviceCollection, IConfiguration configuration) =>
			serviceCollection
				.AddLinqToDBContext<IPosDataConnection, PosDataConnection>(
					(provider, options) =>
					options
						.UseSQLite(configuration.GetConnectionString("pos.config"))
						.UseDefaultLogging(provider));
	}
}