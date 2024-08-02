namespace Pos.UI
{
	using System.Threading;
	using System.Threading.Tasks;

	public interface IQuery<TItem>
	{
		Task<TItem> Execute(CancellationToken cancellationToken);
	}

	public interface IQuery<TRequest, TItem>
	{
		Task<TItem> Execute(TRequest request, CancellationToken cancellationToken);
	}
}