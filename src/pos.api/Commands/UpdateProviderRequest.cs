namespace Pos.Api.Commands
{
	public class UpdateProviderRequest
	{
		public UpdateProviderRequest(int providerID, Models.UpdateProviderRequest provider)
		{
			ProviderID = providerID;
			Provider = provider;
		}

		public int ProviderID { get; }

		public Models.UpdateProviderRequest Provider { get; }
	}
}
