namespace Pos.Api.Models
{
	using System.ComponentModel.DataAnnotations;

	/// <summary>
	/// Order item information
	/// </summary>
	public class UpdateOrderItem : EditableOrderItemProperties
	{
		/// <summary>
		/// Order item unique identifier
		/// </summary>
		[Required]
		public int ID { get; init; }
	}
}
