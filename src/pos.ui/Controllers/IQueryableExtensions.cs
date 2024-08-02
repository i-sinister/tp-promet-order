namespace Pos.UI.Controllers
{
	using Pos.UI.Models;
	using System;
	using System.Linq;

	public static class IQueryableExtensions
	{
		public static object ToListResult(this IQueryable queryable)
		{
			var itemType = queryable.ElementType;
			var resultType = typeof(ListResponse<>).MakeGenericType(itemType);
			var result = Activator.CreateInstance(resultType)!;
			resultType.GetProperty(nameof(ListResponse<object>.Items))!.SetValue(result, queryable);
			return result;
		}
	}
}