using System.Collections;
using System.Collections.Specialized;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class UpdatePlayerData : MonoBehaviour
{
    public TMP_InputField nametxt;
    public TMP_InputField id;
    public TMP_InputField firstName;
    public TMP_InputField lastName;
    public TMP_InputField timesPlayed;
    public TMP_InputField score;

    public FetchData fetch;
    PlayerData player;
    PlayerData playerData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateNewData()
    {
        if(nametxt.text != "" && id.text != "" && firstName.text != "" && lastName.text != "" && score.text != "")
        {
            int scoreData = int.Parse(score.text);
            int timesPlayedData = int.Parse(timesPlayed.text);
            fetch.SetUpdatePlayer(nametxt.text, id.text, firstName.text, lastName.text, timesPlayedData, scoreData);
        }
    }
    public void DeletePlayer()
    {
        if (nametxt.text != "" && id.text != "")
        {
            fetch.SetDeletePlayer(nametxt.text, id.text);
        }
    }
}
