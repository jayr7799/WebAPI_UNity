using UnityEngine;
using UnityEngine.UI;
using Mirror;
public class StartGameButton : MonoBehaviour
{
    NetworkRoomManager roomManager;
    Button startButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startButton = GetComponent<Button>();
        roomManager = FindAnyObjectByType<NetworkRoomManager>();

        startButton.onClick.AddListener(() =>
        {
            if(roomManager)
            {
                roomManager.CheckReadyToBegin();
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        startButton.interactable = NetworkServer.active && roomManager.allPlayersReady;
    }
}
