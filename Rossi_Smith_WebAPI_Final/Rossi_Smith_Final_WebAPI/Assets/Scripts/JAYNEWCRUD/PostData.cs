using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
public class PostData : MonoBehaviour
{
    
    PlayerData player;

    void Start()
    {
        //SetupPlayerData("Luffy", 100000, 100);
    }
    void Update()
    {
        
    }

    public void SetupPlayerData(string name, string firstName, string lastName, int timesPlayed, int score)
    {
        player = new PlayerData();
        player.name = name;
        player.score = score;
        player.firstName = firstName;
        player.timesPlayed = timesPlayed;
        player.lastName = lastName;

        string json = JsonUtility.ToJson(player);
        Debug.Log(json);
        StartCoroutine(PostPlayerData(json));
    }

    IEnumerator PostPlayerData(string json)
    {
        string serverUrl = "http://localhost:3000/sentdataToDB";
        byte[] jsonToSend = Encoding.UTF8.GetBytes(json);
        UnityWebRequest request = new UnityWebRequest(serverUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            //Success
            Debug.Log($"Data Sent: {request.downloadHandler.text}");

            string newPlayerId = ExtractPlayerId(response);
            Debug.Log("New player id:" + newPlayerId);
        }
        else
        {
            //failed
            Debug.LogError($"Error sending data: {request.error}");
        }
    }

    string ExtractPlayerId(string jsonResponse)
    {
        int index = jsonResponse.IndexOf("\"playerid\":\"") + 12;
        if (index < 12) return "";
        int endIndex = jsonResponse.IndexOf("\"", index);
        return jsonResponse.Substring(index, endIndex - index);

    }
}
