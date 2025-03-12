using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
using TMPro;
public class GameTimer : NetworkBehaviour
{
    [SyncVar] public float timeRemaining = 60;
    [SyncVar] public float timeRemainingOG = 60;
    SpawnManager spawnManager;
    GameManager gameManager;


    public GameObject cnvs;
    public TMP_InputField newName;
    [SyncVar] string playerName;

    private void Start()
    {
        GameObject sm = GameObject.Find("NetworkManager");
        spawnManager = sm.GetComponent<SpawnManager>();

        GameObject gm = GameObject.Find("GameManager");
        gameManager = gm.GetComponent<GameManager>();

    }

    void Update()
    {

        if (!isServer) return;


        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0)
        {
            timeRemaining = timeRemainingOG;

        }
    }

    [ClientRpc]
    private void SwitchScene()
    {
        SceneManager.LoadScene(2);
    }
    [Server]
    public void EndGame()
    {
        int frozenCount = 0;

        TagPlayer[] players = FindObjectsByType<TagPlayer>(FindObjectsSortMode.None);

        foreach (var player in players)
        {
            if(player.isFrozen) frozenCount++;
        }

        foreach (var player in gameManager.players)
        {
            var playerTagComp = player.GetComponent<TagPlayer>();
            if (playerTagComp.name != "" && playerTagComp.score != 0 && playerTagComp.playerid != "")
            {
                if(frozenCount == players.Length - 1)
                {
                    Debug.Log("updated player");

                    if (playerTagComp.isIt == true)
                        gameManager.SetUpdatePlayer(playerTagComp.name, playerTagComp.playerid, playerTagComp.timesPlayed + 1, playerTagComp.score, playerTagComp.wins + 1);
                    else
                        gameManager.SetUpdatePlayer(playerTagComp.name, playerTagComp.playerid, playerTagComp.timesPlayed + 1, playerTagComp.score, playerTagComp.wins);
                }
                if (frozenCount < players.Length)
                {
                    Debug.Log("updated player");

                    if (playerTagComp.isIt == false)
                        gameManager.SetUpdatePlayer(playerTagComp.name, playerTagComp.playerid, playerTagComp.timesPlayed + 1, playerTagComp.score, playerTagComp.wins + 1);
                    else
                        gameManager.SetUpdatePlayer(playerTagComp.name, playerTagComp.playerid, playerTagComp.timesPlayed + 1, playerTagComp.score, playerTagComp.wins);
                }
            }
            else
            {
                if (frozenCount == players.Length - 1)
                {
                    Debug.Log("added player");

                    if (playerTagComp.isIt == true)
                        gameManager.SetupPlayerData("NewPlayer", playerTagComp.timesPlayed + 1, playerTagComp.score, playerTagComp.wins + 1);
                    else
                        gameManager.SetupPlayerData("NewPlayer", playerTagComp.timesPlayed + 1, playerTagComp.score, playerTagComp.wins);
                }
                if (frozenCount < players.Length)
                {
                    Debug.Log("added player");

                    if (playerTagComp.isIt == false)
                        gameManager.SetupPlayerData("NewPlayer", playerTagComp.timesPlayed + 1, playerTagComp.score, playerTagComp.wins + 1);
                    else
                        gameManager.SetupPlayerData("NewPlayer", playerTagComp.timesPlayed + 1, playerTagComp.score, playerTagComp.wins);
                }
            }
            cnvs.SetActive(false);
            SwitchScene();
        }
    }

    //[ClientRpc]
    //private void RPCShowWin(bool itWins)
    //{
    //    Debug.Log(itWins ? "It Player Wins" : "Surivors Win");
    //}
}
