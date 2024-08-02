namespace Pos.UI.OpenApi
{
	using Microsoft.Extensions.DependencyInjection;

	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddPosOpenApiDocument(
			this IServiceCollection serviceCollection) =>
			serviceCollection
				.AddOpenApiDocument(
					settings =>
					{
						settings.DocumentName = "pos";
						settings.Version = "0.1.0";
						settings.Title = "Promet Order System";
						settings.Description = "Promet Order System test task";
						settings.UseRouteNameAsOperationId = true;
						settings.OperationProcessors.Add(new HideODataMetadataOperations());
						settings.OperationProcessors.Add(new ApplyODataOptionsProcessor());
					});
	}
}