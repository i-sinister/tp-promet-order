namespace Pos.Api.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;

	/// <summary>
	/// Order information
	/// </summary>
	public class EditableOrderProperties
	{
		/// <summary>
		/// Order number
		/// </summary>
		[Required(AllowEmptyStrings = false)]
		[RegularExpression("^[a-zA-Z0-9\\-_]+$")]
		[MinLength(1)]
		[MaxLength(128)]
		public string Number { get; set; } = string.Empty;

		/// <summary>
		/// Order date
		/// </summary>
		[Required]
		public DateTime Date { get; set; }

		/// <summary>
		/// Order provider ID
		/// </summary>
		[Required]
		public int ProviderID { get; set; }
	}
}
