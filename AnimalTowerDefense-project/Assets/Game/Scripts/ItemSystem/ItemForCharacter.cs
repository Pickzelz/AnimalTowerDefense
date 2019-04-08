using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Game.Item
{
    [RequireComponent(typeof(Rigidbody))]
    [System.Serializable]
    public class ItemForCharacter : MonoBehaviourPunCallbacks, ICanEffectByItem, ICanUseItem
    {
        [System.Serializable]
        public class ItemCollection
        {
            public ItemObject item { get; private set; }
            public int Quantity { get; private set; }

            public ItemCollection(ItemObject item, int Quantity)
            {
                this.item = item;
                this.Quantity = Quantity;
            }

            public void AddItem(int amount)
            {
                Quantity+=amount;
                Debug.Log("ItemCollection : add item amount -> " + Quantity);
            }
        }
        public List<ItemCollection> CollectedItem;
        // Start is called before the first frame update
        void Start()
        {
            CollectedItem = new List<ItemCollection>();
        }

        // Update is called once per frame
        void Update()
        {

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
            }
            else
            {
                Debug.Log("Add new item ");
                ItemObject objectCpy = ItemManager.Instance.Getitem(itemName);
                ItemCollection collection = new ItemCollection(objectCpy, 1);

                CollectedItem.Add(collection);
            }
        }
        
        public void UseItem()
        {

        }
        public void SlowFromItem(float slowFactor)
        {

        }
        public void DamageFromItem(float damage)
        {

        }
    }
}
