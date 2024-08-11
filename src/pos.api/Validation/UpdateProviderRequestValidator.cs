namespace Pos.Api.Validation
{
	using Microsoft.Extensions.Localization;
	using Pos.Api.DataLayer;
	using Pos.Api.Models;

	internal class UpdateProviderRequestValidator : ProviderRequestValidator<UpdateProviderRequest>
	{
		public UpdateProviderRequestValidator(IStringLocalizer<Messages> localizer, IPosDataConnection dataConnection)
			: base(localizer, dataConnection)
		{
		}
	}
}
