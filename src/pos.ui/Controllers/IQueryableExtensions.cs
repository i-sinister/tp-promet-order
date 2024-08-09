namespace Pos.UI.Controllers
{
	using LinqToDB;
	using Microsoft.AspNetCore.OData.Query;
	using Pos.UI.Models;
	using System;
	using System.Linq;
	using System.Reflection;
	using System.Threading;
	using System.Threading.Tasks;

	public static class IQueryableExtensions
	{
		private static readonly MethodInfo ApplyImplMethodInfo =
		 typeof(IQueryableExtensions).GetMethod(nameof(ApplyImpl), BindingFlags.NonPublic | BindingFlags.Static)!;

		public static Task<object> Apply<T>(
			this IQueryable<T> source,
			ODataQueryOptions<T> queryOptions,
			CancellationToken cancellationToken)
		{
			var countQueryable = queryOptions.Count?.Value ?? false
				? queryOptions.ApplyTo(source, AllowedQueryOptions.All & ~(AllowedQueryOptions.Count | AllowedQueryOptions.Filter))
				: null;
			var settings =
				new ODataQuerySettings
				{
					EnsureStableOrdering = false,
					HandleNullPropagation = HandleNullPropagationOption.False,
				};
			var queryable = queryOptions.ApplyTo(source, settings);
			var genericApplyImpl = ApplyImplMethodInfo.MakeGenericMethod(typeof(T), queryable.ElementType);
			return (Task<object>)genericApplyImpl.Invoke(null, [countQueryable, queryable, cancellationToken])!;
		}

		private static async Task<object> ApplyImpl<TSource, TResult>(
			IQueryable<TSource> countQueryable,
			IQueryable<TResult> resultQueryable,
			CancellationToken cancellationToken)
		{
			var count = countQueryable is null
				? (long?)null
				: await countQueryable.LongCountAsync(cancellationToken);
			var items  = await resultQueryable.ToArrayAsync(cancellationToken);
			return new ListResponse<TResult> { Items = items, Count = count };
		}
	}
}
