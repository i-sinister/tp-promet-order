namespace Pos.UI.Models
{
	using System.Diagnostics.CodeAnalysis;
	using System.Linq;

	public class ListResponse<TItem>
	{
		/// <summary>
		/// Query result
		/// </summary>
		[NotNull]
		public IQueryable<TItem> Items { get; init; } = default!;
	}
}