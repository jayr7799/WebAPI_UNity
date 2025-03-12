using UnityEngine;
using Mirror;

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using TMPro;

public class GameManager : NetworkBehaviour
{
    [SyncVar] public GameObject itPlayer;
    [SyncVar] public int index;
    [SyncVar] public List<GameObject> players;

    private void Start()
    {
        StartFetch(); //loads the database players into list: allPlayers
    }
    public override void OnStartClient()
    {
        if (NetworkServer.connections.Count == 1) //makes first play joined it
        {
            itPlayer = NetworkServer.connections[0].identity.gameObject;
            itPlayer.GetComponent<TagPlayer>().isIt = true;
        }
    }

    string serverUrl = "http://localhost:3000/player";
    PlayerData playerData;
    public PlayerData soloPlayer;
    public List<PlayerData> allPlayers = new List<PlayerData>();

    public void StartFetch()
    {
        StartCoroutine(GetAllData());
    }
    public IEnumerator GetAllData()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(serverUrl))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                //success
                string jsonRequest = request.downloadHandler.text;
                Debug.Log($"Received the Data:{jsonRequest}");

                //deserialize data using newtonsoft.json
                allPlayers = JsonConvert.DeserializeObject<List<PlayerData>>(jsonRequest);

                ////print player info to console
                foreach (var player in allPlayers)
                {
                    Debug.Log($"Player userName: {player.name}, First Name is: {player.firstName}, Last Name is: {player.lastName}, Score: {player.score}, Joined on: {player.joined}");
                }
            }
            else
            {
                //failed
                Debug.Log($"Error Fetching Data: {request.error}");
            }
        }
    }

    public Canvas canvas;
    public TMP_Text prefab;
    public void GetPlayer()
    {
        int fontSize = 20;
        float x = 450;
        float y = 400;
        Vector3 position = new Vector3(x, y, 0);

        foreach (var player in allPlayers)
        {
            TMP_Text text = Instantiate(prefab, canvas.transform);
            text.text = "Player UserName: " + player.name + " has a score of " + player.score + $". Users Full name is {player.firstName} {player.lastName} who joined on {player.joined}";
            text.fontSize = fontSize;
            RectTransform nameTransform = text.GetComponent<RectTransform>();
            nameTransform.anchoredPosition = position;
            position.y -= 50;
        }
    }





    public IEnumerator GetDataByID(string json, string playerid = "")
    {
        string url = serverUrl + "/" + playerid;
        byte[] jsonToSend = Encoding.UTF8.GetBytes(json);
        UnityWebRequest request = new UnityWebRequest(url, "GET");
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            Debug.Log($"Success:{response}");
            soloPlayer = JsonConvert.DeserializeObject<PlayerData>(response);
            Debug.Log(soloPlayer.playerid);

            //extract ID
            string newPlayerID = ExtractPlayerID(response);
            if (!string.IsNullOrEmpty(newPlayerID))
            {
                Debug.Log($"PlayerID:{newPlayerID}");
            }


            int fontSize = 20;
            float x = 450;
            float y = 400;
            Vector3 position = new Vector3(x, y, 0);

            TMP_Text text = Instantiate(prefab, canvas.transform);
            text.text = "Player UserName: " + soloPlayer.name + " has a score of " + soloPlayer.score + $". Users Full name is {soloPlayer.firstName} {soloPlayer.lastName} who joined on {soloPlayer.joined}";
            text.fontSize = fontSize;
            RectTransform nameTransform = text.GetComponent<RectTransform>();
            nameTransform.anchoredPosition = position;

            yield return null;
        }
        else
        {
            //handles failed request
            Debug.Log("Error:" + request.error);
            yield return null;

        }
    }
    public void SetupPlayerSearchData(string name, string playerid)
    {
        playerData = new PlayerData();
        playerData.name = name;

        string json = JsonUtility.ToJson(playerData);
        Debug.Log(json);
        StartCoroutine(GetDataByID(json, playerid));
    }
    string ExtractPlayerID(string jsonResponse)
    {
        int index = jsonResponse.IndexOf("\"playerid\":\"") + 12;
        if (index < 12) return "";
        int endIndex = jsonResponse.IndexOf("\"", index);
        return jsonResponse.Substring(index, endIndex - index);
    }




    internal void SetUpdatePlayer(string name, string id, int timesPlayed, int scoreData, int wins, string firstname = null, string lastname = null)
    {
        var player = new PlayerData();
        player.name = name;
        player.playerid = id;
        if(firstname != null)
            player.firstName = firstname;
        if(lastname != null)
            player.lastName = lastname;
        player.timesPlayed = timesPlayed;
        player.score = scoreData;
        player.wins = wins;
        string json = JsonUtility.ToJson(player);
        StartCoroutine(UpdatePlayer(json));
    }
    IEnumerator UpdatePlayer(string json)
    {
        string url = "http://localhost:3000/updatePlayer";
        byte[] jsonToSend = Encoding.UTF8.GetBytes(json);
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            //Success
            Debug.Log($"Data Sent: {request.downloadHandler.text}");
        }
        else
        {
            //failed
            Debug.LogError($"Error sending data: {request.error}");
        }

    }




    public void SetDeletePlayer(string uName, string id = "")
    {
        var player = new PlayerData { name = uName, playerid = id };
        string json = JsonUtility.ToJson(player);
        StartCoroutine(DeletePlayer(json, id));
    }
    public IEnumerator DeletePlayer(string json, string id = "")
    {
        string url = $"http://localhost:3000/deletePlayerUnity?playerid={id}";

        byte[] jsonToSend = Encoding.UTF8.GetBytes(json);
        UnityWebRequest request = new UnityWebRequest();
        request = UnityWebRequest.Delete(url);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            //Success
            Debug.Log($"Data Sent: {request.downloadHandler.text}");
        }
        else
        {
            //failed
            Debug.LogError($"Error sending data: {request.error}");
        }
    }





    PlayerData player;
    public void SetupPlayerData(string name, int timesPlayed, int score, int wins, string firstName = null, string lastName = null)
    {
        player = new PlayerData();
        player.name = name;
        player.timesPlayed = timesPlayed;
        player.score = score;
        player.wins = wins;
        if(firstName != null)
            player.firstName = firstName;
        if(lastName != null)
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

            string newPlayerId = ExtractPlayerID(response);
            Debug.Log("New player id:" + newPlayerId);
        }
        else
        {
            //failed
            Debug.LogError($"Error sending data: {request.error}");
        }
    }
}
