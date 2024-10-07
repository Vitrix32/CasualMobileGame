using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    // Doesnt Work  --  Cant "restart" Game
    public void PlayAgain()
    {
        GameObject.Find("WorldPlayer").GetComponent<PlayerStatus>().ExitingCombat();
        GameObject.Find("WorldPlayer").GetComponent<PlayerStatus>().EnableControl();
        GameObject.Find("RandomEncounter").GetComponent<Encounter>().playerDead();
        SceneManager.LoadScene("Preload");
    }

    // Doesnt Work  --  Cant "restart" Game
    public void MainMenu()
    {
        GameObject.Find("RandomEncounter").GetComponent<Encounter>().playerDead();
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
