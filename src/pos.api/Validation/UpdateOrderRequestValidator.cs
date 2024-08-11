namespace Pos.Api.Validation
{
	using Microsoft.Extensions.Localization;
	using Pos.Api.DataLayer;
	using Pos.Api.Models;

	internal class UpdateOrderRequestValidator : OrderRequestValidator<UpdateOrderRequest, UpdateOrderItem>
	{
		public UpdateOrderRequestValidator(IStringLocalizer<Messages> localizer, IPosDataConnection dataConnection)
			: base(localizer, dataConnection)
		{
		}
	}
}
