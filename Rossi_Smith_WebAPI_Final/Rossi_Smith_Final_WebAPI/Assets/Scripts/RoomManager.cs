using UnityEngine;
using Mirror;
using System.Collections.Generic;
public class RoomManager : NetworkRoomManager
{
    List<Transform> availableSpawnPoints;
    public override void OnRoomServerSceneChanged(string sceneName)
    {
        if(sceneName == GameplayScene)
        {
            GameObject spawnManager = GameObject.Find("SpawnPoints");
            if(spawnManager != null)
            {
                availableSpawnPoints = new List<Transform>();
                foreach(Transform point in spawnManager.transform)
                {
                    availableSpawnPoints.Add(point);
                }
            }
        }
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Transform spawnPoint = GetRandomSpawnPoint();
        GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        NetworkServer.ReplacePlayerForConnection(conn, player, ReplacePlayerOptions.KeepAuthority);
    }

    Transform GetRandomSpawnPoint()
    {
        if (availableSpawnPoints.Count == 0 || availableSpawnPoints == null) return transform;

        int index = (int)Mathf.Floor(Random.Range(0, availableSpawnPoints.Count));
        Transform selectedSpawnPoint = availableSpawnPoints[index];
        //if want to remove spawn point - availableSpawnPoints.RemoveAt(index);
        return selectedSpawnPoint;
    }
}
