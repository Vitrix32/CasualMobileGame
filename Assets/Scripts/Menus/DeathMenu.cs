using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject WorldPlayer;
    // Doesnt Work  --  Cant "restart" Game
    // Going back to preload will cause problems! Will comment out for the time being
    private void Start()
    {
        WorldPlayer = GameObject.Find("WorldPlayer");
    }

    public void PlayAgain()
    {
        /*
        WorldPlayer.GetComponent<UniversalAudioHandling>().ButtonPressed();
        GameObject.Find("WorldPlayer").GetComponent<PlayerStatus>().EnteringGameWorld(true, 0.0f);
        GameObject.Find("RandomEncounter").GetComponent<Encounter>().playerDead();
        //SceneManager.LoadScene("Preload"); */
    }

    // Doesnt Work  --  Cant "restart" Game
    public void MainMenu()
    {
        /*
        WorldPlayer.GetComponent<UniversalAudioHandling>().ButtonPressed();
        Invoke("Menu", 0.4f); */
    }

    public void QuitGame()
    {
        WorldPlayer.GetComponent<UniversalAudioHandling>().ButtonPressed();
        Invoke("Quit", 0.4f);
    }

    private void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void Quit()
    {
        Application.Quit();
    }

}
