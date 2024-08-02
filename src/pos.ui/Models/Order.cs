namespace Pos.UI.Models
{
	using System;

	public class Order : IEquatable<Order>
	{
		public int ID { get; init; }

		public string Number { get; set; } = string.Empty;

		public DateTime Date { get; set; }

		public int ProviderID { get; set; }

		public override int GetHashCode() => ID;

		public override bool Equals(object? obj) => obj is Order other && Equals(other);

		public bool Equals(Order? other) => other is not null && ID == other.ID;
	}

}