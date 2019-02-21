using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

namespace Game.Lobby
{
    public class UIListRoom : MonoBehaviour
    {
        public Text RoomNameText;
        public Text TotalPlayerJoinedText;
        public Button JoinRoomButton;
        // Start is called before the first frame update

        private int _totalPlayerJoined = 0;

        public void DrawListRoom(RoomInfo info)
        {
            if(info.Name != RoomNameText.text)
                RoomNameText.text = info.Name;
            if(info.PlayerCount != _totalPlayerJoined)
            {
                TotalPlayerJoinedText.text = info.PlayerCount.ToString() + "/" + info.MaxPlayers.ToString();
                _totalPlayerJoined = info.PlayerCount;
            }
        }
    }
}

