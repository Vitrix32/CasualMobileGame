using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
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
        WorldPlayer = GameObject.Find("WorldPlayer");
    }
    public void PlayGame()
    {
        //WorldPlayer = GameObject.Find("WorldPlayer");
        //UniversalAudio = GameObject.Find("UniversalAudio");
        // Needs to change if we add menu music  --  DONT BE "EXITING COMBAT"
        WorldPlayer.GetComponent<UniversalAudioHandling>().ButtonPressed();
        WorldPlayer.GetComponent<PlayerStatus>().EnteringGameWorld(false, 0.4f);
        WorldPlayer.transform.position = Vector2.zero;
        SceneManager.LoadScene("GameplayScene");
    }

    public void QuitGame()
    {
        WorldPlayer.GetComponent<UniversalAudioHandling>().ButtonPressed();
        Invoke("Quit", 0.4f);
    }

    private void Quit()
    {
        Debug.Log(Application.persistentDataPath);
        Application.Quit();
    }
}
