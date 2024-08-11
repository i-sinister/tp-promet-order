namespace Pos.Api.Validation
{
	using Microsoft.Extensions.Localization;
	using Pos.Api.DataLayer;
	using Pos.Api.Models;

	internal class CreateProviderRequestValidator : ProviderRequestValidator<CreateProviderRequest>
	{
		public CreateProviderRequestValidator(IStringLocalizer<Messages> localizer, IPosDataConnection dataConnection)
			: base(localizer, dataConnection)
		{
		}
	}
}
