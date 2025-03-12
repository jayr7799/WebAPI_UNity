using UnityEngine;
using Mirror;
using TMPro;
using System;
public class RoomPlayer : NetworkRoomPlayer
{
    public TMP_Text playerStatusText;
    
    public override void OnStartClient()
    {
        base.OnStartClient();
        UpdateReadyStatus();
    }
    public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
    {
        UpdateReadyStatus();
    }
    private void UpdateReadyStatus()
    {
        playerStatusText.text = "Is Ready";
    }
}
