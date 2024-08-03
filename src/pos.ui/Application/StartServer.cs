namespace Pos.UI.Application
{
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.OData;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.OData.Edm;
	using Microsoft.OData.ModelBuilder;
	using Pos.UI.Commands;
	using Pos.UI.DataLayer;
	using Pos.UI.Models;
	using Pos.UI.OpenApi;
	using Pos.UI.Queries;
	using System.Threading;
	using System.Threading.Tasks;

	public class StartServer
	{
		public async Task<int> Execute(string[] args, CancellationToken cancellationToken)
		{
			var builder = WebApplication.CreateBuilder(args);
			builder.Services
				.AddPosDataConnection(builder.Configuration)
				.AddQueries()
				.AddCommands()
				.AddControllers()
				.AddNewtonsoftJson()
				.AddOData(
					options => options
					 	.EnableQueryFeatures()
						.AddRouteComponents("api", GetEdmModel()))
				.Services
				.AddPosOpenApiDocument();
			var app = builder.Build();
			await new Migrations(app.Services).Apply(cancellationToken);
			if (!app.Environment.IsDevelopment())
			{
				app.UseHsts();
			}

			app.AddPosSwaggerUI();
			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.MapControllers();
			await app.RunAsync();
			return 0;
		}

		public static IEdmModel GetEdmModel()
		{
			var builder = new ODataConventionModelBuilder();
			builder
				.EntitySet<Provider>("Providers")
				.EntityType
				.HasKey(provider => provider.ID);
			builder
				.EntitySet<OrderInfo>("OrderInfo")
				.EntityType
				.HasKey(order => order.ID);
			builder
				.EntitySet<OrderDetails>("OrderDetails")
				.EntityType
				.HasKey(order => order.ID);
			return builder.GetEdmModel();
		}
	}
}