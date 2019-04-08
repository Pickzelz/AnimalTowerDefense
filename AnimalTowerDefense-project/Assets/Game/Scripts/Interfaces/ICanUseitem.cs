using Game.Item;

public interface ICanUseItem
{
    void TakeItem(ItemObject item);
    void UseItem();
}