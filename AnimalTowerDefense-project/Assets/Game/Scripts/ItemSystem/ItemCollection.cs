using UnityEngine;
using UnityEngine.Events;
namespace Game.Item
{
    [System.Serializable]
    public class ItemCollection
    {
        public delegate void EventRecover(ItemObject item);

        public EventRecover recoverEvent;
        public ItemObject item { get; private set; }
        public ItemUI UI;
        public int Quantity { get; private set; }
        public bool IsEquiped { get; set; }

        private float timer = 0;
        private bool isEffectedByItem = false;

        public ItemCollection(ItemObject item, int Quantity, ref UnityEvent events)
        {
            this.item = item;
            this.Quantity = Quantity;
            events.AddListener(TimerUpdate);
            recoverEvent = null;
        }

        public void AddItem(int amount)
        {
            Quantity+=amount;
            UI.TextAmount.text = Quantity.ToString();
            Debug.Log("ItemCollection : add item amount -> " + Quantity);
        }

        public void UseItem(int amount, EventRecover recoverCallback)
        {
            Quantity -= amount;
            if(item.time > 0)
            {
                timer = item.time;
                isEffectedByItem = true;
                recoverEvent = recoverCallback;
            }
        }

        void TimerUpdate()
        {
            if(timer > 0)
            {
                Debug.Log(item.name + " : " + timer);
                timer -= Time.deltaTime;
            }
            else
            {
                if(isEffectedByItem)
                {
                    recoverEvent(item);
                    isEffectedByItem = false;
                    recoverEvent = null;
                }
            }
        }
    }
}