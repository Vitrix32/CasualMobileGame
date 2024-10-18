using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameObject UniversalAudio;
    private GameObject WorldPlayer;

    void Start()
    {
        /*
        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 1);
            Load();
        }
        else
        {
            Load();
        }*/
        UniversalAudio = GameObject.Find("UniversalAudio");
        WorldPlayer = GameObject.Find("WorldPlayer");
        UniversalAudio.GetComponent<UniversalAudioHandling>().EnteringCombat();
    }
    public void PlayGame()
    {
        //WorldPlayer = GameObject.Find("WorldPlayer");
        //UniversalAudio = GameObject.Find("UniversalAudio");
        // Needs to change if we add menu music  --  DONT BE "EXITING COMBAT"
        UniversalAudio.GetComponent<UniversalAudioHandling>().ExitingCombat();
        WorldPlayer.GetComponent<PlayerStatus>().ExitingCombat();
        WorldPlayer.GetComponent<PlayerStatus>().EnableControl();
        WorldPlayer.transform.position = Vector2.zero;
        SceneManager.LoadScene("GameplayScene");
    }

    public void QuitGame()
    {
        Debug.Log(Application.persistentDataPath);
        Application.Quit();
    }
}
