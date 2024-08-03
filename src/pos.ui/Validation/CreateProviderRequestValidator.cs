namespace Pos.UI.Validation
{
	using Microsoft.Extensions.Localization;
	using Pos.UI.DataLayer;
	using Pos.UI.Models;

	internal class CreateProviderRequestValidator : ProviderRequestValidator<CreateProviderRequest>
	{
		public CreateProviderRequestValidator(IStringLocalizer<Messages> localizer, IPosDataConnection dataConnection)
			: base(localizer, dataConnection)
		{
		}
	}
}