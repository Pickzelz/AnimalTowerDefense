using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using ATD;

namespace Game.Item
{
    [RequireComponent(typeof(Rigidbody))]
    [System.Serializable]
    public class ItemForCharacter : MonoBehaviourPunCallbacks, ICanEffectByItem, ICanUseItem
    {
        public UnityEvent TimerEvent;

        [System.Serializable]
        public class ItemCollection
        {
            public delegate void EventRecover(ItemObject item);

            public EventRecover recoverEvent;
            public ItemObject item { get; private set; }
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
        private Character _character;
        public List<ItemCollection> CollectedItem;
        // Start is called before the first frame update
        void Start()
        {
            CollectedItem = new List<ItemCollection>();
            _character = GetComponent<Character>();
        }

        // Update is called once per frame
        void Update()
        {
            TimerEvent.Invoke();
        }

        public void TakeItem(ItemObject item)
        {
            if (!photonView.IsMine)
                return;

            PhotonView view = PhotonView.Get(this);
            view.RPC("RPCAddItem", RpcTarget.All, item.ItemName, 1);
        }

        [PunRPC]
        void RPCAddItem(string itemName, int amount)
        {
            if (CollectedItem.Exists(x => x.item.ItemName == itemName))
            {
                ItemCollection collection = CollectedItem.Find(x => x.item.ItemName == itemName);
                collection.AddItem(1);
                Debug.Log("Total item in collection " + collection.Quantity);
                UseItem(collection.item);

            }
            else
            {
                Debug.Log("Add new item ");
                ItemObject objectCpy = ItemManager.Instance.Getitem(itemName);
                ItemCollection collection = new ItemCollection(objectCpy, 1, ref TimerEvent);

                CollectedItem.Add(collection);
                UseItem(collection.item);
            }
        }
        
        public void UseItem(ItemObject item)
        {
            switch(item.ItemType)
            {
                case ItemObject.EItemType.EQUIPABLE:
                    photonView.RPC("Equip",RpcTarget.All, item.name);
                    break;
                case ItemObject.EItemType.TRAP:
                    break;
                case ItemObject.EItemType.TROWABLE:
                    break;
                case ItemObject.EItemType.USEABLE:
                    photonView.RPC( "UseItem", RpcTarget.All, item.name);
                    break;
            }
        }
        void SlowFromItem(float slowFactor)
        {

        }
        void DamageFromItem(float damage)
        {

        }

        [PunRPC]
        void Equip(string itemName)
        {
            ItemCollection collection = CollectedItem.Find(x => x.item.name == itemName);
            ItemCollection CurrentEquipedCollection = CollectedItem.Find(x => x.IsEquiped == true);

            CurrentEquipedCollection.IsEquiped = false;
            collection.IsEquiped = true;

        }

        [PunRPC]
        void UseItem(string itemName)
        {
            ItemCollection collection = CollectedItem.Find(x => x.item.name == itemName);

            _character.ChangeSpeed(collection.item.AddSpeedFactor);
            collection.UseItem(1, Recover);
        }

        void Recover(ItemObject item)
        {
            if(item.AddSpeedFactor != 0)
            {
                _character.ChangeSpeed(item.AddSpeedFactor * -1);
            }
        }
    }
}
