namespace Pos.Api
{
	using System.Threading;
	using System.Threading.Tasks;

	public interface IQuery<TResult>
	{
		Task<TResult> Execute(CancellationToken cancellationToken);
	}

	public interface IQuery<TRequest, TResult>
	{
		Task<TResult> Execute(TRequest request, CancellationToken cancellationToken);
	}
}
