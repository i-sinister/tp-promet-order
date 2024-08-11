namespace Pos.Api.Validation
{
	using FluentValidation;
	using Microsoft.Extensions.Localization;
	using Pos.Api.DataLayer;
	using Pos.Api.Models;
	using System;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;

	internal abstract class OrderRequestValidator<TOrder, TItem> : AbstractValidator<TOrder>
		where TOrder : EditableOrderProperties, IOrderDetails<TItem>
		where TItem : EditableOrderItemProperties
	{
		private readonly IPosDataConnection dataConnection;
		public OrderRequestValidator(IStringLocalizer localizer, IPosDataConnection dataConnection)
		{
			this.dataConnection = dataConnection ?? throw new ArgumentNullException(nameof(dataConnection));
			RuleFor(request => request.Number)
				.MustAsync(ValidateProviderIDAndNumber)
				.WithMessage(localizer["order.number.providerID_and_number_are_unique"]);
			RuleFor(request => request.ProviderID)
				.MustAsync(ValidateProviderExists)
				.WithMessage(localizer["order.providerID.does_exist"]);
			RuleForEach(request => request.Items)
				.Must(ValidateItemNameDoesNotMatchOrderNumber)
				.WithMessage(localizer["order.items.name.does_not_match_order_number"])
				.Must(ValidateItemNameIsUnique)
				.WithMessage(localizer["order.items.name.is_unique"]);
		}

		protected virtual async Task<bool> ValidateProviderIDAndNumber(
			TOrder request, string number, ValidationContext<TOrder> context, CancellationToken cancellationToken)
		{
			var orderID = 0;
			if (context.RootContextData.TryGetValue("orderID", out var rawOrderID)
				&& rawOrderID is int intOrderID)
			{
				orderID = intOrderID;
			}

			return !await dataConnection.DoesOrderExist(orderID, request.ProviderID, number, cancellationToken);
		}

		protected virtual Task<bool> ValidateProviderExists(int providerID, CancellationToken cancellationToken) =>
			dataConnection.DoesProviderExist(providerID, cancellationToken);

		protected virtual bool ValidateItemNameDoesNotMatchOrderNumber(TOrder parent, TItem item) =>
			string.IsNullOrEmpty(item.Name)
				|| string.IsNullOrEmpty(parent.Number)
				|| !item.Name.Equals(parent.Number, StringComparison.InvariantCultureIgnoreCase);

		protected virtual bool ValidateItemNameIsUnique(TOrder parent, TItem item) =>
			string.IsNullOrWhiteSpace(item.Name)
				|| 1 == parent.Items.Count(
					i => item.Name.Equals(i.Name, StringComparison.InvariantCultureIgnoreCase));
	}
}
