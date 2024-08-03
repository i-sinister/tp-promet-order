namespace Pos.UI.Models
{
	public interface IOrderDetails<TItem>
		where TItem : EditableOrderItemProperties
	{
		TItem[] Items { get; }
	}
}