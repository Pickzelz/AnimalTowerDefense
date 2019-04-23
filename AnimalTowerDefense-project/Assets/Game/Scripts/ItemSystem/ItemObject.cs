using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
namespace Game.Item
{
    [System.Serializable]
    [RequireComponent(typeof(Rigidbody))]
    public class ItemObject : MonoBehaviour, IUseable
    {
        public enum EItemType { TROWABLE, USEABLE, EQUIPABLE, TRAP}
        public string ItemName;
        public ItemUI UI;

        public float AddSpeedFactor;
        public float Range;
        public float time;
        public float AddHealthFactor;

        public EItemType ItemType;

        private void Start()
        {
            AddSpeedFactor = 0;
            Range = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Pick item");
            if (other.tag == "Player")
            {
                PhotonView view = PhotonView.Get(this);
                ICanUseItem item = other.GetComponent(typeof(ICanUseItem)) as ICanUseItem;
                item.TakeItem(this);
                view.RPC("RPCDestroyItem", RpcTarget.All);
            }
        }

        [PunRPC]
        void RPCDestroyItem()
        {
            Destroy(gameObject);
        }


        public void Use(ICanUseItem itemUser)
        {
            switch(ItemType)
            {
                case EItemType.EQUIPABLE:
                    break;
                case EItemType.TROWABLE:
                    break;
                case EItemType.USEABLE:
                    break;
            }
        }
    }
}
