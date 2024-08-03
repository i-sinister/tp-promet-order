namespace Pos.UI.Commands
{
	using Microsoft.Extensions.DependencyInjection;

	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddCommands(
			this IServiceCollection serviceCollection) =>
			serviceCollection
				.AddTransient<CreateOrderCommand>()
				.AddTransient<UpdateOrderCommand>()
				.AddTransient<DeleteOrderCommand>()
				.AddTransient<CreateProviderCommand>()
				.AddTransient<UpdateProviderCommand>()
				.AddTransient<DeleteProviderCommand>();
	}
}