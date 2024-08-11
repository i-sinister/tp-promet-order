namespace Pos.Api.Models
{
	using System.ComponentModel.DataAnnotations;

	/// <summary>
	/// Order information
	/// </summary>
	public class UpdateOrderRequest : EditableOrderProperties, IOrderDetails<UpdateOrderItem>
	{
		/// <summary>
		/// Order items
		/// </summary>
		[Required]
		[MinLength(1)]
		public UpdateOrderItem[] Items { get; set; } = [];
	}
}
