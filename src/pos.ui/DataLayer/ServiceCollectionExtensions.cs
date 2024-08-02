namespace Pos.UI.DataLayer
{
	using LinqToDB;
	using LinqToDB.AspNet;
	using LinqToDB.AspNet.Logging;
	using LinqToDB.Mapping;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Pos.UI.Models;

	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddPosDataConnection(
			this IServiceCollection serviceCollection, IConfiguration configuration) =>
			serviceCollection
				.AddLinqToDBContext<IPosDataConnection, PosDataConnection>(
					(provider, options) =>
					options
						.UseSQLite(configuration.GetConnectionString("pos.config")!)
						.UseDefaultLogging(provider)
						.UseMappingSchema(CreateSchema()));

		public static MappingSchema CreateSchema()
		{
			var schema = new MappingSchema();
			var builder = new FluentMappingBuilder(schema);
			builder
				.Entity<Provider>()
				.HasTableName("PROVIDER")
				.HasIdentity(provider => provider.ID)
				.HasPrimaryKey(provider => provider.ID)
				.Property(provider => provider.Name);
			builder
				.Entity<Order>()
				.HasTableName("ORDER")
				.HasIdentity(item => item.ID)
				.HasPrimaryKey(item => item.ID)
				.Property(item => item.Number)
				.Property(item => item.Date)
				.Property(item => item.ProviderID);
			builder
				.Entity<OrderItem>()
				.HasTableName("ORDERITEM")
				.HasIdentity(item => item.ID)
				.HasPrimaryKey(item => item.ID)
				.Property(item => item.OrderID)
				.Property(item => item.Name)
				.Property(item => item.Quantity)
				.Property(item => item.Unit);
			builder.Build();
			return schema;
		}
	}
}