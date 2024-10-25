using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    [SerializeField]
    private AudioSource buttonPress;
    // Doesnt Work  --  Cant "restart" Game
    // Going back to preload will cause problems! Will comment out for the time being
    public void PlayAgain()
    {
        ButtonSound();
        GameObject.Find("WorldPlayer").GetComponent<PlayerStatus>().EnteringGameWorld(true, 0.0f);
        GameObject.Find("RandomEncounter").GetComponent<Encounter>().playerDead();
        //SceneManager.LoadScene("Preload");
    }

    // Doesnt Work  --  Cant "restart" Game
    public void MainMenu()
    {
        ButtonSound();
        Invoke("Menu", 0.4f);
    }

    public void QuitGame()
    {
        ButtonSound();
        Invoke("Quit", 0.4f);
    }

    private void Menu()
    {
        GameObject.Find("RandomEncounter").GetComponent<Encounter>().playerDead();
        SceneManager.LoadScene("MainMenu");
    }

    private void Quit()
    {
        Application.Quit();
    }

    private void ButtonSound()
    {
        buttonPress.Play();
    }
}
