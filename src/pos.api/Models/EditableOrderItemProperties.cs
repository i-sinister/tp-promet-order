namespace Pos.Api.Models
{
	using System.ComponentModel.DataAnnotations;

	/// <summary>
	/// Order iten information
	/// </summary>
	public class EditableOrderItemProperties
	{
		/// <summary>
		/// Order item name
		/// </summary>
		[Required(AllowEmptyStrings = false)]
		[RegularExpression("^[a-zA-Z0-9 \\-_]+$")]
		[MinLength(1)]
		[MaxLength(128)]
		public string Name { get; set; } = string.Empty;

		/// <summary>
		/// Order item quantity
		/// </summary>
		[Required]
		[Range(0, double.MaxValue)]
		public double Quantity { get; set; }

		/// <summary>
		/// Order item unit
		/// </summary>
		[Required(AllowEmptyStrings = false)]
		[RegularExpression("^[a-zA-Z0-9 \\-_]+$")]
		[MinLength(1)]
		[MaxLength(128)]
		public string Unit { get; set; } = string.Empty;
	}
}
