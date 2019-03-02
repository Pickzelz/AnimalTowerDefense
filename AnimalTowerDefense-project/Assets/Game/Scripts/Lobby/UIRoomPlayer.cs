using UnityEngine;
using UnityEngine.UI;
using Multiplayer;
using Heroes = ATD.GameManager.AnimalHero;

namespace Game.Lobby
{
    public class UIRoomPlayer : MonoBehaviour
    {
        private string UserID = null;
        private Heroes? _hero = null;
        public Text PlayerNameObject;
        [HideInInspector] public GameObject HeroSelectionPanel;
        public Button ChangeHeroButton;
        public Text HeroName;
         /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        
        public void DrawObject(MultiplayerManager.StatusPlayer status)
        {
            if(PlayerNameObject.text != status.OnlinePlayerStatus.NickName)
            {
                PlayerNameObject.text = status.OnlinePlayerStatus.NickName;
            }
            if(UserID != status.OnlinePlayerStatus.UserId)
            {
                UserID = status.OnlinePlayerStatus.UserId;
            }
            if(status.HeroSelectedbyPlayer != null)
            {
                if(_hero == null)
                {
                    _hero = status.HeroSelectedbyPlayer;
                    string names = ((Heroes)_hero).Name;
                    HeroName.text = names;
                }
                else if(((Heroes)_hero).Name != ((Heroes)status.HeroSelectedbyPlayer).Name)
                {
                    _hero = status.HeroSelectedbyPlayer;
                    string names = ((Heroes)_hero).Name;
                    HeroName.text = names;
                }
            }
            
        }

        public void GotoHeroSelection()
        {
            if(UserID == MultiplayerManager.Instance.GetPlayerID())
            {
                HeroSelectionPanel.SetActive(true);
            }
        }
    }
    
}
