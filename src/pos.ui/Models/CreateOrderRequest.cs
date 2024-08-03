namespace Pos.UI.Models
{
	using System.ComponentModel.DataAnnotations;

	/// <summary>
	/// Order information
	/// </summary>
	public class CreateOrderRequest : EditableOrderProperties, IOrderDetails<CreateOrderItem>
	{
		/// <summary>
		/// Order items
		/// </summary>
		[Required]
		[MinLength(1)]
		public CreateOrderItem[] Items { get; set; } = [];
	}
}