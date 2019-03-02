using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Multiplayer
{
    public class MultiplayerPlayerManager : MonoBehaviourPunCallbacks
    {
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            Component[] controllers =  gameObject.GetComponents(typeof(IMultiplayerPlayerObject));
            if(!photonView.IsMine && PhotonNetwork.IsConnected)
            {
                foreach(IMultiplayerPlayerObject obj in controllers)
                {
                    obj.WhenNotMine();
                }
            }
        }
    }

}
