namespace Pos.UI.Application
{
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.OData;
	using Microsoft.AspNetCore.OData.NewtonsoftJson;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.OData.Edm;
	using Microsoft.OData.ModelBuilder;
	using Newtonsoft.Json.Serialization;
	using Pos.UI.Commands;
	using Pos.UI.DataLayer;
	using Pos.UI.Models;
	using Pos.UI.OpenApi;
	using Pos.UI.Queries;
	using Pos.UI.Validation;
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
				.AddModelValidation(new CamelCaseNamingStrategy())
				.AddLocalization(o => o.ResourcesPath = "Resources")
				.AddControllers()
				.AddOData(
					options => options
					 	.EnableQueryFeatures()
						.AddRouteComponents("api", GetEdmModel()))
				.AddNewtonsoftJson()
				.AddODataNewtonsoftJson()
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
			app.UseCors(
				policyBuilder => policyBuilder
					.WithOrigins("http://localhost:4200")
					.AllowAnyMethod()
			);
			app.UseStaticFiles();
			app.MapControllers();
			await app.RunAsync();
			return 0;
		}

		public static IEdmModel GetEdmModel()
		{
			var builder = new ODataConventionModelBuilder()
				.EnableLowerCamelCase();
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
