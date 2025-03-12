using System;
using TMPro;
using UnityEngine;

public class SendPlayerData : MonoBehaviour
{
    public TMP_InputField nametxt;
    public TMP_InputField firstName;
    public TMP_InputField lastName;
    public TMP_InputField timesPlayed;
    public TMP_InputField score;
    public PostData post;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SendNewPlayerData()
    {
        if (firstName.text != "" && score.text != "")//&& nametxt.text != "" && score.text != "" && lastName.text != "")
        {
            int scoreData = int.Parse(score.text);
            int timesPlayedData = int.Parse(timesPlayed.text);
            post.SetupPlayerData(nametxt.text, firstName.text, lastName.text, timesPlayedData, scoreData);
        }
    }
}
