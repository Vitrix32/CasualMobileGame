using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    private GameObject UniversalAudio;
    private GameObject WorldPlayer;
    public void PlayGame()
    {
        WorldPlayer = GameObject.Find("WorldPlayer");
        UniversalAudio = GameObject.Find("UniversalAudio");
        // Needs to change if we add menu music  --  DONT BE "EXITING COMBAT"
        if (UniversalAudio != null)
        {
            Debug.Log("INTHISFUNCTION");
            UniversalAudio.GetComponent<UniversalAudioHandling>().ExitingCombat();
            WorldPlayer.GetComponent<PlayerStatus>().ExitingCombat();
            WorldPlayer.GetComponent<PlayerStatus>().EnableControl();
            WorldPlayer.transform.position = Vector2.zero;
            SceneManager.LoadScene("GameplayScene");
        }
        else
        {
            UniversalAudio.GetComponent<UniversalAudioHandling>().ExitingCombat();
            SceneManager.LoadScene("Preload");
        }
    }

    public void QuitGame()
    {
        Debug.Log(Application.persistentDataPath);
        Application.Quit();
    }
}
