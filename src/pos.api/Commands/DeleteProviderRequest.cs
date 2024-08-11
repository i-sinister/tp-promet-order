namespace Pos.Api.Commands
{
	internal class DeleteProviderRequest
	{
		public DeleteProviderRequest(int providerID)
		{
			ProviderID = providerID;
		}

		public int ProviderID { get; }
	}
}
