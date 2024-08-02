namespace Pos.UI.Application
{
	using Microsoft.AspNetCore.Builder;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Pos.UI.DataLayer;
	using System.Threading;
	using System.Threading.Tasks;

	public class StartServer
	{
		public async Task<int> Execute(string[] args, CancellationToken cancellationToken)
		{
			var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddPosDataConnection(builder.Configuration);
			builder.Services.AddControllers();
			var app = builder.Build();
			if (!app.Environment.IsDevelopment())
			{
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseRouting();
			await app.RunAsync();
			return 0;
		}
	}
}