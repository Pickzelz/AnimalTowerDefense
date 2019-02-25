using UnityEngine;
using UnityEngine.UI;
using Multiplayer;

public class UIRoomPlayer : MonoBehaviour
{
    private string UserID;
    public Text PlayerNameObject;

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
    }
}
