using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Game.Item;
public class TakeItemEvent : UnityEvent<ItemCollection> {}
public class UseItemEvent : UnityEvent<ItemCollection> {}
public class GameEvents
{

    private static GameEvents _instance = null;
    public static GameEvents Instance 
    {
        get{
            if(_instance ==null)
            {
                _instance = new GameEvents();
            }
            return _instance;
        }
    }
    public TakeItemEvent ETakeItemEvent;
    public UseItemEvent EUseItemEvent;

    public GameEvents()
    {
        ETakeItemEvent = new TakeItemEvent();
        EUseItemEvent = new UseItemEvent();

    }

}
