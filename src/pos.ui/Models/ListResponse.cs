namespace Pos.UI.Models
{
	using System.Diagnostics.CodeAnalysis;
	using System.Linq;

	public class ListResponse<TItem>
	{
		[NotNull]
		public IQueryable<TItem> Items { get; init; } = default!;
	}
}