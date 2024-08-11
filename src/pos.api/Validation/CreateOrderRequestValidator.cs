namespace Pos.Api.Validation
{
	using Microsoft.Extensions.Localization;
	using Pos.Api.DataLayer;
	using Pos.Api.Models;

	internal class CreateOrderRequestValidator : OrderRequestValidator<CreateOrderRequest, CreateOrderItem>
	{
		public CreateOrderRequestValidator(IStringLocalizer<Messages> localizer, IPosDataConnection dataConnection)
			: base(localizer, dataConnection)
		{
		}
	}
}
