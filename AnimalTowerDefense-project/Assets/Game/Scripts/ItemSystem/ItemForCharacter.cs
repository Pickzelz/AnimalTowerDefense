using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon.Pun;
using ATD;

namespace Game.Item
{
    [RequireComponent(typeof(Rigidbody))]
    [System.Serializable]
    public class ItemForCharacter : MonoBehaviourPunCallbacks, ICanEffectByItem, ICanUseItem
    {
        [HideInInspector] public UnityEvent TimerEvent;
        private Character _character;
        public Canvas ItemContainer;

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

        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        void OnDestroy()
        {
            TimerEvent.RemoveAllListeners();
        }

        public void TakeItem(ItemObject item)
        {
            if (!photonView.IsMine)
                return;

            PhotonView view = PhotonView.Get(this);
            //ItemCollection collection = AddItem(item.ItemName, 1);
            //GameEvents.Instance.ETakeItemEvent.Invoke(collection);
            view.RPC("RPCAddItem", RpcTarget.All, item.ItemName, 1);
            
        }

        [PunRPC]
        void RPCAddItem(string itemName, int amount)
        {
            ItemCollection collection = AddItem(itemName, amount);
            if(photonView.IsMine)
                GameEvents.Instance.ETakeItemEvent.Invoke(collection);
        }

        ItemCollection AddItem(string itemName, int amount)
        {
            ItemCollection collection;
            if (CollectedItem.Exists(x => x.item.ItemName == itemName))
            {
                collection = CollectedItem.Find(x => x.item.ItemName == itemName);
                collection.AddItem(amount);
                Debug.Log("Total item in collection " + collection.Quantity);
                //UseItem(collection.item);

            }
            else
            {
                Debug.Log("Add new item ");
                ItemObject objectCpy = ItemManager.Instance.Getitem(itemName);
                collection = new ItemCollection(objectCpy, 0, ref TimerEvent);
                collection.AddItem(amount);
                CollectedItem.Add(collection);
                //UseItem(collection.item);
            }
            return collection;
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

            GameEvents.Instance.EUseItemEvent.Invoke(collection);
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
