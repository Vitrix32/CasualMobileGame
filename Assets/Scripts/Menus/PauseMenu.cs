using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject WorldPlayer;

    //public GameObject GameMenu;
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
        //GameMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        pauseButton.SetActive(true);
        // Needs to change if we add menu music  --  DONT BE "EXITING COMBAT"
        WorldPlayer.GetComponent<UniversalAudioHandling>().Resume();
    }

    public void SaveAndExitToMenu()
    {
        SaveGame();
        GoToMainMenu();
    }

    public void DontSaveAndExitToMenu()
    {
        GoToMainMenu();
    }

    private void GoToMainMenu()
    {
        WorldPlayer.GetComponent<UniversalAudioHandling>().ButtonPressed();
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        isPaused = false;
    }

    private void SaveGame()
    {
        string quest = File.ReadAllText(Application.dataPath + "/Scripts/Dialogue/Quests.txt");
        File.WriteAllText(Application.dataPath + "/Scripts/Dialogue/SaveQuests.txt", quest);

        string dialogue = File.ReadAllText(Application.dataPath + "/Scripts/Dialogue/Dialogue.txt");
        File.WriteAllText(Application.dataPath + "/Scripts/Dialogue/SaveDialogue.txt", dialogue);

        string stats = File.ReadAllText(Application.dataPath + "/Scripts/Items/PlayerStats.txt");
        File.WriteAllText(Application.dataPath + "/Scripts/Items/SavePlayerStats.txt", stats);

        //PlayerPrefs.SetInt();
        //PlayerPrefs.SetInt();
    }

    public void QuitGame()
    {
        SaveGame();
        WorldPlayer.GetComponent<UniversalAudioHandling>().ButtonPressed();
        Invoke("Quit", 0.4f);
    }

    private void Quit()
    {
        Application.Quit();
    }
}