namespace Pos.Api.OpenApi
{
	using Microsoft.AspNetCore.Builder;

	public static class WebApplicationExtensions
	{
		public static WebApplication AddPosSwaggerUI(
			this WebApplication webApplication)
		{
			var openApiSchemaPath = "/openApi/pos.json";
			webApplication.UseOpenApi(
				settings =>
				{
					settings.DocumentName = "pos";
					settings.Path = openApiSchemaPath;
				});
			webApplication.UseSwaggerUi(
				settings =>
				{
					settings.DocumentPath = openApiSchemaPath;
					settings.DocumentTitle = "Promet Order System";
					settings.Path = "/openApi";
				});
			return webApplication;
		}
	}
}
