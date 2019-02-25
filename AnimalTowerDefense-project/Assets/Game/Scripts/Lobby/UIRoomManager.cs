using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Multiplayer;

namespace Game.Lobby
{
    public class UIRoomManager : MonoBehaviour
    {
        struct ListRoomStruct
        {
            public GameObject ObjectListRoom;
            public RoomInfo RoomOptions;
        }
        public InputField RoomNameField;
        public Button CreateRoomButton;
        public GameObject ListRoomObjectPrefab;
        public GameObject ListRoomsContainer;
        public GameObject RoomLobby;
        public GameObject LobbyContainer;

        public float ListHeight = 30;
        // Start is called before the first frame update

        private List<ListRoomStruct> ListRooms;
        void Start()
        {
            ListRooms = new List<ListRoomStruct>();

            MultiplayerManager.Instance.RegisterOnListRoomChange(RoomChangeCallback);
            MultiplayerManager.Instance.RegisterOnPlayerJoinedRoom(OnPlayerJoinedSomeRoom);
            MultiplayerManager.Instance.JoinLobby();
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        
        public void RoomChangeCallback(List<RoomInfo> rooms)
        {
            foreach(RoomInfo info in rooms)
            {
                //if room is deleted
                if(ListRooms.Count > 0)
                {
                    if(info.RemovedFromList && ListRooms.Exists(x => x.RoomOptions.Name == info.Name))
                    {
                        DeleteListRooms(info);
                    }
                    //if room property has changed
                    else if(ListRooms.Exists(x => x.RoomOptions.Name == info.Name))
                    {
                        ChangeListRooms(info);
                    }
                    else if(!info.RemovedFromList)
                    {
                        addNewListRooms(info);
                    }
                }
                else if(!info.RemovedFromList)
                {
                    addNewListRooms(info);
                }
                
            }

            ResizeListRoomBody();
        }

        public void CreateNewRoom()
        {
            string roomName = RoomNameField.text;
            MultiplayerManager.Instance.CreateRoom(roomName);
        }

        public void OnPlayerJoinedSomeRoom()
        {
            RoomLobby.SetActive(true);
            LobbyContainer.SetActive(false);
        }

        private void addNewListRooms(RoomInfo info)
        {
            Debug.Log("Room added in list");
            GameObject room = Instantiate(ListRoomObjectPrefab, ListRoomsContainer.transform);
            UIListRoom uiRoom = room.GetComponent<UIListRoom>();
            uiRoom.DrawListRoom(info);
            ListRoomStruct lroom = new ListRoomStruct();
            lroom.ObjectListRoom = room;
            lroom.RoomOptions = info;
            ListRooms.Add(lroom);
        }

        private void DeleteListRooms(RoomInfo info)
        {
            Debug.Log("Room deleted in list");
            ListRoomStruct objectToDelete = ListRooms.Find(x => x.RoomOptions.Name == info.Name);
            Destroy(objectToDelete.ObjectListRoom);
            ListRooms.Remove(objectToDelete);
        }

        private void ChangeListRooms(RoomInfo info)
        {
            Debug.Log("List room updated");
            GameObject room = ListRooms.Find(x => x.RoomOptions.Name == info.Name).ObjectListRoom;
            UIListRoom uiRoom = room.GetComponent<UIListRoom>();
            uiRoom.DrawListRoom(info);
        }

        private void ResizeListRoomBody()
        {
            RectTransform bodyTransform = ListRoomsContainer.GetComponent<RectTransform>();
            //resize 
            Vector2 newSize = bodyTransform.sizeDelta;
            newSize.y = ListRooms.Count * ListHeight;
            bodyTransform.sizeDelta = newSize;

            //change position
            // Vector3 newPosition = bodyTransform.localPosition;
            // newPosition.y = 0.5f * bodyTransform.sizeDelta.y;
            // bodyTransform.localPosition = newPosition;
        }
    }

}
