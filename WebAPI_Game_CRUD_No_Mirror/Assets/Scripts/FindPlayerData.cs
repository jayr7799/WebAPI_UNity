using TMPro;
using UnityEditor;
using UnityEngine;

public class FindPlayerData : MonoBehaviour
{
    public TMP_InputField nametxt;
    public TMP_InputField id;
    public FetchData fetch;
    public Canvas canvas;
    public TMP_Text prefab;
    void Start()
    {
        
    }
    void Update()
    {
    }

    public void SearchPlayer()
    {
        if (nametxt.text != "" && id.text != "")
        {
            fetch.SetupPlayerSearchData(nametxt.text, id.text);           
        }
    }
}