using System.Collections.Generic;
using UnityEngine;
using Multiplayer;
using UnityEngine.UI;

namespace Game.Lobby
{
    public class UILobbyManager : MonoBehaviour
    {
        public static string PlayerNamePrefKey = "PlayerName";

        public Text NameText;
        public InputField NameInputField;
        public Button EditButton;
        public Button SaveButton;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            if(PlayerPrefs.HasKey(PlayerNamePrefKey))
            {
                NameText.text = PlayerPrefs.GetString(PlayerNamePrefKey);
                MultiplayerManager.Instance.SetPlayerName(PlayerPrefs.GetString(PlayerNamePrefKey));
            }
            else
            {
                if(MultiplayerManager.Instance.GetPlayerName() != "")
                {
                    PlayerPrefs.SetString(PlayerNamePrefKey, MultiplayerManager.Instance.GetPlayerName());
                    NameText.text = MultiplayerManager.Instance.GetPlayerName();
                }
            }
        }
       
        public void ChangeName()
        {
            string name = NameInputField.text;
            PlayerPrefs.SetString(PlayerNamePrefKey, name);
            MultiplayerManager.Instance.SetPlayerName(name);
            NameText.gameObject.SetActive(true);
            EditButton.gameObject.SetActive(true);
            NameInputField.gameObject.SetActive(false);
            SaveButton.gameObject.SetActive(false);

            NameText.text = PlayerPrefs.GetString(PlayerNamePrefKey);
        } 

        public void OnEditButtonClicked()
        {
            if(PlayerPrefs.HasKey(PlayerNamePrefKey))
            {
                NameInputField.text = PlayerPrefs.GetString(PlayerNamePrefKey);
               
            }
            NameInputField.gameObject.SetActive(true);
            SaveButton.gameObject.SetActive(true);
            NameText.gameObject.SetActive(false);
            EditButton.gameObject.SetActive(false);
        }
    }

}
