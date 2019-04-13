using Game.Item;
using UnityEngine;

public interface ICanUseItem
{
    void TakeItem(ItemObject item);
    void UseItem(ItemObject item);
}