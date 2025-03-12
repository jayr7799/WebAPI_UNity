using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{
    public void SwitchToMultiplayerScene()
    {
        SceneManager.LoadScene(0);
    }
    #region ORIGINAL CRUD SCENE SWITCHES
    public void SwitchToMainMenu()
        {
            SceneManager.LoadScene(1);
        }
        public void SwitchToPlayers()
        {
            SceneManager.LoadScene(2);
        }
        public void SwitchToAddPlayers()
        {
            SceneManager.LoadScene(3);
        }
        public void SwitchToUpdate()
        {
            SceneManager.LoadScene(4);
        }
        public void SwitchToSinglePlayer()
        {
            SceneManager.LoadScene(5);
        }
    #endregion



}
