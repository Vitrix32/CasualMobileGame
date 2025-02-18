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

public class UniversalAudioHandling : MonoBehaviour
{
    [SerializeField]
    private AudioSource primaryMusicSource;
    [SerializeField]
    private AudioSource primaryUISource;
    [SerializeField] 
    private AudioSource primaryAmbianceSource;

    [SerializeField]
    private AudioClip[] backgroundMusics;
    [SerializeField]
    private AudioClip[] combatMusics;

    //The build indexes of each scene in the game.
    //The order of these indexes below indicates the order the audio clips are
    //stored in the backgroundMusics and combatMusics arrays.
    [SerializeField]
    private int mainThemeIndex;
    [SerializeField]
    private int startvilleIndex;
    [SerializeField]
    private int roadToNewCastleIndex;
    [SerializeField]
    private int newCastleIndex;
    [SerializeField]
    private int roadToScorchedMountIndex;
    [SerializeField]
    private int scorchedMountIndex;
    [SerializeField]
    private int shopIndex;

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

    public void ButtonPressed()
    {
        primaryUISource.Play();
    }

    public void NewScene()
    {
        musicSelection();
        primaryMusicSource.clip = backgroundMusic;
        primaryMusicSource.Play();
    }


    private void musicSelection()
    {
        int index = SceneManager.GetActiveScene().buildIndex; //Switch this to SceneName?
        if (index == mainThemeIndex)
        {
            backgroundMusic = backgroundMusics[0];
            combatMusic = combatMusics[0];
        }
        else if (index == startvilleIndex)
        {
            backgroundMusic = backgroundMusics[1];
            //combatMusic = combatMusics[1];
            combatMusic = combatMusics[0];
        }
        else if (index == roadToNewCastleIndex)
        {
            backgroundMusic = backgroundMusics[2];
            combatMusic = combatMusics[2];
        }
        else if (index == newCastleIndex)
        {
            backgroundMusic = backgroundMusics[3];
            combatMusic = combatMusics[3];
        }
        else if (index == roadToScorchedMountIndex)
        {
            backgroundMusic = backgroundMusics[4];
            combatMusic = combatMusics[4];
        }
        else if (index == scorchedMountIndex)
        {
            backgroundMusic = backgroundMusics[5];
            combatMusic = combatMusics[5];
        }
    }
}
