using UnityEngine;
using Photon.Pun;

namespace Multiplayer
{
    public class MultiplayerPlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        /// 

        Component[] controllers;
        void Start()
        {
            controllers =  gameObject.GetComponents(typeof(IMultiplayerPlayerObject));
            if(!photonView.IsMine && PhotonNetwork.IsConnected)
            {
                foreach(IMultiplayerPlayerObject obj in controllers)
                {
                    obj.WhenNotMine();
                }
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            foreach (IMultiplayerPlayerObject obj in controllers)
            {
                obj.SyncVariable(stream, info);
            }
        }
    }

}
