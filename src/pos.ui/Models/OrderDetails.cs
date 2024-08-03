namespace Pos.UI.Models
{
	using System;

	public class OrderDetails : OrderInfo
	{
		public OrderItem[] Items { get; init; } = [];
	}
}