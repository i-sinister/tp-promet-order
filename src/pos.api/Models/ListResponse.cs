namespace Pos.Api.Models
{
	using Newtonsoft.Json;
	using System.Diagnostics.CodeAnalysis;

	public class ListResponse<TItem>
	{
		/// <summary>
		/// Query result
		/// </summary>
		[NotNull]
		public TItem[] Items { get; init; } = default!;

		/// <summary>
		/// Row count without paging applied
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public long? Count { get; init; }
	}
}
