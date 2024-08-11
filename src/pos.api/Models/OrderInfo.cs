namespace Pos.Api.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;

	/// <summary>
	/// Order information
	/// </summary>
	public class OrderInfo
	{
		[Required]
		public int ID { get; init; }

		[Required(AllowEmptyStrings = false)]
		public string Number { get; init; } = string.Empty;

		[Required]
		public DateTime Date { get; init; }

		[Required]
		public int ProviderID { get; init; }

		[Required(AllowEmptyStrings = false)]
		public string ProviderName { get; init; } = string.Empty;
	}
}
