using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    // load Game scene
    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
    }

    // load Main Menu scene
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

