using UnityEngine;
using Multiplayer;

namespace Game.Lobby
{
    public class UILoadingLobby : MonoBehaviour
    {
        // Start is called before the first frame update
        public GameObject UILobbyObject;
        void Start()
        {
            MultiplayerManager.Instance.Connect();
        }

        // Update is called once per frame
        void Update()
        {
            if(this.gameObject.activeSelf && MultiplayerManager.Instance.Status == Multiplayer.MultiplayerManager.Stat.CONNECTED)
            {
                UILobbyObject.SetActive(true);
                this.gameObject.SetActive(false);
            }
        }
    }
}

