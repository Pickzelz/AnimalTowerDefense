using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

namespace Multiplayer
{
#region delegate
    public delegate void PlayerChangedCallback(int data);
    public delegate void PlayerLeaveRoomCallback();
    public delegate void ListRoomChangeCallback(List<RoomInfo> info);
#endregion
    public class MultiplayerManager : MonoBehaviourPunCallbacks
    {
#region singleton
        public static MultiplayerManager Instance = null;
#endregion
#region enums
        public enum Stat {NONE, DISCONNECTED, CONNECTING, CONNECTED, JOINING_LOBBY, JOINED_LOBBY, JOINING_ROOM, JOINED_ROOM}
        public enum Callbacks{PLAYERCHANGE}
#endregion
#region private variable
        string _gameVersion = "1";
        Stat _prevStat;
        
        [SerializeField] private byte maxPlayerInRoom = 4;

#endregion

#region event
        event PlayerChangedCallback _PlayerChangeCallback;
        event PlayerLeaveRoomCallback _OnLeaveRoom;
        event ListRoomChangeCallback _OnListRoomChange;
#endregion

#region public variable
        public Stat Status;
#endregion


#region Unity life cycle
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>

        void Awake()
        {
#region singleton
            Instance = this;
#endregion
            PhotonNetwork.AutomaticallySyncScene = true;
            _PlayerChangeCallback = null;
            _OnLeaveRoom = null;
            _OnListRoomChange = null;

        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            // StatusUpdate();
        }
        private void Start()
        {
            Status = Stat.NONE;
        }
#endregion
#region callback function
        
        public void RegisterOnPlayerChangeCallback(PlayerChangedCallback callback)
        {
            if(_PlayerChangeCallback == null)
                _PlayerChangeCallback += callback;
        }

        public void RegisterOnPlayerLeaveRoomCallback(PlayerLeaveRoomCallback callback)
        {
            if(_OnLeaveRoom == null)
                _OnLeaveRoom += callback;
        }

        public void RegisterOnListRoomChange(ListRoomChangeCallback callback)
        {
            if(_OnListRoomChange == null)
                _OnListRoomChange += callback;
        }
#endregion
#region other function
        public void Connect()
        {
            if(!PhotonNetwork.IsConnected && Status == Stat.NONE)
            {
                Status = Stat.CONNECTING;
                PhotonNetwork.GameVersion = _gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public void SetPlayerName(string name)
        {
            PhotonNetwork.NickName = name;
        }

        // void StatusUpdate()
        // {
        //     if(_prevStat == Status)
        //     {
        //         return;
        //     }
        //     switch(Status)
        //     {
        //         case Stat.NONE:
        //             Connect();
        //         break;
        //         case Stat.CONNECTED:
        //             JoinLobby();
        //         break;
        //         case Stat.DISCONNECTED:
        //         break;
        //     }
        //     _prevStat = Status;
        // }

        public void CreateRoom(string roomName)
        {
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = maxPlayerInRoom;
            options.IsVisible = true;
            PhotonNetwork.CreateRoom(roomName, options);
        }

        public void DeleteRoom(string name)
        {
            
        }

        public void JoinLobby()
        {
            Status = Stat.JOINING_LOBBY;
            PhotonNetwork.JoinLobby(null);
        }

        public void GetRooms()
        {
        }
#endregion        

#region Photon callback
        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Tutorial/Launcer: OnConnectedtoMaster() was called");
            Status = Stat.CONNECTED;
            //PhotonNetwork.JoinRandomRoom();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Status = Stat.DISCONNECTED;
            Debug.LogWarningFormat("PUN OnDisconnected was called");
        }
        
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            
        }
// Room
        public override void OnJoinedRoom()
        {
            Debug.Log("client joined some room");
            Status = Stat.JOINED_ROOM;
        }

        public override void OnLeftRoom()
        {
            _OnLeaveRoom();
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("Room list updated");
            _OnListRoomChange(roomList);
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log("Create room failed");
        }
//Room
        public override void OnJoinedLobby()
        {
            Debug.Log("Joined to lobby");
            Status = Stat.JOINED_LOBBY;
        }
#endregion
    }

    class Players
    {
        string Name;

    }

}
