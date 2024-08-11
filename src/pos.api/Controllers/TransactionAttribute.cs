namespace Pos.Api.Controllers
{
	using Microsoft.AspNetCore.Mvc.Filters;
	using Microsoft.Extensions.DependencyInjection;
	using Pos.Api.DataLayer;
	using System.Threading.Tasks;

	public class TransactionAttribute : ActionFilterAttribute
	{
		public override async Task OnActionExecutionAsync(
			ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var cancellationToken = context.HttpContext.RequestAborted;
			var connection = context.HttpContext.RequestServices.GetRequiredService<IPosDataConnection>();
			using var transaction = await connection.BeginTransaction(cancellationToken);
			await base.OnActionExecutionAsync(context, next);
		}
	}
}
