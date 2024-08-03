[assembly: Microsoft.Extensions.Localization.RootNamespace("Pos.UI")]
namespace Pos.UI.Application
{
	using System.CommandLine;
	using System.Threading.Tasks;

	public static class Cli
	{
		public static async Task<int> Main(string[] args)
		{
			var rootCommand = CreateStartServerCommand(args);
			return await rootCommand.InvokeAsync(args);
		}

		public static RootCommand CreateStartServerCommand(string[] args)
		{
			var command = new RootCommand("Start POC UI");
			command.SetHandler(
				async context =>
				{
					var startServer = new StartServer();
					var cancellationToken = context.GetCancellationToken();
					context.ExitCode = await startServer.Execute(args, cancellationToken);
				}
			);

			return command;
		}
	}
}