using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Name: UniversalAudioHandling
 * Author: Isaac Drury
 * Date: January 2025
 * Description:
 * This script handles all music, UI sound effects, and other universal sounds.
 */

public class SceneMusicTest : MonoBehaviour
{
    private GameObject player;
    //The build indexes of each scene in the game.
    //The order of these indexes below indicates the order the audio clips are
    //stored in the backgroundMusics and combatMusics arrays.

    public void Start()
    {
        player = GameObject.Find("WorldPlayer");

    }

    public void triggerEnter(string name)
    {
        Debug.Log("entered trigger");
        if (player != null)
        {
            player.GetComponent<UniversalAudioHandling>().enteredZone(name);
        }
        switch(name)
        {
            case "caveZone":
                PlayerPrefs.SetInt("LocID", 1);
                break;
            case "dungeonZone":
                PlayerPrefs.SetInt("LocID", 2);
                break;
            case "desertZone":
                PlayerPrefs.SetInt("LocID", 4);
                break;
            case "churchZone":
                PlayerPrefs.SetInt("LocID", 3);
                break;
            case "mountainZone":
                PlayerPrefs.SetInt("LocID", 5);
                break;
            default:
                break;

        }
        Debug.Log("Zone " + PlayerPrefs.GetInt("LocID"));

    }

    public void triggerExit(string name)
    {
        
        if (player != null && player.GetComponent<PlayerStatus>().IsInCombat())
        {
            Debug.Log("exited trigger");
            player.GetComponent<UniversalAudioHandling>().exitedZone();
            PlayerPrefs.SetInt("LocID", 0);
        }
        
    }
}
