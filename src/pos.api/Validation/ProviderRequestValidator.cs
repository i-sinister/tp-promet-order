namespace Pos.Api.Validation
{
	using FluentValidation;
	using Microsoft.Extensions.Localization;
	using Pos.Api.DataLayer;
	using Pos.Api.Models;
	using System;
	using System.Threading;
	using System.Threading.Tasks;

	internal abstract class ProviderRequestValidator<TProvider> : AbstractValidator<TProvider>
		where TProvider : EditableProviderProperties
	{
		private readonly IPosDataConnection dataConnection;
		public ProviderRequestValidator(IStringLocalizer localizer, IPosDataConnection dataConnection)
		{
			this.dataConnection = dataConnection ?? throw new ArgumentNullException(nameof(dataConnection));
			RuleFor(request => request.Name)
				.MustAsync(ValidateProviderName)
				.WithMessage(localizer["provider.name.is_unique"]);
		}

		protected virtual async Task<bool> ValidateProviderName(
			TProvider request, string name, ValidationContext<TProvider> context, CancellationToken cancellationToken)
		{
			var providerID = 0;
			if (context.RootContextData.TryGetValue("providerID", out var rawProviderId)
				&& rawProviderId is int intProviderID)
			{
				providerID = intProviderID;
			}

			return !await dataConnection.DoesProviderExist(providerID, request.Name, cancellationToken);
		}
	}
}
