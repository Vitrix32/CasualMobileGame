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

    //The build indexes of each scene in the game.
    //The order of these indexes below indicates the order the audio clips are
    //stored in the backgroundMusics and combatMusics arrays.

    private AudioClip backgroundMusic;
    private AudioClip combatMusic;

    // Start is called before the first frame update
    void Start()
    {
        backgroundMusic = backgroundMusics[0];
        combatMusic = combatMusics[0];
    }

    public void EnteringCombat()
    {
        primaryMusicSource.clip = combatMusic;
        primaryMusicSource.Play();
    }

    public void ExitingCombat() 
    {
        primaryMusicSource.clip = backgroundMusic;
        primaryMusicSource.Play();
    }

    public void Die()
    {
        primaryMusicSource.Stop();
    }

    public void Pause()
    {
        primaryMusicSource.Pause();
    }

    public void Resume()
    {
        primaryMusicSource.Play();
    }

    public void NewScene()
    {
        musicSelection();
        primaryMusicSource.clip = backgroundMusic;
        primaryMusicSource.Play();
    }


    private void musicSelection()
    {
       
    }
}
