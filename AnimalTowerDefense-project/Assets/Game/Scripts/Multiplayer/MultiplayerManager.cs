using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using Game.Lobby;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Heroes = GameManager.AnimalHero;

namespace Multiplayer
{
#region delegate
    public delegate void PlayerChangedCallback(int data);
    public delegate void PlayerLeaveRoomCallback();
    public delegate void ListRoomChangeCallback(List<RoomInfo> info);
    public delegate void PlayerJoinedRoomCallback();
    public delegate void UpdatePlayersInRoomCallback(List<MultiplayerManager.StatusPlayer> players);
#endregion
    public class MultiplayerManager : MonoBehaviourPunCallbacks
    {
#region temp sturct for player
        public class StatusPlayer
        {
            public Player OnlinePlayerStatus;
            public Heroes? HeroSelectedbyPlayer;

            public bool isLeave;

            public bool isNew;

            public bool isChanged;
            public StatusPlayer()
            {
                HeroSelectedbyPlayer = null;
                isLeave = false;
                isChanged = false;
                isNew = true;
            }
        }

#endregion
#region singleton
        public static MultiplayerManager Instance = null;
#endregion
#region enums
        public enum Stat {NONE, DISCONNECTED, CONNECTING, CONNECTED, JOINING_LOBBY, JOINED_LOBBY, JOINING_ROOM, JOINED_ROOM}
#endregion
#region private variable
        string _gameVersion = "1";
        Stat _prevStat;
        int _totalPlayerInRoom;
        
        List<StatusPlayer> Players;
        
        [SerializeField] private byte maxPlayerInRoom = 4;

#endregion
#region constant variable
        public const string PLAYER_HERO_SELECTED = "HeroSelectedByPlayer"; //key for hero selected by player
#endregion

#region event
        event PlayerChangedCallback _PlayerChangeCallback;
        event PlayerLeaveRoomCallback _OnLeaveRoom;
        event ListRoomChangeCallback _OnListRoomChange;
        event PlayerJoinedRoomCallback _OnPlayerJoinedRoom;
        event UpdatePlayersInRoomCallback _UpdatePlayerInRoom;
#endregion

#region public variable
        public Stat Status;
        public RoomInfo currentRoom;
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
            _OnPlayerJoinedRoom = null;
            _UpdatePlayerInRoom = null;

            _totalPlayerInRoom = 0;
            Players = new List<StatusPlayer>();
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
            _PlayerChangeCallback += callback;
        }

        public void CleanOnPlayerChangeCallback()
        {
            _PlayerChangeCallback = null;
        }

        public void RegisterOnPlayerLeaveRoomCallback(PlayerLeaveRoomCallback callback)
        {
            _OnLeaveRoom += callback;
        }

        public void CleanOnPlayerLeaveRoomCallback()
        {
            _OnLeaveRoom = null;
        }

        public void RegisterOnListRoomChange(ListRoomChangeCallback callback)
        {
            Debug.Log("RegisterOnListRoomChange");
            _OnListRoomChange += callback;
        }

        public void CleanOnListRoomChange()
        {
            _OnListRoomChange = null;
        }

        public void RegisterOnPlayerJoinedRoom(PlayerJoinedRoomCallback callback)
        {
            _OnPlayerJoinedRoom += callback;
        }

        public void CleanOnPlayerJoinedCallback()
        {
            _OnPlayerJoinedRoom = null;
        }

        public void RegisterOnPlayerInRoomChanged(UpdatePlayersInRoomCallback callback)
        {
            _UpdatePlayerInRoom += callback;
            if(Players.Count > 0)
                Players.Clear();
            foreach(Player player in PhotonNetwork.PlayerList)
            {
                AddNewPlayerOnList(player);
            }
            if(_UpdatePlayerInRoom != null)
                _UpdatePlayerInRoom(Players);

            foreach(StatusPlayer player in Players)
            {
                player.isNew = false;
            }   
            
        }

        public void CleanOnTotalPlayerinRoomChange()
        {
            _UpdatePlayerInRoom = null;
            Players.Clear();
        }

#endregion
#region other function

        private void AddNewPlayerOnList(Player player)
        {
            StatusPlayer status = new StatusPlayer();
            status.OnlinePlayerStatus = player;
            status.HeroSelectedbyPlayer = null;
            Players.Add(status);
        }
        private void EditPlayerOnList(Player player)
        {
            StatusPlayer status = Players.Find(x => x.OnlinePlayerStatus.UserId == player.UserId);
            status.isChanged = true;
            status.OnlinePlayerStatus = player;
        }
        private void DeletePlayerOnList(Player player)
        {
            StatusPlayer status = Players.Find(x => x.OnlinePlayerStatus.UserId == player.UserId);
            status.OnlinePlayerStatus = player;
            status.isLeave = true;
            Players.Remove(status);
        }
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

        public string GetPlayerName()
        {
            return PhotonNetwork.NickName;
        }

        public string GetPlayerID()
        {
            return PhotonNetwork.LocalPlayer.UserId;
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


        public void JoinLobby()
        {
            Status = Stat.JOINING_LOBBY;
            PhotonNetwork.JoinLobby(null);
        }

        public void JoinRoom(string name)
        {
            Status = Stat.JOINING_LOBBY;
            PhotonNetwork.JoinRoom(name);
        }

        public void GetRooms()
        {
        }

        public Room GetCurrentRoom()
        {
            return PhotonNetwork.CurrentRoom;
        }

        public void SendPlayerInformation(string key, string value)
        {
            // Hashtable hash = new Hashtable();
            // hash.Add(PLAYER_HERO_SELECTED, value);

            // PhotonNetwork.SetPlayerCustomProperties(hash);
        }

        public void PlayerSelectHero(Heroes hero)
        {
            Hashtable hash = new Hashtable();
            hash.Add(PLAYER_HERO_SELECTED, hero.Name);

            PhotonNetwork.SetPlayerCustomProperties(hash);
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
        public override void OnJoinedRoom()
        {
            Debug.Log("client joined some room");
            Status = Stat.JOINED_ROOM;
            if(_OnPlayerJoinedRoom != null)
                _OnPlayerJoinedRoom();
        }

        public override void OnLeftRoom()
        {
            if(_OnLeaveRoom != null)
                _OnLeaveRoom();
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("Room list updated");
            if(_OnListRoomChange != null)
                _OnListRoomChange(roomList);
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log("Create room failed");
        }
        public override void OnJoinedLobby()
        {
            Debug.Log("Joined to lobby");
            Status = Stat.JOINED_LOBBY;
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log("Player " + newPlayer.UserId + " enter room");
            StatusPlayer pl = new StatusPlayer();
            pl.OnlinePlayerStatus = newPlayer;

            Players.Add(pl);
            if(_UpdatePlayerInRoom != null)
                _UpdatePlayerInRoom(Players);
            pl.isNew = false;

        }

        public override void OnPlayerLeftRoom(Player OtherPlayer)
        {
            Debug.Log("Player " + OtherPlayer.UserId + " Leave room");
            if(Players.Exists(x => x.OnlinePlayerStatus.UserId == OtherPlayer.UserId))
            {
                StatusPlayer pl = Players.Find(x => x.OnlinePlayerStatus.UserId == OtherPlayer.UserId);
                pl.isLeave = true;
                if(_UpdatePlayerInRoom != null)
                    _UpdatePlayerInRoom(Players);
                Players.Remove(pl);
            }
        }

        public override void OnPlayerPropertiesUpdate(Player target, Hashtable changedProps)
        {
            Debug.Log("Change properties");
            if(Players.Exists(x => x.OnlinePlayerStatus.UserId == target.UserId) && GameManager.Instance.Heroes.Exists(x => x.Name == changedProps[PLAYER_HERO_SELECTED].ToString()))
            {
                StatusPlayer player = Players.Find( x => x.OnlinePlayerStatus.UserId == target.UserId);
                player.OnlinePlayerStatus = target;
                
                player.HeroSelectedbyPlayer = GameManager.Instance.Heroes.Find(x => x.Name == changedProps[PLAYER_HERO_SELECTED].ToString());
                player.isChanged = true;
                if(_UpdatePlayerInRoom != null)
                    _UpdatePlayerInRoom(Players);
                player.isChanged = false;
            }
        }
#endregion
    }
}
