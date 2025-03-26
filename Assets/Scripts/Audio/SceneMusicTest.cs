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
    [SerializeField]
    private AudioSource primaryMusicSource;
    [SerializeField]
    private AudioClip[] backgroundMusics;
    [SerializeField]
    private AudioClip[] combatMusics;

    [SerializeField]
    private GameObject player;

    //The build indexes of each scene in the game.
    //The order of these indexes below indicates the order the audio clips are
    //stored in the backgroundMusics and combatMusics arrays.

    public void Start()
    {
        player = GameObject.Find("WorldPlayer");
        player.GetComponent<UniversalAudioHandling>().NewScene("GameplayScene");
    }

    public void triggerEnter(string name)
    {
        Debug.Log("entered trigger");
        if (player != null)
        {
            player.GetComponent<UniversalAudioHandling>().enteredZone(name);
        }
    }

    public void triggerExit(string name)
    {
        Debug.Log("exited trigger");
        if (player != null)
        {
            player.GetComponent<UniversalAudioHandling>().exitedZone();
        }
    }
}
