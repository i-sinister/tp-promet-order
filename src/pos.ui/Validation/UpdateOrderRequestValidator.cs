namespace Pos.UI.Validation
{
	using Microsoft.Extensions.Localization;
	using Pos.UI.DataLayer;
	using Pos.UI.Models;

	internal class UpdateOrderRequestValidator : OrderRequestValidator<UpdateOrderRequest, UpdateOrderItem>
	{
		public UpdateOrderRequestValidator(IStringLocalizer<Messages> localizer, IPosDataConnection dataConnection)
			: base(localizer, dataConnection)
		{
		}
	}
}