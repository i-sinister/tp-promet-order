namespace Pos.UI.Models
{
	using System;

	public class Provider : IEquatable<Provider>
	{
		public int ID { get; init; }

		public string Name { get; set; } = string.Empty;

		public override int GetHashCode() => ID;

		public override bool Equals(object? obj) => obj is Provider other && Equals(other);

		public bool Equals(Provider? other) => other is not null && ID == other.ID;
	}
}