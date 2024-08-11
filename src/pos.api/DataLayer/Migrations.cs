namespace Pos.Api.DataLayer
{
	using LinqToDB;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;
	using System;
	using System.Collections.Immutable;
	using System.Data.SQLite;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;

	public class Migrations
	{
		private readonly IServiceProvider services;

		public Migrations(IServiceProvider services)
		{
			this.services = services ?? throw new ArgumentNullException(nameof(services));
		}

		internal async Task Apply(CancellationToken cancellationToken)
		{
			await using var scope = services.CreateAsyncScope();
			var logger = scope.ServiceProvider.GetRequiredService<ILogger<Migrations>>();
			logger.LogDebug("Connecting to the database");
			var options = scope.ServiceProvider.GetRequiredService<DataOptions<PosDataConnection>>();
			var connectionString = options.Options.ConnectionOptions.ConnectionString;
			var dataSource = new SQLiteConnectionStringBuilder(connectionString).DataSource;
			dataSource = Path.Combine(Directory.GetCurrentDirectory(), dataSource);
			var databaseFolder = Path.GetDirectoryName(dataSource);
			if (!string.IsNullOrEmpty(databaseFolder) && !Directory.Exists(databaseFolder))
			{
				Directory.CreateDirectory(databaseFolder);
			}

			if (!":memory:".Equals(dataSource, StringComparison.InvariantCultureIgnoreCase))
			{
				if (File.Exists(dataSource))
				{
					File.Delete(dataSource);
				}

				SQLiteConnection.CreateFile(dataSource);
			}

			using var connection = new SQLiteConnection();
			connection.ConnectionString = connectionString;
			await connection.OpenAsync(cancellationToken);
			logger.LogDebug("Discovering migrations");
			var applicationPath = Process.GetCurrentProcess().MainModule!.FileName;
			var applicationFolder = Path.GetDirectoryName(applicationPath)!;
			var migrationsPath = Path.Combine(applicationFolder, "Migrations");
			var migrationNames = Directory.GetFiles(migrationsPath, "*.sql")
				.OrderBy(name => name)
				.ToImmutableArray();
			logger.LogDebug("Discovered {migrationCount} migrations", migrationNames.Length);
			foreach (var migrationName in migrationNames)
			{
				logger.LogDebug("Executing migration '{migration}'", Path.GetFileName(migrationName));
				using var command = connection.CreateCommand();
				command.CommandText = await File.ReadAllTextAsync(migrationName);
				await command.ExecuteNonQueryAsync(cancellationToken);
			}

			await connection.CloseAsync();
			logger.LogDebug("Migrations completed");
		}
	}
}
