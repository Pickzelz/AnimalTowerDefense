using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
namespace Game.Item
{
    [System.Serializable]
    [RequireComponent(typeof(Rigidbody))]
    public class ItemObject : MonoBehaviour
    {
        public enum EItemType { TROWABLE, USEABLE, EQUIPABLE }
        public string ItemName;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

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

    }
}
