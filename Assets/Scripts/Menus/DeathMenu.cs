using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    // Doesnt Work  --  Cant "restart" Game
    public void PlayAgain()
    {
        SceneManager.LoadScene("Preload");
    }

    // Doesnt Work  --  Cant "restart" Game
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
