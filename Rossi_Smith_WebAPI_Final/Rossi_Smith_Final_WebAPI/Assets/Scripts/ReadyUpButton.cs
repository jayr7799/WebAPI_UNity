using UnityEngine;
using UnityEngine.UI;
using Mirror;
public class ReadyUpButton : MonoBehaviour
{
    NetworkRoomPlayer roomPlayer;
    Button readyButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        readyButton = GetComponent<Button>();
        roomPlayer = NetworkClient.localPlayer.GetComponent<NetworkRoomPlayer>();

        readyButton.onClick.AddListener(() =>
        {
            if(roomPlayer)
            {
                roomPlayer.CmdChangeReadyState(!roomPlayer.readyToBegin);
            }
        });
    }

}
