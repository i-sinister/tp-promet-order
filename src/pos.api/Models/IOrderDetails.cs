namespace Pos.Api.Models
{
	public interface IOrderDetails<TItem>
		where TItem : EditableOrderItemProperties
	{
		TItem[] Items { get; }
	}
}
