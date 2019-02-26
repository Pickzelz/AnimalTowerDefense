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
        public GameObject HeroSelectionObject;
        
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
            roomPlayer.HeroSelectionPanel = HeroSelectionObject;
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
            Debug.Log("DeletePlayer ");
            PlayerListObject listObject = PlayerListObjects.Find(x => x.Status.OnlinePlayerStatus.UserId == status.OnlinePlayerStatus.UserId);
            foreach(PlayerListObject obj in PlayerListObjects)
            {
                Debug.Log("DeletePlayer ->" + obj.Status.OnlinePlayerStatus.NickName);
            }
            Destroy( listObject.ListObject);
            PlayerListObjects.Remove(listObject);
        }

        public void OpenHeroSelection()
        {
            
        }
    }    
}
