namespace Pos.UI.Controllers
{
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.OData.Query;
	using Microsoft.Extensions.DependencyInjection;
	using Pos.UI.Models;
	using Pos.UI.Queries;
	using System.Threading;
	using System.Threading.Tasks;

	/// <summary>
	/// API to manage providers
	/// </summary>
	[ApiController]
	[Route("api/providers")]
	public class ProviderController : ControllerBase
	{
		/// <summary>
		/// Returns all providers
		/// </summary>
		/// <param name="odataOptions"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[EnableQuery]
		[HttpGet]
		[Route("", Name = "provider_getAll")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProvidersListResponse))]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Get(ODataQueryOptions<Provider> odataOptions, CancellationToken cancellationToken)
		{
			var query = HttpContext.RequestServices.GetRequiredService<ProvidersQuery>();
			var items = await query.Execute(cancellationToken);
			var result = odataOptions.ApplyTo(items).ToListResult();
			return Ok(result);
		}
	}
}