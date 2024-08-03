namespace Pos.UI.Commands
{
	public class DeleteOrderRequest
	{
		public DeleteOrderRequest(int orderID)
		{
			OrderID = orderID;
		}

		public int OrderID { get; }
	}
}