namespace Pos.Api
{
	using System.Threading;
	using System.Threading.Tasks;

	public interface ICommand<TRequest, TResult>
	{
		Task<TResult> Execute(TRequest request, CancellationToken cancellationToken);
	}
}
