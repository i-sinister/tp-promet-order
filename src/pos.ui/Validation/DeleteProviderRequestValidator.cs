namespace Pos.UI.Validation
{
	using FluentValidation;
	using Microsoft.Extensions.Localization;
	using Pos.UI.Commands;
	using Pos.UI.DataLayer;
	using System;
	using System.Threading;
	using System.Threading.Tasks;

	internal class DeleteProviderRequestValidator : AbstractValidator<DeleteProviderRequest>
	{
		private readonly IPosDataConnection dataConnection;
		public DeleteProviderRequestValidator(IStringLocalizer<Messages> localizer, IPosDataConnection dataConnection)
		{
			this.dataConnection = dataConnection ?? throw new ArgumentNullException(nameof(dataConnection));
			RuleFor(request => request.ProviderID)
				.MustAsync(ValidateProviderNotUsed)
				.WithMessage(localizer["provider.is_not_used"]);
		}

		private async Task<bool> ValidateProviderNotUsed(int providerID, CancellationToken cancellationToken) =>
			!await dataConnection.IsProviderUsed(providerID, cancellationToken);
	}
}