using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject WorldPlayer;

    public GameObject pauseMenu;
    public GameObject pauseButton;
    public bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        WorldPlayer = GameObject.Find("WorldPlayer");
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
    }

    // Update is called once per frame 
    
    void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                ResumeGame();
            } else
            {
                PauseGame();
            }
        }
        */
    }

    public void PauseGame()
    {
        WorldPlayer.GetComponent<UniversalAudioHandling>().ButtonPressed();
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        pauseButton.SetActive(false);
        WorldPlayer.GetComponent<UniversalAudioHandling>().Pause();
    }

    public void ResumeGame()
    {
        WorldPlayer.GetComponent<UniversalAudioHandling>().ButtonPressed();
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        pauseButton.SetActive(true);
        // Needs to change if we add menu music  --  DONT BE "EXITING COMBAT"
        WorldPlayer.GetComponent<UniversalAudioHandling>().Resume();
    }

    public void GoToMainMenu()
    {
        WorldPlayer.GetComponent<UniversalAudioHandling>().ButtonPressed();
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        isPaused = false;
    }

    public void QuitGame()
    {
        WorldPlayer.GetComponent<UniversalAudioHandling>().ButtonPressed();
        Invoke("Quit", 0.4f);
    }

    private void Quit()
    {
        Application.Quit();
    }
}