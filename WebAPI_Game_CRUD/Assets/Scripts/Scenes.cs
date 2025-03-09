using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{
    public void SwitchToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void SwitchToPlayers()
    {
        SceneManager.LoadScene(1);
    }
    public void SwitchToAddPlayers()
    {
        SceneManager.LoadScene(2);
    }
    public void SwitchToUpdate()
    {
        SceneManager.LoadScene(3);
    }
    public void SwitchToSinglePlayer()
    {
        SceneManager.LoadScene(4);
    }
}
