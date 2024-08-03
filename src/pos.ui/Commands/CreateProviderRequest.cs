
namespace Pos.UI.Commands
{
	using System;

	public class CreateProviderRequest
	{
		public CreateProviderRequest(Models.CreateProviderRequest provider)
		{
			Provider = provider ?? throw new ArgumentNullException(nameof(provider));
		}

		public Models.CreateProviderRequest Provider { get; }
	}
}