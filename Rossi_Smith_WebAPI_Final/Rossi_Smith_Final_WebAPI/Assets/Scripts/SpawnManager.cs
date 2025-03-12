using UnityEngine;
using Mirror;

using System.Collections.Generic;
public class SpawnManager : NetworkManager
{
    public Transform[] spawnPoints;
    GameManager gm;



    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        GameObject gameManager = GameObject.Find("GameManager");
        Transform spawnPoint = GetRandomSpawnPoint();
        GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        NetworkServer.AddPlayerForConnection(conn, player);

        gm = gameManager.GetComponent<GameManager>();
        gm.players.Add(player);
        gm.index = gm.players.IndexOf(player);
        var playerTagComp = player.GetComponent<TagPlayer>();

        if (gm.index <= gm.allPlayers.Count - 1)
        {
            Debug.Log("existing player");

            playerTagComp.playerName = gm.allPlayers[gm.index].name;
            playerTagComp.score = gm.allPlayers[gm.index].score;
            playerTagComp.timesPlayed = gm.allPlayers[gm.index].timesPlayed;
            playerTagComp.playerid = gm.allPlayers[gm.index].playerid;
            playerTagComp.wins = gm.allPlayers[gm.index].wins;
        }
        else
        {
            Debug.Log("NewPlayer");
            playerTagComp.playerName = "";
            playerTagComp.score = 0;
            playerTagComp.timesPlayed = 0;
            playerTagComp.playerid = "";
            playerTagComp.wins = 0;
        }
        
    }

    Transform GetRandomSpawnPoint()
    {
        if (spawnPoints.Length == 0) return transform;
        return spawnPoints[(int)Mathf.Floor(Random.Range(0, spawnPoints.Length - 1))];
    }
}
