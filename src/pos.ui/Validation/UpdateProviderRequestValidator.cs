namespace Pos.UI.Validation
{
	using Microsoft.Extensions.Localization;
	using Pos.UI.DataLayer;
	using Pos.UI.Models;

	internal class UpdateProviderRequestValidator : ProviderRequestValidator<UpdateProviderRequest>
	{
		public UpdateProviderRequestValidator(IStringLocalizer<Messages> localizer, IPosDataConnection dataConnection)
			: base(localizer, dataConnection)
		{
		}
	}
}