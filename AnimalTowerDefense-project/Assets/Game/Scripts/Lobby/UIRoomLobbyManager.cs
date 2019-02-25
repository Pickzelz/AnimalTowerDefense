using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Multiplayer;
using Photon.Realtime;

namespace Game.Lobby
{
    class UIRoomLobbyManager : MonoBehaviour 
    {

        struct PlayerListObject
        {
            public MultiplayerManager.StatusPlayer Status;
            public GameObject ListObject;
        }
        public Text RoomNameText;
        public GameObject PlayerStatusBoxPrefab;
        public GameObject ListPlayersinRoomContainer;
        private List<PlayerListObject> PlayerListObjects;
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            PlayerListObjects = new List<PlayerListObject>();
        }
        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            if(PlayerListObjects == null)
                PlayerListObjects = new List<PlayerListObject>();
            if(RoomNameText.text != MultiplayerManager.Instance.GetCurrentRoom().Name)
            {
                RoomNameText.text = MultiplayerManager.Instance.GetCurrentRoom().Name;
            }

            MultiplayerManager.Instance.RegisterOnPlayerInRoomChanged(OnPlayerChanged);
        }

        public void OnPlayerChanged(List<MultiplayerManager.StatusPlayer> players)
        {
            foreach(MultiplayerManager.StatusPlayer status in players)
            {
                if(status.isNew)
                {
                    AddNewPlayers(status);
                }
                else if(status.isChanged)
                {
                    EditPlayer(status);
                }
                else if(status.isLeave)
                {
                    DeletePlayer(status);
                }
            }
        }

        private void AddNewPlayers(MultiplayerManager.StatusPlayer status)
        {
            GameObject listObject = Instantiate(PlayerStatusBoxPrefab, ListPlayersinRoomContainer.transform);
            UIRoomPlayer roomPlayer = listObject.GetComponent<UIRoomPlayer>();
            roomPlayer.DrawObject(status);
            PlayerListObject obj = new PlayerListObject();
            obj.ListObject = listObject;
            obj.Status = status;
            PlayerListObjects.Add(obj);
        }

        private void EditPlayer(MultiplayerManager.StatusPlayer status)
        {
            PlayerListObject listObject = PlayerListObjects.Find(x => x.Status.OnlinePlayerStatus.UserId == status.OnlinePlayerStatus.UserId);
            listObject.Status = status;
            UIRoomPlayer roomPlayer = listObject.ListObject.GetComponent<UIRoomPlayer>();
            roomPlayer.DrawObject(status);
        }

        private void DeletePlayer(MultiplayerManager.StatusPlayer status)
        {
            PlayerListObject listObject = PlayerListObjects.Find(x => x.Status.OnlinePlayerStatus.UserId == status.OnlinePlayerStatus.UserId);
            Destroy( listObject.ListObject);
            PlayerListObjects.Remove(listObject);
        }

    }    
}
