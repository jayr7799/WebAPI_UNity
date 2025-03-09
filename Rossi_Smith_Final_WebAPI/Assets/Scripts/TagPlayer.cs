using UnityEngine;
using Mirror;
using TMPro;
public class TagPlayer : NetworkBehaviour
{
    [SyncVar] public bool isIt = false;
    [SyncVar] public bool isFrozen = false;
    [SyncVar] public string scoreText;
    public TMP_Text scoreTextBox;

    //player data vars
    [SyncVar] public string name = "";
    [SyncVar] public int score = 0;
    [SyncVar] public int timesPlayed = 0;
    [SyncVar] public string playerid = "";
    [SyncVar] public int wins = 0;

    private void Start()
    {

        
    }

    private void Update()
    {
        scoreText = score.ToString();
        scoreTextBox.text = scoreText;  
    }
    [Server]
    private void OnTriggerEnter(Collider other)
    {
        if (!isServer) return; //makes so only server handles tagging
        TagPlayer otherPlayer = other.gameObject.GetComponent<TagPlayer>();
        if(otherPlayer != null)
        {
            if(isIt && !otherPlayer.isFrozen)
            {
                otherPlayer.FreezePlayer();
                score += 50;
                
            }
            else if(!isIt && !isFrozen && otherPlayer.isFrozen)
            {
                otherPlayer.unFreezePlayer();
                score += 25;
            }
        }
    }
    
    [Server]
    public void FreezePlayer()
    {
        isFrozen = true;
        RPCUpdateState(isFrozen);
    }
    [Server]
    public void unFreezePlayer()
    {
        isFrozen = false;
        RPCUpdateState(isFrozen);
    }
    [ClientRpc]
    void RPCUpdateState(bool frozen)
    {
        isFrozen = frozen;
        GetComponent<PlayerController>().isFrozen = frozen;
        GetComponent<Renderer>().material.color = frozen ? Color.blue : Color.gray;     
    }
    
}
