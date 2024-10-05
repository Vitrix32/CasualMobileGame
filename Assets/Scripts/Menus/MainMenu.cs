using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    GameObject UniversalAudio;
    public void PlayGame()
    {
        UniversalAudio = GameObject.Find("UniversalAudio");
        // Needs to change if we add menu music  --  DONT BE "EXITING COMBAT"
        if (UniversalAudio != null)
        {
            Debug.Log("INTHISFUNCTION");
            UniversalAudio.GetComponent<UniversalAudioHandling>().ExitingCombat();
            SceneManager.LoadScene("GameplayScene");
        }
        else
        {
            SceneManager.LoadScene("Preload");
        }
    }

    public void QuitGame()
    {
        Debug.Log(Application.persistentDataPath);
        Application.Quit();
    }
}
