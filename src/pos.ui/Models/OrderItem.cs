namespace Pos.UI.Models
{
	using System;

	public class OrderItem : IEquatable<OrderItem>
	{
		public int ID { get; init; }

		public int OrderID { get; init; }

		public string Name { get; set; } = string.Empty;

		public double Quantity { get; set; }

		public string Unit { get; set; } = string.Empty;

		public override int GetHashCode() => ID;

		public override bool Equals(object? obj) => obj is OrderItem other && Equals(other);

		public bool Equals(OrderItem? other) => other is not null && ID == other.ID;
	}

}