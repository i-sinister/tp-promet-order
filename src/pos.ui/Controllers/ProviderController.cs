namespace Pos.UI.Controllers
{
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.OData.Query;
	using Microsoft.Extensions.DependencyInjection;
	using Pos.UI.Commands;
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

		/// <summary>
		/// Adds new provider
		/// </summary>
		/// <param name="provider">provider information</param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("", Name = "provider_create")]
		[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Create([FromBody]Models.CreateProviderRequest provider, CancellationToken cancellationToken)
		{
			if (!await this.Validate(provider, default, default, cancellationToken))
			{
				return ValidationProblem();
			}

			var query = HttpContext.RequestServices.GetRequiredService<CreateProviderCommand>();
			var providerID = await query.Execute(new Commands.CreateProviderRequest(provider), cancellationToken);
			return Created((string?)null, providerID);
		}

		/// <summary>
		/// Update existing provider
		/// </summary>
		/// <param name="providerID">existing provider ID</param>
		/// <param name="provider">provider information</param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("{providerID}", Name = "provider_update")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Update(int providerID, [FromBody]Models.UpdateProviderRequest provider, CancellationToken cancellationToken)
		{
			if (!await this.Validate(provider, nameof(providerID), providerID, cancellationToken))
			{
				return ValidationProblem();
			}

			var query = HttpContext.RequestServices.GetRequiredService<UpdateProviderCommand>();
			var updated = await query.Execute(new Commands.UpdateProviderRequest(providerID, provider), cancellationToken);
			return updated ? Ok() : NotFound();
		}

		/// <summary>
		/// Delete existing provider
		/// </summary>
		/// <param name="providerID">existing provider ID</param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpDelete]
		[Route("{providerID}", Name = "provider_delete")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Delete(int providerID, CancellationToken cancellationToken)
		{
			var request = new DeleteProviderRequest(providerID);
			if (!await this.Validate(request, default, default, cancellationToken))
			{
				return ValidationProblem();
			}

			var query = HttpContext.RequestServices.GetRequiredService<DeleteProviderCommand>();
			var deleted = await query.Execute(request, cancellationToken);
			return deleted ? NoContent() : NotFound();
		}
	}
}