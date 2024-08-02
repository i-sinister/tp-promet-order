namespace Pos.UI.DataLayer
{
	using LinqToDB;
	using LinqToDB.Data;

	public class PosDataConnection : DataConnection, IPosDataConnection
	{
		public PosDataConnection(DataOptions<PosDataConnection> options)
			: base(options.Options)
		{
		}
	}
}