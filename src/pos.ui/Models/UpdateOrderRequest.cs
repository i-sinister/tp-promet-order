namespace Pos.UI.Models
{
	using System.ComponentModel.DataAnnotations;

	/// <summary>
	/// Order information
	/// </summary>
	public class UpdateOrderRequest : EditableOrderProperties
	{
		/// <summary>
		/// Order items
		/// </summary>
		[Required]
		[MinLength(1)]
		public UpdateOrderItem[] Items { get; set; } = [];
	}
}