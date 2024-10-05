using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject pauseButton;
    public bool isPaused;

    public GameObject UniversalAudio;

    // Start is called before the first frame update
    void Start()
    {
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
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        pauseButton.SetActive(false);
        UniversalAudio = GameObject.Find("UniversalAudio");
        UniversalAudio.GetComponent<UniversalAudioHandling>().Pause();
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        pauseButton.SetActive(true);
        UniversalAudio = GameObject.Find("UniversalAudio");
        // Needs to change if we add menu music  --  DONT BE "EXITING COMBAT"
        UniversalAudio.GetComponent<UniversalAudioHandling>().ExitingCombat();
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        isPaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}