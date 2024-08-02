namespace Pos.UI.Queries
{
	using Microsoft.Extensions.DependencyInjection;

	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddQueries(
			this IServiceCollection serviceCollection) =>
			serviceCollection
				.AddTransient<ProvidersQuery>()
				.AddTransient<OrdersQuery>()
				.AddTransient<OrderDetailsQuery>();

	}
}