namespace Pos.Api.Validation
{
	using FluentValidation;
	using FluentValidation.Internal;
	using Microsoft.Extensions.DependencyInjection;
	using Newtonsoft.Json.Serialization;
	using Pos.Api.Models;
	using System;
	using System.Linq;

	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddModelValidation(
			this IServiceCollection serviceCollection,
			CamelCaseNamingStrategy namingStrategy)
		{
			ValidatorOptions.Global.PropertyNameResolver =
				(type, member, expression) =>
				{
					var fullName = expression is null ? member.Name : PropertyChain.FromExpression(expression).ToString();
					var propertySeparator = ValidatorOptions.Global.PropertyChainSeparator;
					var tweakedParts = fullName
						.Split(propertySeparator, StringSplitOptions.RemoveEmptyEntries)
						.Select(part => namingStrategy.GetPropertyName(part, false));
					return string.Join(propertySeparator, tweakedParts);
				};
			return serviceCollection
				.AddScoped<IValidator<CreateOrderRequest>, CreateOrderRequestValidator>()
				.AddScoped<IValidator<UpdateOrderRequest>, UpdateOrderRequestValidator>()
				.AddScoped<IValidator<CreateProviderRequest>, CreateProviderRequestValidator>()
				.AddScoped<IValidator<UpdateProviderRequest>, UpdateProviderRequestValidator>()
				.AddScoped<IValidator<Commands.DeleteProviderRequest>, DeleteProviderRequestValidator>();
		}
	}
}
