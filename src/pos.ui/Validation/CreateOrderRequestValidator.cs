namespace Pos.UI.Validation
{
	using Microsoft.Extensions.Localization;
	using Pos.UI.DataLayer;
	using Pos.UI.Models;

	internal class CreateOrderRequestValidator : OrderRequestValidator<CreateOrderRequest, CreateOrderItem>
	{
		public CreateOrderRequestValidator(IStringLocalizer<Messages> localizer, IPosDataConnection dataConnection)
			: base(localizer, dataConnection)
		{
		}
	}
}