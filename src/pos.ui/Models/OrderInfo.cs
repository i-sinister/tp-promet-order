namespace Pos.UI.Models
{
	using System;

	public class OrderInfo
	{
		public int ID { get; init; }

		public string Number { get; init; } = string.Empty;

		public DateTime Date { get; init; }

		public int ProviderID { get; init; }

		public string ProviderName { get; init; } = string.Empty;
	}
}