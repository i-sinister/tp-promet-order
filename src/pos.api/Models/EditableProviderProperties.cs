namespace Pos.Api.Models
{
	using System.ComponentModel.DataAnnotations;

	/// <summary>
	/// Provider information
	/// </summary>
	public class EditableProviderProperties
	{
		/// <summary>
		/// Provider name
		/// </summary>
		[Required(AllowEmptyStrings = false)]
		[RegularExpression("^[a-zA-Z0-9 \\-_\\(\\)]+$")]
		[MinLength(1)]
		[MaxLength(128)]
		public string Name { get; set; } = string.Empty;
	}
}
