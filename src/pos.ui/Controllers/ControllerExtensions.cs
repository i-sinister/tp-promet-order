namespace Pos.UI.Controllers
{
	using FluentValidation;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.DependencyInjection;
	using System.Threading;
	using System.Threading.Tasks;

	public static class ControllerExtensions
	{
		public static async Task<bool> Validate<T>(
			this ControllerBase controller,
			T request,
			string? keyName,
			int keyValue,
			CancellationToken cancellationToken)
		{
			var validator = controller.HttpContext.RequestServices.GetRequiredService<IValidator<T>>();
			var context = new ValidationContext<T>(request);
			if (!string.IsNullOrWhiteSpace(keyName))
			{
				context.RootContextData[keyName] = keyValue;
			}

			var result = await validator.ValidateAsync(context, cancellationToken);
			if (!result.IsValid)
			{
				result.AddToModelState(controller.ModelState);
				return false;
			}

			return true;
		}
	}
}